using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class Account
    {
        [Key]
        public int idAccount { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string email { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string diminutive { get; set; }
        public int idState { get; set; }
    }
}
