using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPBlog.Core.Domain.Content;
using TPBlog.Core.Shared.Enums;

namespace TPBlog.Core.Models.content
{
    public class InventoryInListDto
    {
        public Guid Id { get; set; }

        public EDocumentType DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public string[]? Thumbnail { get; set; }

        public string? FilePdf { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
        public string ProjectSlug { get; set; }
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
        public string InvtCategorySlug { get; set; }
        public string InvtCategoryName { get; set; }
        public Guid InvtCategoryId { get; set; }
        public string? POUnit { get; set; }
        public string? SOUnit { get; set; }
        public int? CnvFact { get; set; }
        //-- sửa lại stkunit=string
        public string? StkUnit { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_InventoryEntry, InventoryInListDto>();
            }
        }
    }
}
