using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    public class ElementoContenido
    {
        public String FullPath { get; set; }
        public String Comentario { get; set; }
        public bool EsArchivo { get; set; }
        public bool EsDirectorio { get; set; }
        public List<ElementoContenido> Contenido { get; set; }

        //Constructores
        public ElementoContenido() 
        {
            this.Contenido = new List<ElementoContenido>();
        }
        public ElementoContenido(String path, bool esDirectorio)
        {
            this.Contenido = new List<ElementoContenido>();
            this.FullPath = path;
            this.EsDirectorio = esDirectorio;
            this.EsArchivo = !esDirectorio;

        }

        //Optimizar llamando padre
        public ElementoContenido(String path, bool esDirectorio, String comentario)
        {
            this.Contenido = new List<ElementoContenido>();
            this.FullPath = path;
            this.Comentario = comentario;
            this.EsDirectorio = esDirectorio;
            this.EsArchivo = !esDirectorio;
        }
    }
}
