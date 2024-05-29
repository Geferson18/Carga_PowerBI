using Billycock_MS_Reusable.Service;
using Billycock_MS_Reusable.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Billycock_MS_Reusable.Models.Utils;
using Billycock_MS_Reusable.Repositories.Utils.Common;
using Newtonsoft.Json;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.DTO.Common;
using Billycock_MS_Reusable.Models;
using System.Reflection;
using System.Diagnostics;

namespace Billycock_MS_Reusable.Repositories.Repositories
{
    public class LoginRepository: ILoginRepository
    {
        private readonly BillycockServiceContext _context;
        private readonly ICommonRepository _commonRepository;
        private readonly string Tipo = "TOKENUSER";

        public LoginRepository(BillycockServiceContext context, ICommonRepository commonRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
        }
        public async Task<bool> ValidateCredentials(GeneralClass<object> objeto)
        {
            Globals.Message = string.Empty;
            try
            {
                TokenUser _tokenUser = JsonConvert.DeserializeObject<TokenUser>(objeto.objeto.ToString());
                TokenUser tokenUser = await (from u in _context.TOKENUSER where u.userName == _tokenUser.userName select u).FirstOrDefaultAsync();
                if (tokenUser != null)
                {
                    /***********************Encryption**********************************************/
                    // Get the bytes of the string
                    byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(_tokenUser.password);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes("Password");

                    // Hash the password with SHA256
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                    byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                    string encryptedResult = Convert.ToBase64String(bytesEncrypted);
                    /***********************End*Encryption******************************************/


                    /***********************Decryption**********************************************/
                    // Get the bytes of the string
                    byte[] bytesToBeDecrypted = Convert.FromBase64String(tokenUser.password);
                    byte[] passwordBytesdecrypt = Encoding.UTF8.GetBytes("Password");
                    passwordBytesdecrypt = SHA256.Create().ComputeHash(passwordBytesdecrypt);

                    byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                    string decryptedResult = Encoding.UTF8.GetString(bytesDecrypted);
                    /***********************End*Decryption******************************************/

                    if (_tokenUser.password == decryptedResult && tokenUser.password == encryptedResult) return true;
                    else
                    {
                        objeto.objeto = tokenUser;
                        try
                        {
                            tokenUser.accessFailedCount += 1;
                            await _commonRepository.UpdateObject(objeto);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            await _commonRepository.GetExceptionMessage(new ExceptionMessageRequest() { MessageType = objeto.tipo, tipo = "R" });
                            return false;
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Globals.Message = _commonRepository.GetExceptionMessage(new ExceptionMessageRequest()
                    {
                        MessageType = "R",
                        tipo = Tipo
                    }).Result.Object;
                return false;
            }
        }
        public General<string> GetBasicAuthorization(GeneralClass<object> objeto)
        {
            TokenUser _tokenUser = JsonConvert.DeserializeObject<TokenUser>(objeto.objeto.ToString());
            return new General<string>()
            {
                Success = true,
                Object = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(_tokenUser.userName + ":" + _tokenUser.password))
            };
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
