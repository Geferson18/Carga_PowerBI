using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class UserPlatformAccount
    {
        public int idUser { get; set; }
        public int idPlatform { get; set; }
        public int idAccount { get; set; }
        public int GuiID { get; set; }
        public string pin { get; set; }
    }
}
