using Billycock_MS_Reusable.DTO.Common;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.Models.Utils;
using Billycock_MS_Reusable.DTO.Utils;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<bool> ValidateCredentials(GeneralClass<object> _tokenUser);
        General<string> GetBasicAuthorization(GeneralClass<object> _tokenUser);
    }
}
