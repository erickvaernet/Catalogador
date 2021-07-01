using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP_Prog3_Catalogador
{
    public partial class FormAgregarCarpetaEnCategoria : Form
    {
        private DirectoryInfo directorio;
        private List<String> lugares;
        private NodoAdaptador nodoAdaptador; //TODO: cambiar  Nodo simplemente, es lo que se usa
        private Nodo nodoSimpleSeleccionado;
        

        public FormAgregarCarpetaEnCategoria(NodoAdaptador nodoAdapter,Nodo nodoSeleccionado)
        {
            InitializeComponent();
            if (nodoAdapter == null)
            {
                MessageBox.Show("Error al cargar el NodoAdaptador");
            }
            this.nodoAdaptador = nodoAdapter;
            label5.Text = nodoSeleccionado.Nombre;
            this.nodoSimpleSeleccionado = nodoSeleccionado;

        }

        /*
        private void CargarCategorias(Nodo nodoPrincipal)            
        {
            foreach (Nodo nodo in nodoPrincipal.NodosHijos)
            {
                if (nodo.NodosHijos.Count > 0)
                {
                    CargarCategorias(nodo);
                }
                comboBox1.Items.Add(nodo.Nombre);
            }
        }*/

        public DirectoryInfo Directorio { get => directorio; set => directorio = value; }
        public List<string> Lugares { get => lugares; set => lugares = value; }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //((Form1)this.Owner).NodoAuxiliar = new NodoAuxiliar(this.Directorio, comboBox1.Text, comboBox2.Text);

            //Agregarmos la Carpeta al nodo correspondiente segun categoria
            /*
            Nodo nodoauxiliar = BuscarNodoCategoria(comboBox1.SelectedItem.ToString(), nodoAdaptador.NodoRaiz);
            if (nodoauxiliar == null) MessageBox.Show("Error al adjuntar carpeta a la categoria");
            else 
            {
                if (Directorio.FullName == null) MessageBox.Show("Error al instanciar el directorio");
                else nodoauxiliar.CarpetasComentadas.Add(new CarpetaComentada(directorio,textBox1.Text));
            }*/


            if (Directorio.FullName == null) MessageBox.Show("Error al instanciar el directorio");
            else nodoSimpleSeleccionado.CarpetasComentadas.Add(new CarpetaComentada(directorio, textBox1.Text));
            this.Dispose();

            
        }

        /*
        private Nodo BuscarNodoCategoria(String nombreCategoria,Nodo nodoPrincipal)
        {
            foreach (Nodo nodo in nodoPrincipal.NodosHijos)
            {
                if (nodo.Nombre == nombreCategoria) return nodo;
                if (nodo.NodosHijos.Count > 0)
                {
                    CargarCategorias(nodo);
                }
                if (nodo.Nombre == nombreCategoria) return nodo;       
            }
            return null;
        }*/
    }
}
