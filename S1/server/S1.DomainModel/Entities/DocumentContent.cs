using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Abstractions.Entities;
using System.ComponentModel.DataAnnotations;

namespace S1.DomainModel
{    
    public class DocumentContent : TenantEntity
    {
        [MaxLength(100)]
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

    }
}
