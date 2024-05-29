using System.Collections.Generic;

namespace Billycock_MS_Reusable.DTO.Utils
{
    public class General<T> where T : class
    {
        public bool Success { get; set; } = false;
        public List<string> Errors { get; set; }
        public List<T> List { get; set; }
        public T Object {get;set;}

        public General()
        {
            Errors = new List<string>();
            List = new List<T>();
        }
    }
}
