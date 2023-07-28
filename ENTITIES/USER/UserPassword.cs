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
    [SoftDelete]
    public class UserPassword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  ID { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public int IsDeleted { get; set; }
    }
}
