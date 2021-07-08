using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ElementoContenido contenidoCarpetaSeleccionada;

        public Form1()
        {
            InitializeComponent();
            Nodo nodoRaiz1 = new Nodo();

            nodoAdapter.AgregarNodoRaiz("Categorias", treeView1, nodoRaiz1);

            treeView2.Nodes.Add("Lugares");
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

            Save();
            
        }

        private void Save() {

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
                //TODO: Agregar al constructor el nombbre de directorio
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
            //Todo:Solucionar cuando se selecciona entre resultados buscados y borrar el try 
            try
            {
                treeView3.Nodes.Clear();

                //si la grilla tiene al menos una fila llena, se obtienen los archivos y carpetas de la carpeta principal
                if (dataGridView2.Rows.Count > 0)
                {
                    //Creamos un Elemento contenido que tendra todo el contenido de la Carpeta Comentada
                    ElementoContenido elem = new ElementoContenido();

                    //Rastreamos la CarpetaComentada seleccionada en la grilla;
                    elem.Contenido = MapearCarpetaEnGrillaSeleccionada().Contenido;
                    elem.FullPath = @dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    elem.EsDirectorio = true;

                    this.contenidoCarpetaSeleccionada = elem;

                    LoadFolder2(treeView3.Nodes, elem);
                    treeView3.Nodes[0].Expand();
                }
            }
            catch (Exception)
            {               
            }
               
        }

        //Rastrea la carpetaComentada Que se encuentra seleccionada en el GridView Principal
        private CarpetaComentada MapearCarpetaEnGrillaSeleccionada() 
        {            
            foreach (CarpetaComentada carpeta in nodoAdapter.MapearNodoSeleccionado().CarpetasComentadas) 
            {
                if (carpeta.Directorio == @dataGridView2.CurrentRow.Cells[0].Value.ToString()) return carpeta;
                if (carpeta.Directorio == dataGridView2.CurrentRow.Cells[0].Value.ToString()) return carpeta;
            }
            return nodoAdapter.MapearNodoSeleccionado().CarpetasComentadas[0];
        }
        private void LoadFolder2(TreeNodeCollection nodes, ElementoContenido elem)
        {
            //posicion del ultimo elem del nombre full (Pasar a ElementoContenido)
            int index = elem.FullPath.Split("\\").Length - 1;
            var newNode = nodes.Add(elem.FullPath.Split("\\")[index]);
            foreach (var child in elem.Contenido)
            {
                if(child.EsDirectorio) LoadFolder2(newNode.Nodes, child);
                else newNode.Nodes.Add(child.FullPath.Split("\\")[index+1]);
            }
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

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            //limpiamos la grilla
            dataGridView2.Rows.Clear();

            //Creamos una lista vacia y la pasamos a buscarCarpetaComentada,
            //la cual llenara la lista con los valores que coinsidan con la busqueda
            List<CarpetaComentada> carpetasObtenidas= new();
            BuscarCarpetaComentada(nodoAdapter.NodoRaiz,carpetasObtenidas);

            //Si la lista esta vacía no hubo resultados, caso contrario se pintan los resultados en la grilla
            if (carpetasObtenidas.Count==0) return;
            else 
            {
                foreach (CarpetaComentada carpeta in carpetasObtenidas)
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

            textBoxBusqueda.Text = "";

        }
       

        //Método auxiliar para llenar una lista con CarpetasComentadas coinsidentes con la búsqueda
        //TODO: Hayq ue hacerlo más genético y pasarle el textbox en lugar de usarlo directamente dentro
        private void BuscarCarpetaComentada(Nodo nodoPadre, List<CarpetaComentada> carpetasResultantes) 
        {                       
            foreach (Nodo nodo in nodoPadre.NodosHijos)
            {
                if (nodo.NodosHijos.Count > 0) BuscarCarpetaComentada(nodo,carpetasResultantes);
                else
                {
                    foreach (CarpetaComentada carpeta in nodo.CarpetasComentadas) 
                    {
                        if (carpeta.Comentario.ToLower().Contains(textBoxBusqueda.Text.ToLower()))
                            carpetasResultantes.Add(carpeta);
                    }
                }
            }

        }


        private void treeView3_DoubleClick(object sender, EventArgs e)
        {
            ElementoContenido elem= RastrearElementoSeleccionadoTVSecundario();      
            try
            {
                Process proceso = new Process();
                proceso.StartInfo.FileName= elem.FullPath;
                proceso.StartInfo.UseShellExecute = true;
                proceso.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("El archivo o carpeta: "+elem.FullPath+"\n No se encuentra disponible. Si el contenido pertenece a un disco externo pruebe conectandolo");
            }
        }

        private ElementoContenido RastrearElementoSeleccionadoTVSecundario()
        {
            List<int> lvls = new();

            TreeNode treeNodeSeleccionado = treeView3.SelectedNode;
            while (treeNodeSeleccionado.Parent != null)
            {
                lvls.Insert(0, treeNodeSeleccionado.Index);
                treeNodeSeleccionado = treeNodeSeleccionado.Parent;
            }

            ElementoContenido elemetoContenidoAux = this.contenidoCarpetaSeleccionada;

            foreach (int lvl in lvls)
            {
                elemetoContenidoAux = elemetoContenidoAux.Contenido[lvl];
            }

            return elemetoContenidoAux;

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Todo:Testear
            nodoAdapter = new NodoAdaptador();

            carpetasSeleccionadas = new List<FileInfo>();
            Nodo nodoRaiz1 = new Nodo();

            treeView3.Nodes.Clear();
            dataGridView2.Rows.Clear();
            treeView1.Nodes.Clear();

            nodoAdapter.AgregarNodoRaiz("Categorias", treeView1, nodoRaiz1);

            treeView2.Nodes.Add("Lugares");

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
            this.Dispose();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
