using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace TP_Prog3_Catalogador
{
    public partial class Form1 : Form
    {

        private NodoAdaptador nodoAdapter = new NodoAdaptador();
        private List<FileInfo> carpetasSeleccionadas = new List<FileInfo>();

        public Form1()
        {
            InitializeComponent();
            Nodo nodoRaiz1 = new Nodo();

            nodoAdapter.AgregarNodoRaiz("Categorias", treeView1, nodoRaiz1);

            treeView2.Nodes.Add("Lugares");
            label1.Text = treeView1.Nodes[0].Text;
        }

        //Agregar categoria
        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String nombre = Interaction.InputBox("Indique el nombre de la nueva Categoria", "NuevaCategoria");
            if (treeView1.SelectedNode != null && !String.IsNullOrWhiteSpace(nombre))
            {

                nodoAdapter.AgregarNodoHijo(nombre);
            }

        }

        //Quitar categoria
        private void quitarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Text == "Categorias") return;
            nodoAdapter.QuitarNodoSeleccionado();
            nodoAdapter.CargarRaizATreeView();
        }


        /*
        //devuelve lista con nombres de nodos de Categorias sin nodos hijos
        public List<String> getCategorias() {

            recursivoNodo(treeView1.Nodes[0].Nodes);
            return catDisponibles;

        }

        List<String> catDisponibles= new();
        private void recursivoNodo(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                
                if (node.Nodes.Count > 0)
                {
                    recursivoNodo(node.Nodes);
                }
                else 
                {
                    //Si no tiene hijos podemos escribir carpetas
                    catDisponibles.Add(node.Text);
                }
            }
        }*/

        //Guardar archivo .json
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String jsonString;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK && !String.IsNullOrWhiteSpace(saveFileDialog1.FileName))
            {
                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.Preserve };

                //Escribir nodo en json:
                jsonString = JsonSerializer.Serialize<Nodo>(nodoAdapter.NodoRaiz);
                File.WriteAllText(saveFileDialog1.FileName, jsonString);
            }
        }

        //Abrir archivo.json
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            /*
             * Creamos una ventana de OpenFileDialog que seleccione el archivo a abrir, si apeta en OK y se tiene
             * seleccionado una rchivo, entonces se procede a leer el archivo .json y parsearlo a un objeto Nodo
             * el cual se usara como nueva Raiz del nodo principal del programa (nodoAdapter) 
             */
            if (openFileDialog1.ShowDialog() == DialogResult.OK && !String.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                //Leemos el archivo y asignamos el nuevo nodo raiz, nodoadapter:
                nodoAdapter.ControlRaizCreada = false;

                String jsonString;
                jsonString = File.ReadAllText(openFileDialog1.FileName);
                Nodo nodoAuxiliar = JsonSerializer.Deserialize<Nodo>(jsonString);
                nodoAdapter.NodoRaiz = nodoAuxiliar;

                nodoAdapter.ControlRaizCreada = true;
                //--------------------------------------------------------------

                //Limpiamos todo
                treeView3.Nodes.Clear();
                dataGridView2.Rows.Clear();

                //Cargamos el nodo al treeView 
                nodoAdapter.CargarRaizATreeView();

            }

        }

        //Agregar carpeta a Una categoria
        private void agregarCarpetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK &&
                !String.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                FormAgregarCarpetaEnCategoria formAgregarCarpetaEnCategoria = new FormAgregarCarpetaEnCategoria(nodoAdapter, nodoAdapter.MapearNodoSeleccionado());

                formAgregarCarpetaEnCategoria.Directorio = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                //formAgregarCarpetaEnCategoria.Lugares =

                formAgregarCarpetaEnCategoria.ShowDialog();
            }
            ActualizarGrillaPrincipal();
            ActualizarTreeViewSecundario();
        }

        //eliminar
        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Trae Contenido de la carpeta seleccionada en treeview secundario 
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActualizarTreeViewSecundario();
        }

        //Métodos auxiliares para actualizar el treeview secundario
        private void ActualizarTreeViewSecundario()
        {
            treeView3.Nodes.Clear();
            if (dataGridView2.Rows[0].IsNewRow) {
                
                return;
            } 

            DirectoryInfo directorioSeleccionado = new DirectoryInfo(@dataGridView2.CurrentRow.Cells[0].Value.ToString());

            LoadFolder(treeView3.Nodes, directorioSeleccionado);
            treeView3.Nodes[0].Expand();
        }

        private void LoadFolder(TreeNodeCollection nodes, DirectoryInfo folder)
        {
            var newNode = nodes.Add(folder.Name);
            foreach (var childFolder in folder.EnumerateDirectories())
            {
                LoadFolder(newNode.Nodes, childFolder);
            }
            foreach (FileInfo file in folder.EnumerateFiles())
            {
                newNode.Nodes.Add(file.Name);
            }
        }

        //eliminar
        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }


        //actualizar grilla principal (dataGridView) y treeviewSecundario
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ActualizarGrillaPrincipal();
            ActualizarTreeViewSecundario();
        }

        public void ActualizarGrillaPrincipal()
        {
            if (treeView1.SelectedNode == null) return;
            //Se limpia la grilla
            dataGridView2.Rows.Clear();

            //al seleccionar nodo trae las carpetas asociadas a ese nodo
            List<CarpetaComentada> carpetasSeleccionadas = nodoAdapter.MapearNodoSeleccionado().CarpetasComentadas;

            if (carpetasSeleccionadas.Count() > 0)
            {
                foreach (CarpetaComentada carpeta in carpetasSeleccionadas)
                {
                    DataGridViewRow fila = new DataGridViewRow();
                    fila.CreateCells(dataGridView2);
                    fila.Cells[0].Value = carpeta.Directorio;
                    fila.Cells[1].Value = carpeta.Comentario;
                    fila.Cells[2].Value = carpeta.numeroDeDirectoriosHijos.ToString();
                    fila.Cells[3].Value = carpeta.numeroDeArchivosHijos.ToString();
                    fila.Cells[4].Value = carpeta.TamañoEnKB;
                    dataGridView2.Rows.Add(fila);
                }

            }
        }
    }
}
