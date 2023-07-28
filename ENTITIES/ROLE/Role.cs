using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.ROLE
{
    public class Role
    {
        [Key]
        public string RoleCode { get; set; }
    }
}
