using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Billycock_MS_Reusable.Models.Billycock
{
    public class UserPlatform
    {
        public int idUser { get; set; }
        public int idPlatform { get; set; }
        public int GuiID { get; set; }
        public int quantity { get; set; }
        [NotMapped]
        public List<UserPlatformAccount> userPlatformAccounts { get; set; }

        public UserPlatform()
        {
            userPlatformAccounts = new List<UserPlatformAccount>();
        }
    }
}
