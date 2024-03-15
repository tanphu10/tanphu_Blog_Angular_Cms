using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Repositories;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class TagRepositiory : RepositoryBase<Tag, Guid>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepositiory(TPBlogContext context, IMapper mapper) : base
            (context)
        {
            _mapper = mapper

        }
    }
}
