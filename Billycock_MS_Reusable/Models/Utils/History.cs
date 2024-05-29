using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models.Utils
{
    public class History
    {
        [Key]
        public int idHistory {get;set;}
        [Column(TypeName = "varchar(MAX)")]
        public string Request { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string Response { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string date { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string integration { get; set; }
        public string target { get; set; }
    }
}
