using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    class CarpetaComentada
    {
        public DirectoryInfo Directorio { get; set; }
        public String Comentario { get;set }

        public CarpetaComentada() { }
        public CarpetaComentada(DirectoryInfo dir) 
        {
            Directorio = dir;
        }
        public CarpetaComentada(DirectoryInfo dir,String comentario)
        {
            Directorio = dir;
            Comentario = comentario;
        }
    }
}
