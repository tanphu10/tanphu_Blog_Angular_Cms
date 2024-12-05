using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class ProductDto : ProductInListDto
    {

        public string Summary { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_Product, ProductDto>();
            }
        }

    }
}
