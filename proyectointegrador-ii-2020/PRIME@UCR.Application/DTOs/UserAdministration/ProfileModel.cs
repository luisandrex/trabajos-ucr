using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Application.DTOs.UserAdministration
{
    public class ProfileModel
    {
        public string ProfileName { get; set; }

        public string StatusMessage { get; set; }

        public string StatusMessageType { get; set; }

        public List<Tuple<string,bool>> CheckedUsers { get; set; }

        public List<bool> CheckedPermissions { get; set; }

        public List<Usuario> UserLists { get; set; }

        public List<Permiso> PermissionsList { get; set; }
    }
}
