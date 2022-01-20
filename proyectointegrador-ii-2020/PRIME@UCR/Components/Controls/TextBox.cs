using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace PRIME_UCR.Components.Controls
{
    public class TextBox : GenericInput<string>
    {
        protected override async Task OnChangeEvent(ChangeEventArgs e)
        {
            Value = (string) e.Value;
            await ValueChanged.InvokeAsync(Value);
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }
    }
}