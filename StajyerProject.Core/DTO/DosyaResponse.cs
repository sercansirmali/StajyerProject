using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajyerProject.Core.DTO
{
    public class DosyaResponse
    {
        public int Id { get; set; }
        public string? Dosya { get; set; }
        public string? Adres { get; set; }
        public DateTime? Tarih { get; set; }
    }
}
