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
        public String Directorio { get; set; } 
        public String Comentario { get; set; }
        
        public int numeroDeDirectoriosHijos { get; set; }
        public int numeroDeArchivosHijos { get; set; }
        private long TamañoEnBytes { get; set; }
        public long TamañoEnKB { get; set; }

        public CarpetaComentada() { }
        public CarpetaComentada(DirectoryInfo dir) 
        {
            Directorio = dir.FullName;
            this.numeroDeArchivosHijos= dir.GetFiles().Length;
            this.numeroDeDirectoriosHijos = dir.GetDirectories().Length;
            TamañoEnBytes = 0;
            ObtenerTamaño(dir);
            TamañoEnKB = TamañoEnBytes / 1024;

        }
        public CarpetaComentada(DirectoryInfo dir,String comentario)
        {
            Directorio = dir.FullName;
            Comentario = comentario;
            this.numeroDeArchivosHijos = dir.GetFiles().Length;
            this.numeroDeDirectoriosHijos = dir.GetDirectories().Length;
            TamañoEnBytes = 0;
            ObtenerTamaño(dir);

            TamañoEnKB = TamañoEnBytes / 1000;
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
                this.TamañoEnBytes += archivo.Length;
            }
            
        }
    }
}
