using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billycock_MS_Reusable.Models.Utils
{
    public class Audit
    {
        [Key]
        public int idAudit { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string Input { get; set; }
        [Column(TypeName = "varchar(MAX)")]
        public string Output { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string AuditMethod { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string date { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string integration { get; set; }
    }
}
