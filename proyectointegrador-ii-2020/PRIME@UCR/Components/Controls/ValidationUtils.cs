namespace PRIME_UCR.Components.Controls
{
    public static class ValidationUtils
    {
        public static string ToBootstrapValidationCss(string cssClass)
        {
            if (cssClass.Contains("modified"))
                return (" " + cssClass)
                    .Replace(" invalid", " is-invalid")
                    .Replace(" valid", " is-valid");
            return (" " + cssClass)
                .Replace(" invalid", "")
                .Replace(" valid", "");
        }
    }
}