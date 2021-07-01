using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Prog3_Catalogador
{
    class CarpetaInfo
    {
        String nombre;        
        List<DirectoryInfo> directoriosHijos;
        List<FileInfo> archivosHijos;
        
        double size;
        int cantidadArchivosHijos;
        int cantidadDirectoriosHijos;

        public CarpetaInfo() { }

        public CarpetaInfo(DirectoryInfo carpetaPrincipal) 
        {
            this.nombre = carpetaPrincipal.FullName;
        }


    }
}
