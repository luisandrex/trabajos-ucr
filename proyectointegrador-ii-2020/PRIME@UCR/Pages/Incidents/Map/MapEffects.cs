#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Fluxor;
using PRIME_UCR.Application.Services.Incidents;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;

namespace PRIME_UCR.Pages.Incidents.Map
{
    public class MapEffects : IDisposable
    {
        private const int PeriodInMs = (int)(5 * 60 * 1e3); // 5 minutes in milliseconds
        private readonly IGpsDataService _service;
        private IDispatcher? _dispatcher;
        private Timer? _timer;
        private Modalidad? _unitFilter;
        private Estado? _stateFilter;

        public MapEffects(IGpsDataService service)
        {
            _service = service;
        }

        [EffectMethod]
        public Task HandleLoadGpsDataWithUnitFilterAction(LoadGpsDataWithUnitFilterAction action, IDispatcher dispatcher)
        {
            _unitFilter = action.UnitTypeFilter;
            _dispatcher ??= dispatcher;
            RefreshGpsData();

            return Task.CompletedTask;
        }


        [EffectMethod]
        public Task HandleLoadGpsDataWithStateFilterAction(LoadGpsDataWithStateFilterAction action, IDispatcher dispatcher)
        {
            _stateFilter = action.StateFilter;
            _dispatcher ??= dispatcher;
            RefreshGpsData();

            return Task.CompletedTask;
        }

        [EffectMethod]
        public async Task HandleLoadGpsDataAction(LoadGpsDataAction action, IDispatcher dispatcher)
        {
            if (_timer == null)
            {
                _timer = new Timer(state => RefreshGpsData(), null, PeriodInMs, PeriodInMs);
            }

            dispatcher.Dispatch(new LoadingGpsDataAction());

            var gpsData = await _service.GetGpsDataAsync(_unitFilter, _stateFilter);

            var unitFilters = await _service.GetGpsDataUnitFiltersAsync();
            var stateFilters = await _service.GetGpsDataStateFiltersAsync();

            dispatcher.Dispatch(new LoadGpsDataSuccessfulAction(
                gpsData,
                unitFilters,
                _unitFilter,
                stateFilters,
                _stateFilter
            ));
        }

        private void RefreshGpsData()
        {
            _dispatcher?.Dispatch(new LoadGpsDataAction());
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
