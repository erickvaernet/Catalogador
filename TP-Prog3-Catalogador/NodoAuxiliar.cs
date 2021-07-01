using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    class NodoAuxiliar
    {
        public String Categoria { get; set; }
        public String Lugar { get; set; }

        public DirectoryInfo Directorio { get; set; }

        public NodoAuxiliar(DirectoryInfo dir, String categoria, String Lugar) 
        {
            this.Directorio = dir;
            this.Categoria = categoria;
            this.Lugar = Lugar;
        }

        public NodoAuxiliar() { }
    }
}
