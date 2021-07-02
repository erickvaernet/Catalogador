using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    public class Nodo
    {
        public String Nombre { get; set; } //categoria

       
        public List<CarpetaComentada> CarpetasComentadas { get; set; }
        public String Comentario { get; set; }
        public List<Nodo> NodosHijos { get; set; }

        public Nodo(String nombre) 
        {
            this.NodosHijos = new List<Nodo>(); 
            this.Nombre = nombre;
            this.CarpetasComentadas = new List<CarpetaComentada>();
        }

        public Nodo() 
        { 
            this.NodosHijos = new List<Nodo>();
            this.CarpetasComentadas = new List<CarpetaComentada>();
        }
       

    }

}


