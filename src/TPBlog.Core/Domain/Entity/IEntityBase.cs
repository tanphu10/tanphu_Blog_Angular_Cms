using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Domain.Entity
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
    }
}
