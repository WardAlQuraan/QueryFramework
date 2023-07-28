using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON.ATTRIBUTES;

namespace ENTITIES.FAMILY
{
    public class FamilyMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string? WifeName { get; set; }
        public int FamilyId { get; set; }
        public string? ImagePath { get; set; }
        [IntBoolValidation]
        public int IsFamilyParent { get; set; } = 0;
    }
}
