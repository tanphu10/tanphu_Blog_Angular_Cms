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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TPBlog.Data.Repositories
{
    public class AnnouncementRepository : RepositoryBase<IC_Announcement, int>, IAnnouncementRepository
    {

        private readonly IMapper _mapper;

        public AnnouncementRepository(TPBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PageResult<AnnouncementViewModel>> ListAllUnread(Guid userId, int pageIndex, int pageSize)
        {
            var user = _context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result;
            var query = (from x in _context.Announcements
                         join y in _context.AnnouncementUsers on x.Id equals y.AnnouncementId into xy
                         from y in xy.DefaultIfEmpty()
                         where (y.UserId == null || y.UserId == userId)
                         select new AnnouncementViewModel
                         {
                             Id = x.Id,
                             Title = x.Title,
                             Content = x.Content,
                             DateCreated = x.DateCreated,
                             UserId = x.UserId,
                             Status = x.Status,
                             ProjectName = x.ProjectSlug,
                             UserName = user.UserName,
                             HasRead = y.HasRead
                         });

            var totalRow = await query.CountAsync();

            var result = await query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize).ToListAsync();
            //var result = _mapper.Map<>(queryRow);
            return new PageResult<AnnouncementViewModel>
            {
                Results = result,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task<PageResult<AnnouncementViewModel>> GetAllPaging(int pageIndex = 1, int pageSize = 10)
        {


            var query = from a in _context.Announcements
                        join p in _context.Project on a.ProjectSlug equals p.Slug
                        join u in _context.Users on a.UserId equals u.Id
                        select new AnnouncementViewModel()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content,
                            DateCreated = a.DateCreated,
                            UserId = a.UserId,
                            UserName = u.UserName,
                            ProjectName = p.Name,
                            Status = a.Status
                        };

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<AnnouncementViewModel>
            {
                Results = query.ToList(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

        public async Task<PageResult<AnnouncementViewModel>> GetUserAnnoucenmentsAsync(Guid userId)
        {
            var query = await (from au in _context.AnnouncementUsers
                               join a in _context.Announcements on au.AnnouncementId equals a.Id
                               where au.UserId == userId && !au.HasRead
                               select new AnnouncementViewModel
                               {
                                   Id = a.Id,
                                   Title = a.Title,
                                   Content = a.Content,
                                   DateCreated = a.DateCreated
                               }).ToListAsync();
            return new PageResult<AnnouncementViewModel>
            {
                Results = query.ToList(),
            };
        }


    }
}
