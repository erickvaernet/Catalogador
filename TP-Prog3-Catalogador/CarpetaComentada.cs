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

        public List<ElementoContenido> Contenido { get; set; }


    //"Características de la carpeta"
    public int numeroDeDirectoriosHijos { get; set; }
        public int numeroDeArchivosHijos { get; set; }
        private long TamañoEnBytes { get; set; }
        public long TamañoEnKB { get; set; }

        //Constructores
        public CarpetaComentada() 
        {
            this.Contenido = new List<ElementoContenido>();
            
        }
        public CarpetaComentada(DirectoryInfo dir) 
        {
            Directorio = dir.FullName;
            this.numeroDeArchivosHijos= dir.GetFiles().Length;
            this.numeroDeDirectoriosHijos = dir.GetDirectories().Length;
            TamañoEnBytes = 0;
            ObtenerTamaño(dir);
            TamañoEnKB = TamañoEnBytes / 1024;

            this.Contenido = new List<ElementoContenido>();
            CargarContenido();

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

            this.Contenido = new List<ElementoContenido>();
            CargarContenido();
        }

        //obtiene tamaño en bytes de la carpeta (con su contenido)
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

        private void CargarContenido(){
            DirectoryInfo dirActual = new DirectoryInfo(this.Directorio);
            foreach (DirectoryInfo dirHijo in dirActual.EnumerateDirectories()) 
            {
                //Cargar cada elementoContenido con su contnido
                ElementoContenido elementoHijo = new ElementoContenido(dirHijo.FullName,true);
                CargarElementoContenido(elementoHijo);
                //.....

                //Pasamos la direccion de carpeta y como es una carpeta ponemos el segundo paramtroe en true (Si fuera archivo seria false)
                this.Contenido.Add(elementoHijo);
            }

            foreach (FileInfo archivoHijo in dirActual.EnumerateFiles())
            {
                //Pasamos la direccion de carpeta y como NO es una carpeta ponemos el segundo paramtroe en False
                this.Contenido.Add(new ElementoContenido(archivoHijo.FullName, false));
            }
        }

        public void CargarElementoContenido(ElementoContenido elemContenido) 
        {
            DirectoryInfo dirActual = new DirectoryInfo(elemContenido.FullPath);
            foreach (DirectoryInfo dirHijo in dirActual.EnumerateDirectories())
            {
                //Cargar cada elementoContenido con su contenido
                ElementoContenido elemContenidoHijo = new ElementoContenido(dirHijo.FullName,true);
                CargarElementoContenido(elemContenidoHijo);

                elemContenido.Contenido.Add(elemContenidoHijo);
            }

            foreach (FileInfo archivoHijo in dirActual.EnumerateFiles())
            {
                //Pasamos la direccion de carpeta y como es una carpeta ponemos el segundo paramtroe en true (Si fuera archivo seria false)
                elemContenido.Contenido.Add(new ElementoContenido(archivoHijo.FullName,false));
            }
        }
    }
}
