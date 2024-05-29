using Billycock_MS_Reusable.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Models
{
    public class Globals
    {
        public const string OCURRIDO_PROBLEMA_SERVIDOR = "HA OCURRIDO UN PROBLEMA CON EL SERVIDOR";
        public const string MENSAJE_ERROR_EQUIPO_REMOTO = "NO SE PUEDE ESTABLECER CONEXIÓN CON EL SERVICIO REMOTO";
        public const string MENSAJE_ERROR_RESPUESTA_EQUIPO_REMOTO = "MENSAJE DE ERROR EN LA RESPUESTA DEL SERVICIO REMOTO";
        public static string Message;

        public const string tipo = "REUSABLE";
        public const string integration = "Billycock_MS_Reusable";
    }
    public class TransactionalProcess
    {
        public const string ONLINE = "ONLINE";
        public const int CREATION_PROCESS = 1;
        public const int UPDATE_PROCESS = 2;
        public const int ELIMINATION_PROCESS = 3;
        public const int HISTORY = 99;

        public static string CREATION = "CREACION CORRECTA DE @TABLE@";
        public static string NOCREATION = "CREACION INCORRECTA DE @TABLE@";
        public static string UPDATE = "ACTUALIZACION CORRECTA DE @TABLE@";
        public static string NOUPDATE = "ACTUALIZACION INCORRECTA DE @TABLE@";
        public static string ELIMINATION = "ELIMINACION CORRECTA DE @TABLE@";
        public static string NOELIMINATION = "ELIMINACION INCORRECTA DE @TABLE@";
        public static string NOHISTORY = "INSERCION HISTORICA INCORRECTA DE @TABLE@";
        public const string NOCHANGES = "NO SE ENCONTRARON CAMBIOS";
    }
    public class ExceptionMessage
    {
        public static string message = "ERROR EN @PROCESS@ DE @TABLE@";
    }
    public class DuplicateMessage
    {
        public static string message = "YA EXISTE UN REGISTRO CON DATOS IGUALES EN LA TABLA @TABLE@";
    }
    public class Mail
    {
        public const string ASUNTO_ERROR_DATA = "General WS Billycock – ERROR DATA {0}";
        public const string CUERPO_ERROR_DATA = "Se ha presentado un error de dato en WS Billycock. Se adjunto el INPUT y OUTPUT de la solicitud para su información.. [Código de Error]: {0} [Mensaje de Error]: {1}. Archivos: INPUT y OUTPUT del WS ";
    }
}
