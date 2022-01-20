using Microsoft.AspNetCore.Components;
using PRIME_UCR.Domain.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Components.CheckLists.Plantillas
{
    public partial class CheckListMenu
    {
        [Parameter] public IEnumerable<CheckList> Lists { get; set; }
    }
}
