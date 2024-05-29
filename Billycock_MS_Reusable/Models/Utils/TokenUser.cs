using Billycock_MS_Reusable.DTO.Common;

namespace Billycock_MS_Reusable.Models.Utils
{
    public class TokenUser
    {
        public string Id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public int accessFailedCount { get; set; }
        public bool lockoutEnabled{ get; set; } 
    }
}
