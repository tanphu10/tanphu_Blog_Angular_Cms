using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Entity;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_UnitConversion")]

    public class IC_UnitConversion : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid InvtId { get; set; }

        [Required]
        [Column(TypeName = "varchar(250)")]
        public string FromUnit { get; set; }

        [Required]
        [Column(TypeName = "varchar(250)")]
        public string ToUnit { get; set; }
        public int CnvFactor { get; set; }
        public string MultDiv { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string FromUnitDescr { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string ToUnitDescr { get; set; }
    }
}
