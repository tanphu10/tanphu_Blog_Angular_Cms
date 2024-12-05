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
    public class InventoryEntryDto
    {
        public Guid Id { get; set; }
        public EDocumentType DocumentType { get; set; }
        public string DocumentNo { get; set; } = Guid.NewGuid().ToString();
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public string ExternalDocumentNo { get; set; } = Guid.NewGuid().ToString();
        public string Notice { get; set; }
        public string[]? Thumbnail { get; set; }
        public string? FilePdf { get; set; }
        public Guid ProjectId { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<IC_InventoryEntry, InventoryEntryDto>();
                CreateMap<InventoryEntryDto, IC_InventoryEntry>();
            }
        }
    }
}
