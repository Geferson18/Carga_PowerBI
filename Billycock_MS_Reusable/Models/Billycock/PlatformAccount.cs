using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class PlatformAccount
    {
        public int idPlatform { get; set; }
        public int idAccount { get; set; }
        public int GuiID { get; set; }
        public int freeUsers { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string payDate { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string password { get; set; }
    }
}
