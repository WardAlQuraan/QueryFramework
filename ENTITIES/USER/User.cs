using COMMON.ATTRIBUTES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.USER
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        [RequiredFamily]
        public string RoleCode { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public string? Password { get; set; }

    }
}
