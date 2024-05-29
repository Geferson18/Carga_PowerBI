using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Utils
{
    public class State
    {
        [Key]
        public int idState { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string description { get; set; }
    }
}
