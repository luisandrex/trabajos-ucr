using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace PRIME_UCR.Components.Controls
{
    public partial class TabMenu<TEnum> where TEnum : notnull
    {
        [Parameter] public bool IsVertical { get; set; }
        [Parameter] public TEnum DefaultTab { get; set; }
        [Parameter] public TEnum Value { get; set; } // used for data binding
        [Parameter] public EventCallback<TEnum> ValueChanged { get; set; }
        [Parameter] public IEnumerable<Tuple<TEnum, string>> Tabs { get; set; }
        [Parameter] public string TooltipText { get; set; }

        private string _tabClass;

        async Task OnTabSet(TEnum tab)
        {
            await ValueChanged.InvokeAsync(tab);
        }

        void SetTabClass()
        {
            _tabClass = IsVertical ? "nav nav-pills flex-column" : "nav nav-pills";
        }
        
        protected override void OnParametersSet()
        {
            SetTabClass();
        }
    }
}