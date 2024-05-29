using Billycock_MS_Reusable.Service;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.Models;
using Billycock_MS_Reusable.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Billycock_MS_Reusable.Models.Billycock;
using System.Diagnostics;
using Billycock_MS_Reusable.DTO.Common;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace Billycock_MS_Reusable.Repositories.Utils.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly BillycockServiceContext _context;
        private readonly IConfiguration _configuration;
        public CommonRepository(
            BillycockServiceContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #region Metodos Principales
        #region Create
        public async Task<General<string>> InsertObject(GeneralClass<object> objeto)
        {
            General<string> respuesta;
            try
            {
                await transactObjetoAsync(objeto, TransactionalProcess.CREATION_PROCESS);
                respuesta = new General<string>() { Success = true, Object = CreateMessage(objeto.tipo, TransactionalProcess.CREATION) };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string resource = MethodBase.GetCurrentMethod().DeclaringType.Name.Substring(MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf("<") + 1, MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf(">") - 1);
                RegisterErrorMethod(ex, resource);
                await RegisterAudit(objeto.integration, resource, JsonConvert.SerializeObject(objeto), CreateMessage(objeto.tipo, TransactionalProcess.NOCREATION));
                respuesta = new General<string>() { Errors = new List<string>() { CreateMessage(objeto.tipo, TransactionalProcess.NOCREATION) } };
            }
            await InsertHistory(objeto,respuesta.Success ? respuesta.Object:JsonConvert.SerializeObject(respuesta.Errors));
            return respuesta;
        }

        public async Task InsertHistory(GeneralClass<object> objeto,string respuesta)
        {
            try
            {
                objeto.objeto = await GetObjetoAsync(objeto, TransactionalProcess.HISTORY);
                History history = new History()
                {
                    Request = JsonConvert.SerializeObject(objeto),
                    Response = respuesta,
                    date = GetDateTime(),
                    integration = objeto.integration,
                    target = objeto.target
                };
                await Save(1, history);
            }
            catch (Exception ex)
            {
                string resource = MethodBase.GetCurrentMethod().DeclaringType.Name.Substring(MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf("<") + 1, MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf(">") - 1);
                RegisterErrorMethod(ex, resource);
                await RegisterAudit(objeto.integration, resource, JsonConvert.SerializeObject(objeto), CreateMessage(objeto.tipo, TransactionalProcess.NOHISTORY));
                Console.WriteLine(ex.Message);
            }
            //IteratedMessages();
        }
        #endregion
        #region Read
        #endregion
        #region Update
        public async Task<General<string>> UpdateObject(GeneralClass<object> objeto)
        {
            General<string> respuesta;
            try
            {
                await transactObjetoAsync(objeto, TransactionalProcess.UPDATE_PROCESS);
                respuesta = new General<string>() { Success = true, Object = CreateMessage(objeto.tipo, TransactionalProcess.UPDATE) };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string resource = MethodBase.GetCurrentMethod().DeclaringType.Name.Substring(MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf("<") + 1, MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf(">") - 1);
                RegisterErrorMethod(ex, resource);
                await RegisterAudit(objeto.integration, resource, JsonConvert.SerializeObject(objeto), CreateMessage(objeto.tipo, TransactionalProcess.NOUPDATE));
                respuesta = new General<string>() { Errors = new List<string>() { CreateMessage(objeto.tipo, TransactionalProcess.NOUPDATE) } };
            }
            await InsertHistory(objeto, respuesta.Success ? respuesta.Object : JsonConvert.SerializeObject(respuesta.Errors));
            return respuesta;
        }
        #endregion
        #region Delete
        public async Task<General<string>> DeleteObject(GeneralClass<object> objeto)
        {
            General<string> respuesta;
            try
            {
                await transactObjetoAsync(objeto, TransactionalProcess.ELIMINATION_PROCESS);
                respuesta = new General<string>() { Success = true, Object = CreateMessage(objeto.tipo, TransactionalProcess.ELIMINATION) };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string resource = MethodBase.GetCurrentMethod().DeclaringType.Name.Substring(MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf("<") + 1, MethodBase.GetCurrentMethod().DeclaringType.Name.IndexOf(">") - 1);
                RegisterErrorMethod(ex, resource);
                await RegisterAudit(objeto.integration, resource, JsonConvert.SerializeObject(objeto), CreateMessage(objeto.tipo, TransactionalProcess.NOELIMINATION));
                respuesta = new General<string>() { Errors = new List<string>() { CreateMessage(objeto.tipo, TransactionalProcess.NOELIMINATION) } };
            }
            await InsertHistory(objeto, respuesta.Success ? respuesta.Object : JsonConvert.SerializeObject(respuesta.Errors));
            return respuesta;
        }
        #endregion
        #region Extras
        public async Task<General<string>> DescriptionValidation(DescriptionValidationRequest descriptionValidation)
        {
            General<string> respuesta = new General<string>() { Object = "VALIDACION CORRECTA", Success = true };
            foreach (var item in descriptionValidation.descriptions)
            {
                if (item.ToUpper() == descriptionValidation.descriptiontoValidate.ToUpper())
                {
                    respuesta.Success = false;
                    respuesta.Object = respuesta.Object.Replace("CORRECTA", "INCORRECTA");
                    break;
                }
            }
            if (respuesta.Success == false) respuesta.Object = await GetDuplicateMessage(descriptionValidation.tipo);
            return respuesta;
        }
        public async Task<General<string>> GetExceptionMessage(ExceptionMessageRequest exceptionMessageRequest)
        {
            string message = ExceptionMessage.message;
            if (exceptionMessageRequest.MessageType == "C")
            {
                message = message.Replace("@PROCESS@", "CREACION");
            }
            else if (exceptionMessageRequest.MessageType == "R")
            {
                message = message.Replace("@PROCESS@", "LECTURA");
            }
            else if (exceptionMessageRequest.MessageType == "U")
            {
                message = message.Replace("@PROCESS@", "ACTUALIZACION");
            }
            else if (exceptionMessageRequest.MessageType == "D")
            {
                message = message.Replace("@PROCESS@", "ELIMINACION");
            }
            return new General<string>() { Success = true, Object = CreateMessage(exceptionMessageRequest.tipo, message) };
        }
        public async Task<General<string>> RegisterException(RegisterExceptionRequest registerExceptionRequest)
        {
            RegisterErrorMethod(JsonConvert.DeserializeObject<Exception>(registerExceptionRequest.ex), registerExceptionRequest.method);

            return new General<string>() { Success = true, Object = await RegisterAudit(registerExceptionRequest.integration, registerExceptionRequest.method, registerExceptionRequest.input, JsonConvert.DeserializeObject<Exception>(registerExceptionRequest.ex).Message) };
        }
        #endregion
        #endregion
        #region Extras
        public async Task<string> GetDuplicateMessage(string tipo)
        {
            return CreateMessage(tipo, DuplicateMessage.message);
            //IteratedMessages();
        }
        private async Task transactObjetoAsync(GeneralClass<object> objeto, int proceso)
        {
            try
            {
                object objectTemporal = await GetObjetoAsync(objeto, proceso);

                await Save(proceso, objectTemporal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task Save(int proceso, object objectTemporal)
        {
            var dbContext = _context;

            if (proceso == 1) dbContext.Entry(objectTemporal).State = EntityState.Added;
            if (proceso == 2) dbContext.Entry(objectTemporal).State = EntityState.Modified;
            if (proceso == 3) dbContext.Entry(objectTemporal).State = EntityState.Deleted;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627) // 2627 es el código de error para una violación de clave primaria en SQL Server
            {
                // Limpiar el contexto
                foreach (var entry in dbContext.ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }

                // Manejar la excepción según tus necesidades
                Console.WriteLine("Se produjo una violación de clave primaria. El contexto ha sido limpiado.");
                throw;
            }
        }
        public async Task<string> RegisterAudit(string integration, string method, string input, string output)
        {
            try
            {
                Audit audit = new Audit()
                {
                    AuditMethod = method,
                    Input = input,
                    Output = output,
                    date = DateTime.Now.ToString(),
                    integration = integration
                };
                await Save(1, audit);

                /*-----------------NOTIFICACION AL CORREO----------------------------------------------------*/
                String fecha = DateTime.Now.ToString("yyyyMMddHHmmss");
                string archivoInput = "Input_" + fecha + ".txt";
                string archivoOutput = "Output_" + fecha + ".txt";

                // Create random data to write to the file.
                byte[] byIUnput = Encoding.ASCII.GetBytes(input);
                byte[] byOutput = Encoding.ASCII.GetBytes(output);

                using (FileStream
                    fileStream = new FileStream(archivoInput, FileMode.Create))
                {
                    fileStream.Write(byIUnput);
                    fileStream.Seek(0, SeekOrigin.Begin);
                }

                using (FileStream
                    fileStream = new FileStream(archivoOutput, FileMode.Create))
                {
                    fileStream.Write(byOutput);
                    fileStream.Seek(0, SeekOrigin.Begin);
                }

                NotificacionRequest objNotificacion = new NotificacionRequest();

                String FechaHoy = DateTime.Now.ToString("yyyyMMdd HHmmss");
                String cod_error = "01";
                String desc_error = "ERROR EN EL SERVICIO";

                String asunto = String.Format(Mail.ASUNTO_ERROR_DATA, FechaHoy);
                String mensaje = String.Format(Mail.CUERPO_ERROR_DATA, cod_error, desc_error);

                objNotificacion.asunto = asunto;
                objNotificacion.mensaje = mensaje;
                objNotificacion.listaArchivos = new List<string>
                {
                    archivoInput,
                    archivoOutput
                };
                Notify(objNotificacion);

                foreach (var item in objNotificacion.listaArchivos)
                {
                    File.Delete(item);
                }
                /*-----------------------------------------------------------------------------------*/
            }
            catch (Exception ex)
            {
                RegisterErrorMethod(ex, "RegisterAudit");
            }
            return CreateMessage("AUDIT", TransactionalProcess.CREATION);
        }
        public string CreateMessage(string tipo, string BaseMessage)
        {
            return BaseMessage.Replace("@TABLE@", tipo);
        }
        public async Task<object> GetObjetoAsync(GeneralClass<object> objeto, int proceso)
        {
            try
            {
                if (objeto.tipo == "TOKENUSER")
                {
                    return JsonConvert.DeserializeObject<TokenUser>(objeto.objeto.ToString());
                }
                else if (objeto.tipo == "STATE")
                {
                    return JsonConvert.DeserializeObject<State>(objeto.objeto.ToString());
                }
                else if (objeto.tipo == "PLATFORM")
                {
                    return JsonConvert.DeserializeObject<Platform>(objeto.objeto.ToString());
                }
                else if (objeto.tipo == "ACCOUNT")
                {
                    return JsonConvert.DeserializeObject<Account>(objeto.objeto.ToString());
                }
                else if (objeto.tipo == "PLATFORMACCOUNT")
                {
                    var objecto = JsonConvert.DeserializeObject<PlatformAccount>(objeto.objeto.ToString());
                    if (proceso == TransactionalProcess.CREATION_PROCESS || proceso == TransactionalProcess.HISTORY)
                    {
                        return new PlatformAccount()
                        {
                            idPlatform = objecto.idPlatform,
                            idAccount = objecto.idAccount,
                            payDate = proceso == TransactionalProcess.CREATION_PROCESS ? GetDate(DateTime.Now.AddMonths(1)):objecto.payDate,
                            password = objecto.password,
                            freeUsers = proceso == TransactionalProcess.CREATION_PROCESS ? await _context.PLATFORM.Where(p => p.idPlatform == objecto.idPlatform).Select(p => p.numberMaximumUsers).FirstOrDefaultAsync():objecto.freeUsers,
                            GuiID = proceso == TransactionalProcess.CREATION_PROCESS ? await GetCorrelativeCode(objeto.tipo) : objecto.GuiID
                        };
                    }
                    return objecto;
                }
                else if (objeto.tipo == "USER")
                {
                    var objecto = JsonConvert.DeserializeObject<User>(objeto.objeto.ToString());

                    return new User()
                    {
                        idUser = objecto.idUser,
                        description = objecto.description,
                        contact = objecto.contact,
                        idState = objecto.idState,
                        inscriptionDate = proceso == TransactionalProcess.CREATION_PROCESS ? GetDateTime():objecto.inscriptionDate,
                        billing = proceso == TransactionalProcess.CREATION_PROCESS ? GetBillingDateofUser():objecto.billing,
                        pay = GetUserPayAmount(objecto.userPlatforms)
                    };
                }
                else if (objeto.tipo == "USERPLATFORM")
                {
                    var objecto = JsonConvert.DeserializeObject<UserPlatform>(objeto.objeto.ToString());
                    if (proceso == TransactionalProcess.CREATION_PROCESS || proceso == TransactionalProcess.HISTORY)
                    {
                        return new UserPlatform()
                        {
                            idUser = objecto.idUser,
                            idPlatform = objecto.idPlatform,
                            quantity = objecto.quantity,
                            GuiID = proceso == TransactionalProcess.CREATION_PROCESS ? await GetCorrelativeCode(objeto.tipo) : objecto.GuiID
                        };
                    }
                    return objecto;
                }
                else if (objeto.tipo == "USERPLATFORMACCOUNT")
                {
                    var objecto = JsonConvert.DeserializeObject<UserPlatformAccount>(objeto.objeto.ToString());
                    if (proceso == TransactionalProcess.CREATION_PROCESS || proceso == TransactionalProcess.HISTORY)
                    {
                        return new UserPlatformAccount()
                        {
                            idUser = objecto.idUser,
                            idPlatform = objecto.idPlatform,
                            idAccount = objecto.idAccount,
                            GuiID = proceso == TransactionalProcess.CREATION_PROCESS ? await GetCorrelativeCode(objeto.tipo) : objecto.GuiID,
                            pin = objecto.pin
                        };
                    }
                    return objecto;
                }
                else return objeto.objeto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        //public async Task SendQueue(T t)
        //{
        //    //Mandar a cola
        //    // the client that owns the connection and can be used to create senders and receivers
        //    ServiceBusClient client;

        //    // the sender used to publish messages to the queue
        //    ServiceBusSender sender;
        //    client = new ServiceBusClient(_configuration["Service_Bus"]);
        //    sender = client.CreateSender(_configuration["Queue"]);
        //    try
        //    {
        //        ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
        //        messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(t)));
        //        await sender.SendMessagesAsync(messageBatch);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}
        public void RegisterErrorMethod(Exception ex, String descripcion_metodo)
        {
            try
            {
                string rutaLog = _configuration["Folder_Error"].ToString().Replace("@", DateTime.Now.ToShortDateString().Replace("/", "_"));

                if (ex != null)
                {
                    using (StreamWriter file = new StreamWriter(rutaLog, true))
                    {

                        file.WriteLine("-----           -INICIO DE ERROR          ----");
                        file.WriteLine("Empresa                  : Billycock");
                        file.WriteLine("Elaborado                : Programador");
                        file.WriteLine("Proyecto                 : WS Billycock");
                        file.WriteLine("Fecha                    : " + DateTime.Now.ToLongDateString()); // Escribe la Traza
                        file.WriteLine("Hora de incidencia       : " + DateTime.Now.ToString("hh:mm:ss").ToString()); // Escribe la Traza
                        file.WriteLine("Metodo         :   " + descripcion_metodo);// escribe el mensaje
                        file.WriteLine("Mensaje de Error         :   " + ex.Message);// escribe el mensaje
                        file.WriteLine(ex.HelpLink); // Escribe la Traza
                        file.WriteLine(ex.Source); // Escribe la Traza
                        file.WriteLine(ex.StackTrace); // Escribe la Traza
                        if (ex.TargetSite != null) file.WriteLine(ex.TargetSite.ToString()); // Escribe la Traza

                        file.WriteLine("-----           - FIN DE ERROR           ----");
                    }
                }
                else
                {
                    using (StreamWriter file = new StreamWriter(rutaLog, true))
                    {

                        file.WriteLine("-----           -INICIO DE ERROR          ----");
                        file.WriteLine("Empresa                  : Billycock");
                        file.WriteLine("Elaborado                : Programador");
                        file.WriteLine("Proyecto                 : WS Billycock");
                        file.WriteLine("Fecha                    : " + DateTime.Now.ToLongDateString());
                        file.WriteLine("Hora de incidencia       : " + DateTime.Now.ToString("hh:mm:ss").ToString());
                        file.WriteLine("Metodo   o  lugar      :   " + descripcion_metodo);
                        file.WriteLine("-----           - FIN DE ERROR           ----");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                throw;
            }
        }
        public void Notify(NotificacionRequest objNotificacion)
        {

            MailMessage objMensaje = new MailMessage();
            objMensaje = GenerateNotify(objNotificacion);
            SmtpClient envio = new SmtpClient();

            //Parametro objParametroDL = new Parametro();
            //ParametroRequest objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.CorreoSISTEMA;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            string correoEmisor = _configuration["Mail_Transmitter"];

            //objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.ContrasenaCorreoSISTEMA;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            string correoEmisorcontrasena = _configuration["Mail_Transmitter_Password"];

            //objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.HostSISTEMA;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            string host = _configuration["Mail_Host"];

            //objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.PuertoSISTEMA;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            Int32 puerto = Int32.Parse(_configuration["Mail_Port"]);

            //objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.SslSISTEMA;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            Boolean ssl = Boolean.Parse(_configuration["Mail_Ssl"]);

            objMensaje.From = new MailAddress(correoEmisor);
            envio.Credentials = new NetworkCredential(correoEmisor, correoEmisorcontrasena);

            envio.Host = host;
            envio.Port = puerto;
            envio.EnableSsl = ssl;
            envio.Send(objMensaje);
            envio.Dispose();

            if (objMensaje.Attachments != null)
            {
                objMensaje.Attachments.Dispose();
            }
        }
        public MailMessage GenerateNotify(NotificacionRequest objNotificacion)
        {
            MailMessage correos = new MailMessage();

            var mensaje = String.Format(objNotificacion.mensaje);
            AlternateView html = AlternateView.CreateAlternateViewFromString(mensaje, new System.Net.Mime.ContentType("text/html; charset=UTF-8"));
            correos.AlternateViews.Add(html);

            correos.To.Clear();
            correos.Body = mensaje;
            correos.BodyEncoding = Encoding.UTF8;
            correos.Subject = objNotificacion.asunto;
            correos.SubjectEncoding = Encoding.UTF8;
            correos.IsBodyHtml = true;

            //Parametro objParametroDL = new Parametro();
            //ParametroRequest objParametroBE = new ParametroRequest();
            //objParametroBE.idParametro = (int)Constantes.EnumParametros.CorreoSoporte;
            //objParametroBE = objParametroDL.consultarParametro(objParametroBE);
            string correoPara = _configuration["Mail_Receiver"];

            objNotificacion.para = correoPara;

            if (objNotificacion.para != null)
            {
                if (objNotificacion.para != "") { correos.To.Add(objNotificacion.para); }
            }
            if (objNotificacion.copia != null)
            {
                if (objNotificacion.copia != "") { correos.CC.Add(objNotificacion.copia); }
            }
            if (objNotificacion.copiaOculta != null)
            {
                if (objNotificacion.copiaOculta != "") { correos.Bcc.Add(objNotificacion.copiaOculta); }
            }

            if (objNotificacion.listaArchivos != null)
            {
                foreach (String archivo in objNotificacion.listaArchivos)
                {
                    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    if (System.IO.File.Exists(archivo))
                    {
                        Attachment data = new Attachment(archivo);
                        correos.Attachments.Add(data);
                    }
                }
            }

            return correos;
        }
        public string GetDateTime()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZone"]);
            DateTime cstTime = TimeZoneInfo.ConvertTime(DateTime.Now, cstZone);

            return cstTime.ToString();
        }
        //public void IteratedMessages()
        //{
        //    string message = Globals.Message;
        //    if (Globals.Message_S != string.Empty && Globals.Message_S != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_S;
        //        Globals.Message_S = string.Empty;
        //    }
        //    if (Globals.Message_U != string.Empty && Globals.Message_U != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_U;
        //        Globals.Message_U = string.Empty;
        //    }
        //    if (Globals.Message_P != string.Empty && Globals.Message_P != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_P;
        //        Globals.Message_P = string.Empty;
        //    }
        //    if (Globals.Message_A != string.Empty && Globals.Message_A != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_A;
        //        Globals.Message_A = string.Empty;
        //    }
        //    if (Globals.Message_PA != string.Empty && Globals.Message_PA != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_PA;
        //    }
        //    if (Globals.Message_UP != string.Empty && Globals.Message_UP != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_UP;
        //    }
        //    if (Globals.Message_UPA != string.Empty && Globals.Message_UPA != null)
        //    {
        //        if (message != string.Empty) message += Environment.NewLine;
        //        message += Globals.Message_UPA;
        //    }

        //    Globals.Message = message;
        //}
        public string GetDate(DateTime fecha)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZone"]);
            DateTime cstTime;
            try
            {
                cstTime = TimeZoneInfo.ConvertTime(fecha, cstZone);
            }
            catch
            {
                cstTime = TimeZoneInfo.ConvertTime(DateTime.Now, cstZone);
            }

            return cstTime.ToShortDateString();
        }
        public async Task<int> GetCorrelativeCode(string tipo)
        {
            int code = 0;
            try
            {
                Correlative correlative = await _context.CORRELATIVE.FirstOrDefaultAsync();
                switch (tipo)
                {
                    case "PLATFORMACCOUNT":
                        correlative.idPlatformAccount++;
                        code = correlative.idPlatformAccount;
                        break;
                    case "USERPLATFORM":
                        correlative.idUserPlatform++;
                        code = correlative.idUserPlatform;
                        break;
                    default:
                        correlative.idUserPlatformAccount++;
                        code = correlative.idUserPlatformAccount;
                        break;
                }
                await UpdateCorrelativeCode(correlative);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            return code;
        }
        public async Task UpdateCorrelativeCode(Correlative correlative)
        {
            await Save(2, correlative);
        }
        public string GetBillingDateofUser()
        {
            DateTime fechaHoy = DateTime.Now;
            bool QuincenaMes = fechaHoy.Day <= 15 ? true : false;
            DateTime oPrimerDiaDelMes = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);

            if (fechaHoy.Month < 12)
            {
                if (QuincenaMes)
                {
                    return GetDate(new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1));
                }
                else
                {
                    return GetDate(oPrimerDiaDelMes.AddMonths(2).AddDays(-1));
                }
            }
            else
            {
                if (QuincenaMes)
                {
                    return GetDate(new DateTime(fechaHoy.Year, fechaHoy.Month, 15).AddMonths(1));
                }
                else
                {
                    return GetDate(oPrimerDiaDelMes.AddMonths(2).AddDays(-1));
                }
            }
        }
        public string SetCorrectFormatDate(string fecha)
        {
            string correctDate = string.Empty;
            try
            {
                string[] diaMesAño = fecha.Split("/");

                for (int i = 0; i < diaMesAño.Length - 1; i++)
                {
                    if (diaMesAño[i].Length == 1) diaMesAño[i] = "0" + diaMesAño[i];
                    correctDate += diaMesAño[i] + "/";
                }
                correctDate += diaMesAño[^1];
            }
            catch (Exception)
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZone"]);
                DateTime cstTime = TimeZoneInfo.ConvertTime(DateTime.Now, cstZone);
                correctDate = cstTime.ToShortDateString();
            }
            return correctDate;
        }
        public int GetUserPayAmount(List<UserPlatform> UserPlatforms)
        {
            double acumulado = 0;
            Platform Platform;
            for (int i = 0; i < UserPlatforms.Count; i++)
            {
                //Platform = _Repository_P.GetPlatformbyId(UserPlatforms[i].idPlatform, false);
                Platform = _context.PLATFORM.Where(p => p.idPlatform == UserPlatforms[i].idPlatform).Select(p => p).FirstOrDefault();
                if (UserPlatforms[i].quantity == 1) acumulado += Platform.highPrice;
                else
                {
                    acumulado += Platform.highPrice * (UserPlatforms[i].quantity - 1);
                    acumulado += Platform.lowPrice;
                }
            }
            if (UserPlatforms.Count > 1 && UserPlatforms.Count < 4 && UserPlatforms.Sum(upa => upa.quantity) == UserPlatforms.Count) acumulado *= 0.93;
            return (int)Math.Round(acumulado);
        }
        #endregion
    }
}