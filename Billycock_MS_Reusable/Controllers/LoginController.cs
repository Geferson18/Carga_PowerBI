using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Billycock_MS_Reusable.Repositories.Interfaces;
using Billycock_MS_Reusable.Models;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.Repositories.Utils.Common;
using Billycock_MS_Reusable.Models.Utils;
using Billycock_MS_Reusable.DTO.Common;

namespace Billycock.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;
        private readonly ILoginRepository _loginRepository;
        public LoginController(ICommonRepository commonRepository, 
            ILoginRepository loginRepository)
        {
            _commonRepository = commonRepository;
            _loginRepository = loginRepository;
        }

        //[HttpPost]
        //[Route("GetToken")]
        //public async Task<General<string>> PostTokenUser(GeneralClass<object> _tokenUser)
        //{
        //    try
        //    {
        //        if (await _loginRepository.ValidateCredentials(_tokenUser)) return _loginRepository.GetBasicAuthorization(_tokenUser);
        //        else 
        //        {
        //            var mensaje=string.Empty;
        //            if (Globals.Message != string.Empty)
        //            {
        //                mensaje = Globals.Message;
        //            }
        //            else mensaje =  "Error de credenciales";
                    
        //            return new General<string>() { Errors = new List<string>() { mensaje } };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //#region "Sección de Error"
        //        string resource = MethodBase.GetCurrentMethod().DeclaringType.Name.Substring(MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf("<") + 1, MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf(">") - 1);
        //        string Body = JsonConvert.SerializeObject(_tokenUser);

        //        await _commonRepository.RegisterException(new RegisterExceptionRequest() { ex = JsonConvert.SerializeObject(ex), method = resource, input = Body });
        //        //#endregion
        //        return new General<string>() { Errors = new List<string>() { ex.Message } };
        //    }
        //}
    }
}