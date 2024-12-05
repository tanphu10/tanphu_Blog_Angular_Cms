using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("IC_InventoryEntries")]
    public class IC_InventoryEntry : EntityBase<Guid>
    {
        public IC_InventoryEntry()
        {
            //DocumentType = EDocumentType.Purchase;
            //DocumentNo = Guid.NewGuid().ToString();
            //ExternalDocumentNo = Guid.NewGuid().ToString();
        }
        public EDocumentType DocumentType { get; set; }

        //--
        public string DocumentNo { get; set; }
        //= Guid.NewGuid().ToString();

        //-- mỗi sản phẩm đều có item No riêng
        public string ItemNo { get; set; }

        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string Slug { get; set; }
        public int Quantity { get; set; }
        //--
        public string ExternalDocumentNo { get; set; }
        //= Guid.NewGuid().ToString();
        public string Notice { get; set; }
        public string[]? Thumbnail { get; set; }
        public string? FilePdf { get; set; }
        public Guid ProjectId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string ProjectSlug { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string InvtCategorySlug { get; set; }
        [MaxLength(250)]
        [Required]
        public required string InvtCategoryName { get; set; }
        [Required]
        public Guid InvtCategoryId { get; set; }
        public string? POUnit { get; set; }
        public string? SOUnit { get; set; }
        public int? CnvFact { get; set; }
        //-- sửa lại stkunit=string
        public string? StkUnit { get; set; }

    }
}
