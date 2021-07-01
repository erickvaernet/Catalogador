using Microsoft.VisualBasic;
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
using System.Xml;

namespace TP_Prog3_Catalogador
{
    public partial class Form1 : Form {

        private XmlTextWriter xr;
        private FileInfo workingDirectory;
        private NodoAdaptador nodoAdapter = new NodoAdaptador();
        private List<FileInfo> carpetas = new List<FileInfo>();
        private NodoAuxiliar nodoAuxiliar;

        internal NodoAuxiliar NodoAuxiliar { get => nodoAuxiliar; set => nodoAuxiliar = value; }

        public Form1()
        {
            InitializeComponent();
            //if(workingDirectory==null)
            Nodo nodoRaiz1 = new Nodo();

            nodoAdapter.AgregarNodoRaiz("Categorias", treeView1, nodoRaiz1);
            
            treeView2.Nodes.Add("Lugares");
            label1.Text = treeView1.Nodes[0].Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK &&
                !String.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                FormAgregarCarpetaEnCategoria formAgregarCarpetaEnCategoria = new FormAgregarCarpetaEnCategoria(getCategorias());

                formAgregarCarpetaEnCategoria.Directorio = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
               
                //formAgregarCarpetaEnCategoria.Lugares =

                formAgregarCarpetaEnCategoria.ShowDialog();
            }
        }

        
        private void populateTreeview()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open XML Document";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            dlg.FileName = Application.StartupPath + "\\..\\..\\example.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Just a good practice -- change the cursor to a 
                    //wait cursor while the nodes populate
                    this.Cursor = Cursors.WaitCursor;
                    //First, we'll load the Xml document
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(dlg.FileName);
                    //Now, clear out the treeview, 
                    //and add the first (root) node
                    treeView1.Nodes.Clear();
                    treeView1.Nodes.Add(new
                      TreeNode(xDoc.DocumentElement.Name));
                    TreeNode tNode = new TreeNode();
                    tNode = (TreeNode)treeView1.Nodes[0];
                    //We make a call to addTreeNode, 
                    //where we'll add all of our nodes
                    addTreeNode(xDoc.DocumentElement, tNode);
                    //Expand the treeview to show all nodes
                    treeView1.ExpandAll();
                }
                catch (XmlException xExc)
                //Exception is thrown is there is an error in the Xml
                {
                    MessageBox.Show(xExc.Message);
                }
                catch (Exception ex) //General exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Default; //Change the cursor back
                }
            }
        }
        //This function is called recursively until all nodes are loaded
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];
                    treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
            else //No children, so add the outer xml (trimming off whitespace)
                treeNode.Text = xmlNode.OuterXml.Trim();
        }

        public void exportToXml2(TreeView tv, string filename)
        {
            xr = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xr.WriteStartDocument();
            //Write our root node
            xr.WriteStartElement(treeView1.Nodes[0].Text);
            foreach (TreeNode node in tv.Nodes)
            {
                saveNode2(node.Nodes);
            }
            //Close the root node
            xr.WriteEndElement();
            xr.Close();
        }

        private void saveNode2(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    xr.WriteStartElement(node.Text);
                    saveNode2(node.Nodes);
                    xr.WriteEndElement();
                }
                else //No child nodes, so we just write the text
                {
                    /*
                     * lista de carpetas con sus nodos y comentarios asi en el if pregunto si el nodo.Text es igual al nodo de carpeta
                     * escribo con la carpeta y un comentario
                     * if(get )
                     */
                    //xr.WriteStartElement(node.Text);
                    xr.WriteString(node.Text); //Si no tiene hijos podemos escribir carpetas
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            populateTreeview();
        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String nombre = Interaction.InputBox("Indique el nombre de la nueva Categoria", "NuevaCategoria");
            if (treeView1.SelectedNode != null && !String.IsNullOrWhiteSpace(nombre)) {
                treeView1.SelectedNode.Nodes.Add(nombre);
            }

        }

        private void quitarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Text == "Categorias") return;

            treeView1.SelectedNode.Remove();
        }


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
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    recursivoNodo(node.Nodes);
                }
                else //No child nodes, so we just write the text
                {
                    //Si no tiene hijos podemos escribir carpetas
                    catDisponibles.Add(node.Text);
                }
            }
        }
    }
}
