﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    class Nodo
    {
        public String Nombre { get; set; }
        public String Carpeta { get; set; }
        public String Comentario { get; set; }
        public List<Nodo> NodosHijos { get; set; }


        public void AddNodoHijo(Nodo nodo)
        {
            if (NodosHijos != null) this.NodosHijos.Add(nodo);
            else
            {
                this.NodosHijos = new();
                this.NodosHijos.Add(nodo);
            }
        }
        
    }
}