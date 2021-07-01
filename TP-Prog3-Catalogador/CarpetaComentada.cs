using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    public class CarpetaComentada
    {
        public DirectoryInfo Directorio { get; set; }
        public String Comentario { get; set; }
        public int numeroDeDirectoriosHijos { get; set; }
        public int numeroDeArchivosHijos { get; set; }
        public long Tamaño { get; set; }

        public CarpetaComentada() { }
        public CarpetaComentada(DirectoryInfo dir) 
        {
            Directorio = dir;
            this.numeroDeArchivosHijos= dir.GetFiles().Length;
            this.numeroDeDirectoriosHijos = dir.GetDirectories().Length;
            Tamaño = 0;
        }
        public CarpetaComentada(DirectoryInfo dir,String comentario)
        {
            Directorio = dir;
            Comentario = comentario;
        }

        
        private void ObtenerTamaño(DirectoryInfo dir2) 
        {
            if (dir2.GetDirectories().Length > 0) 
            {
                foreach (DirectoryInfo dirAux in dir2.GetDirectories()) 
                {                   
                    ObtenerTamaño(dirAux) ;
                }
            }
            foreach (FileInfo archivo in dir2.GetFiles()) {
                this.Tamaño += archivo.Length;
            }
            
        }
    }
}
