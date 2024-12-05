using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Models.content
{
    public class CreatedSalesOrderSuccessDto
    {
        public string DocumentNo { get; }
        public CreatedSalesOrderSuccessDto(string documentNo)
        {
            DocumentNo = documentNo;
        }
    }
}
