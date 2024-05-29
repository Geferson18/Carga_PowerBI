using System;
using System.ComponentModel.DataAnnotations;

namespace Billycock_MS_Reusable.Models.Utils
{
    public class Correlative
    {
        [Key]
        public Guid guid { get; set; }
        public int idPlatformAccount { get; set; }
        public int idUserPlatform { get; set; }
        public int idUserPlatformAccount { get; set; }
    }
}
