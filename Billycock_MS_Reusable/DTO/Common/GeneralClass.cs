using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.DTO.Common
{
    public class GeneralClass<T> where T : class
    {
        public string integration { get; set; }
        public string target { get; set; }
        public T objeto { get; set; }
        public string tipo { get; set; }
    }
}
