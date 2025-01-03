﻿using Microsoft.EntityFrameworkCore;
using TPBlog.Api.Services.IServices;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Models.content;
using TPBlog.Core.Repositories;
using TPBlog.Data;
using TPBlog.Data.Repositories;
using static TPBlog.Core.SeedWorks.Contants.Permissions;

namespace TPBlog.Api.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _repository;
        private readonly IAnnouncementUserRepository _announUserRepository;
        private readonly TPBlogContext _context;
        public AnnouncementService(IAnnouncementRepository repository, IAnnouncementUserRepository announUserRepository, TPBlogContext context)
        {
            _repository = repository;
            _announUserRepository = announUserRepository;
            _context = context;
        }

        public async Task CreateAsync(IC_Announcement newAnnoun)
        {

            await _repository.Add(newAnnoun);

            await _context.SaveChangesAsync(); // Hoặc _context.SaveChangesAsync();
        }
        public async Task MarkAsRead(Guid userId, int notificationId)
        {
            var announ = await _announUserRepository.GetSingleByCondition(x => x.AnnouncementId == notificationId && x.UserId == userId);

            if (announ == null)
            {
                await _announUserRepository.Add(new IC_AnnouncementUser()
                {
                    AnnouncementId = notificationId,
                    UserId = userId,
                    HasRead = true
                });
            }
            else
            {
                announ.HasRead = true;
            }
            await _context.SaveChangesAsync(); // Hoặc _context.SaveChangesAsync();
        }

    }
}
