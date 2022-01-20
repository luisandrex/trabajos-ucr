using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.Multimedia
{
    public enum AlertType
    {
        Primary, Danger
    }

    public class MAlertMessage
    {
        public AlertType AlertType { get; set; }
        public string Message { get; set; }
    }

    public partial class MultimediaAlert
    {
        [Parameter]
        public MAlertMessage Message { get; set; }

        string GetMessageClass()
        {
            string mClass = "hidden";
            switch (Message.AlertType)
            {
                case AlertType.Primary:
                    mClass = "alert alert-primary";
                    break;
                case AlertType.Danger:
                    mClass = "alert alert-danger";
                    break;
                default:
                    break;
            }
            return mClass;
        }
    }
}
