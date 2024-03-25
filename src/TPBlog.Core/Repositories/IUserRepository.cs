using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Identity;
using TPBlog.Data.SeedWorks;

namespace TPBlog.Core.Repositories
{
    public interface IUserRepository:IRepository<AppUser,Guid>
    {
        Task RemoveUserFromRoles(Guid UserId, string[] roles);
    }
}
