﻿using PaySlipManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlipManagement.BAL.Interfaces
{
    public interface IDocumentBALRepo
    {
        Task<Document> GetByIdAsync(string empcode, string doc);
        Task<bool> Create(Document pdf);
    }
}
