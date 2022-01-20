using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace PRIME_UCR.Components.Controls
{
    public partial class TextAreaInput
    {
        [Parameter] public int? Rows { get; set; }
        [Parameter] public int? Cols { get; set; }

        protected override async Task OnChangeEvent(ChangeEventArgs e)
        {
            Value = (string)e.Value;
            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }
    }
}