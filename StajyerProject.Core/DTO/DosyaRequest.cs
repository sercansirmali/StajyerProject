using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace StajyerProject.Core.DTO
{
    public class DosyaRequest
    {
        public string? Adres { get; set; }
        public IFormFile? Dosya { get; set; }
    }
}
