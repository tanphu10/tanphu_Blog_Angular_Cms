using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class SaleItemDto
    {
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public EDocumentType DocumentType => EDocumentType.Sale;
    }
}
