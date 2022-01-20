using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PRIME_UCR.Components.Controls
{
    public static class EnumUtils
    {
        // gets description attribute for an enum
        public static string GetDescription<TEnum>(TEnum enumVal) where TEnum : notnull
        {
            if (enumVal is Enum)
            {
                Type genericEnumType = enumVal.GetType();
                MemberInfo[] memberInfo = genericEnumType.GetMember(enumVal.ToString());
                if (memberInfo.Length <= 0) return enumVal.ToString();
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attribs.Length > 0)
                {
                    return ((DescriptionAttribute)attribs.ElementAt(0)).Description;
                }
                return enumVal.ToString();
                
            }

            throw new ArgumentException("Argument must be an enum.");
        }
    }
}