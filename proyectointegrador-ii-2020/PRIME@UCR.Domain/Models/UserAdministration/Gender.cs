using System.ComponentModel;

namespace PRIME_UCR.Domain.Models.UserAdministration
{
    public enum Gender
    {
        [Description("Femenino")]
        Female,
        [Description("Masculino")]
        Male,
        [Description("Otro")]
        Unspecified
    }
}