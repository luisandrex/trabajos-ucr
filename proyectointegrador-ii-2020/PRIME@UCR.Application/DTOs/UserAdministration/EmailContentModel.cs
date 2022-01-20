using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class EmailContentModel
    {
        public string Destination { get; set; }

        public string Subject { get; set; }
        
        public string Body { get; set; }

        public string AttachmentPath { get; set; }
    }
}
