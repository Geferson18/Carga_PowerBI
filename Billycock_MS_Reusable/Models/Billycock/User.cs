using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class User
    {
        [Key]
        public int idUser { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string description { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string inscriptionDate { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string billing { get; set; }
        public int pay { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string contact { get; set; }
        public int idState { get; set; }
        [NotMapped]
        public List<UserPlatform> userPlatforms { get; set; }

        public User()
        {
            userPlatforms = new List<UserPlatform>();
        }
    }
}
