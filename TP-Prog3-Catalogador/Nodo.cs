using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    class Nodo
    {
        public String Nombre { get; set; }
        public List<DirectoryInfo> Carpetas { get; set; }
        public String Comentario { get; set; }
        public List<Nodo> NodosHijos { get; set; }

        public Nodo(String nombre) 
        {
            this.NodosHijos = new List<Nodo>(); 
            this.Nombre = nombre;
            this.Carpetas = new List<DirectoryInfo>();
        }

        public Nodo() 
        { 
            this.NodosHijos = new List<Nodo>();
            this.Carpetas = new List<DirectoryInfo>();
        }
       

    }

}


/*
       public void AddNodoHijo(Nodo nodo)
       {
           if (NodosHijos != null) this.NodosHijos.Add(nodo);
           else
           {
               this.NodosHijos = new();
               this.NodosHijos.Add(nodo);
           }
       }
*/
