using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class Platform
    {
        [Key]
        public int idPlatform { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string description { get; set; }
        public int numberMaximumUsers { get; set; }
        public int cost { get; set; }
        public int lowPrice { get; set; }
        public int highPrice { get; set; }
        public int idState { get; set; }
    }
}
