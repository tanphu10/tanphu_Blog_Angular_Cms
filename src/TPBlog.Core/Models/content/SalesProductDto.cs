using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public record SalesProductDto(string ExternalDocumentNo, int Quantity)
    {
        public EDocumentType DocumentType = EDocumentType.Sale;
        public string ItemNo { get; set; }
        public string? Notice { get; set; }
        public string? FilePdf { get; set; }
        public Guid ProjectId { get; set; }
        public void SetItemNo(string itemNo)
        {
            ItemNo = itemNo;
        }
    }
}
