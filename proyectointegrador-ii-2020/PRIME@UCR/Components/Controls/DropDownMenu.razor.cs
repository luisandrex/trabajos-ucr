using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace PRIME_UCR.Components.Controls
{
    public partial class DropDownMenu<T>
    {
        [Parameter] public List<T> Data { get; set; }
        [Parameter] public string TextProperty { get; set; }
        [Parameter] public Func<T, string> TextExpression { get; set; }
        [Parameter] public Func<T, bool> ValueComparer { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public string DefaultText { get; set; }
        [Parameter] public bool UseValidation { get; set; } = true;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public bool Required { get; set; } = true;
        [Parameter] public string Id { get; set; }

        private int _index = -1;
        private string SelectedValue => _index.ToString();

        public string ValidationCssClass
        {
            get
            {
                if (UseValidation)
                {
                    return ValidationUtils.ToBootstrapValidationCss(CssClass);
                }

                return "";
            }    
        }


        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            var index = Int32.Parse(value);
            result = index >= 0 ? Data[index] : default;
            validationErrorMessage = null;
            return true;
        }

        async Task OnChangeEvent(ChangeEventArgs args)
        {
            _index = Int32.Parse((string)args.Value);
            Value = _index >= 0 ? Data[_index] : default;
            await ValueChanged.InvokeAsync(Value);
            if (UseValidation)
                EditContext.NotifyFieldChanged(FieldIdentifier);
        }

        string GetText(T value)
        {
            if (TextExpression != null && TextProperty != null)
            {
                throw new InvalidOperationException("Only select one option of: TextProperty or TextExpression ");
            }
            if (TextExpression != null)
            {
                return TextExpression(value);
            }

            if (TextProperty != null)
            {
                return (string) typeof(T).GetProperty(TextProperty)?.GetValue(value);
            }
            
            // default value
            return value.ToString();
        }

        protected override void OnParametersSet()
        {
            if (Value != null)
            {
                if (ValueComparer != null)
                    _index = Data.IndexOf(Data.FirstOrDefault(ValueComparer));
                else
                    _index = Data.IndexOf(Value);
            }
            else
            {
                _index = -1;
                if (String.IsNullOrEmpty(DefaultText))
                {
                    throw new InvalidOperationException("The DefaultText parameter must be set when the initial value is set to null.");
                }
            }
        }
    }
}
