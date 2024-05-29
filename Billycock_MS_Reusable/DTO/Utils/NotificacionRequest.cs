using System;
using System.Collections.Generic;

namespace Billycock_MS_Reusable.DTO.Utils
{
    public class NotificacionRequest
    {
        public String nombreHost { get; set; }
        public String puertoHost { get; set; }
        public String correoEmisor { get; set; }
        public String claveEmisor { get; set; }
        public String asunto { get; set; }
        public String mensaje { get; set; }
        public String para { get; set; }
        public String copia { get; set; }
        public String copiaOculta { get; set; }
        public List<String> listaArchivos { get; set; }

    }
}
