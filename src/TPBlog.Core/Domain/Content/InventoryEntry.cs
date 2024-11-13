using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TPBlog.Core.Domain.Entity;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Domain.Content
{
    [Table("InventoryEntries")]
    public class InventoryEntry : EntityBase<Guid>
    {
        public InventoryEntry()
        {
            //DocumentType = EDocumentType.Purchase;
            //DocumentNo = Guid.NewGuid().ToString();
            //ExternalDocumentNo = Guid.NewGuid().ToString();
        }
        public EDocumentType DocumentType { get; set; }
        public string DocumentNo { get; set; } 
            //= Guid.NewGuid().ToString();
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public string ExternalDocumentNo { get; set; } 
            //= Guid.NewGuid().ToString();
        public string Notice { get; set; }
        public string[]? Thumbnail { get; set; }
        public string? FilePdf { get; set; }
        public Guid ProjectId { get; set; }
    }
}
