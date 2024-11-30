using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class PurchaseProductDto
    {
        public EDocumentType DocumentType => EDocumentType.Purchase;
        public string? ItemNo { get; set; }
        public string? DocumentNo { get; set; }
        public int Quantity { get; set; }
        public string? ExternalDocumentNo { get; set; }
        public string? Notice { get; set; }
        public string? FilePdf { get; set; }
        public Guid ProjectId { get; set; }
        public Guid InvtCategoryId { get; set; }
        public string Slug { get; set; }
    }
}