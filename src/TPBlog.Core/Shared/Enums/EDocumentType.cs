﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Core.Shared.Enums
{
    public enum EDocumentType
    {
        All = 0,// filtering
        Purchase = 101,
        PurchaseInternal = 102,
        Sale = 201,
        SaleInternal = 202,
    }
}
