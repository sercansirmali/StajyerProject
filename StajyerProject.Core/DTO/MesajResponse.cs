using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajyerProject.Core.DTO
{
    [Table("dt_mesaj")]
    public class MesajResponse
    {
        public int id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public int? Tip { get; set; }
        public string? MesajMetni { get; set; }
        public bool? Durum { get; set; }
        public DateTime? Tarih { get; set; }
        // Additional properties for response
        // Bu iki property veritabanına yazılmasın
        [NotMapped]
        public string StatusMessage { get; set; } = "Success";

        [NotMapped]
        public DateTime ResponseTime { get; set; } = DateTime.Now;
    }
}
