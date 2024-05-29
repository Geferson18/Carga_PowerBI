using Billycock_MS_Reusable.DTO.Common;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Repositories.Utils.Common
{
    public interface ICommonRepository
    {
        #region Create
        public Task<General<string>> InsertObject(GeneralClass<object> objeto);
        #endregion
        #region Read
        #endregion
        #region Update
        public Task<General<string>> UpdateObject(GeneralClass<object> objeto);
        #endregion
        #region Delete
        public Task<General<string>> DeleteObject(GeneralClass<object> objeto);
        #endregion
        #region Extras
        public Task<General<string>> DescriptionValidation(DescriptionValidationRequest descriptionValidation);
        public Task<General<string>> GetExceptionMessage(ExceptionMessageRequest exceptionMessage);
        //public Task SendQueue(T t);
        public Task<General<string>> RegisterException(RegisterExceptionRequest registerException);
        #endregion
    }
}
