using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Core.SeedWorks.Contants;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Data.Repositories
{
    public class AnnouncementRepository : RepositoryBase<Announcement, int>, IAnnouncementRepository
    {

        private readonly IMapper _mapper;

        public AnnouncementRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PageResult<Announcement>> ListAllUnread(Guid userId, int pageIndex, int pageSize)
        {
            var query = (from x in _context.Announcements
                         join y in _context.AnnouncementUsers on x.Id equals y.AnnouncementId into xy
                         from y in xy.DefaultIfEmpty()
                         where (y.HasRead == null || y.HasRead == false)
                               && (y.UserId == null || y.UserId == userId)
                         select x);
            //.Include(x => x.AppUser);
            //totalRow = query.Count();4
            var totalRow = await query.CountAsync();

            var result =await query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize).ToListAsync();
            return new PageResult<Announcement>
            {
                //Results = await _mapper.ProjectTo<Announcement>(res).ToListAsync(),
                Results = result, // `result` is now awaited and gives List<Announcement>
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task<PageResult<AnnouncementViewModel>> GetAllPaging(int pageIndex = 1, int pageSize = 10)
        {


            var query = _context.Announcements.AsQueryable();


            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<AnnouncementViewModel>
            {
                Results = await _mapper.ProjectTo<AnnouncementViewModel>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

        //public IQueryable<Announcement> GetAllUnread(Guid userId)
        //{
        //    var query = (from x in _context.Announcements.Include("AppUser")
        //                 join u in _context.AnnouncementUsers.DefaultIfEmpty()
        //                 on x.Id equals u.AnnouncementId
        //                 where (u.UserId == null || u.UserId == userId) && (u.HasRead == null || u.HasRead == false)
        //                 select x);

        //    //.Include(x => x.AppUser);
        //    return query;
        //}
    }
}
