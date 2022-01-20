using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace PRIME_UCR.Components.Controls
{
    public partial class TabItem<TEnum> where TEnum : notnull
    {
        [Parameter] public TEnum Tab { get; set; }
        [Parameter] public TEnum CurrentTab { get; set; }
        [Parameter] public string TabName { get; set; }
        [Parameter] public EventCallback<TEnum> OnTabSetCallback { get; set; }
        [Parameter] public string Icon { get; set; }
        [Parameter] public string TooltipText { get; set; } = "Campo pendiente";

        async Task OnTabSet()
        {
            await OnTabSetCallback.InvokeAsync(Tab);
        }

        private string _activeClass;

        void SetActiveClass()
        {
            _activeClass = Tab.Equals(CurrentTab) ? "active" : "";
        }
        
        protected override void OnParametersSet()
        {
            SetActiveClass();
        }
    }
}