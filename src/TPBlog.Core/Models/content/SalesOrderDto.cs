using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Models.content
{
    public class SalesOrderDto
    {
        //Order's DocumentNo
        public Guid InvtCategoryId { get; set; }
        public string? Slug { get; set; }
        public string OrderNo { get; set; }
        public List<SaleItemDto> SaleItems { get; set; }

    }
}
