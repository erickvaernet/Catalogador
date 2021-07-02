using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP_Prog3_Catalogador
{
    public class NodoAdaptador
    {
        private bool controlRaizCreada = false;
        private TreeView treeView;
        private Nodo nodoRaiz;
        public Nodo NodoRaiz { get => nodoRaiz; set => nodoRaiz = value; }
        public bool ControlRaizCreada { get => controlRaizCreada; set => controlRaizCreada = value; }

        public NodoAdaptador()
        { 
        }

        //Para agregar el Nodo Modelo con el cual nos manejaremos
        public void AgregarNodoRaiz(String nombre, TreeView treeView, Nodo nodoRaiz)            
        {
            if (controlRaizCreada == true) return; //no se puede crear mas de una raiz; yo deberia crear un error

            this.treeView = treeView;
            this.nodoRaiz = nodoRaiz;
            treeView.Nodes.Add(nombre);
            nodoRaiz.Nombre = nombre;
            controlRaizCreada = true;
        }

        //Agrega un Nodo Hijo dentro del treeView segun donde se seleccione;
        public void AgregarNodoHijo(String hijo)
        { 

            //TODO: Reemplazar por mapearNodoSeleccionado, y nodo retornado usarlo para agregar hijo
            List<int> lvls= new();

            TreeNode nodo = treeView.SelectedNode;
            while (nodo.Parent!=null) 
            {
                lvls.Insert(0,nodo.Index);
                nodo = nodo.Parent;
            }

            Nodo nodoAux= nodoRaiz;
            foreach (int lvl in lvls) 
            {
                nodoAux = nodoAux.NodosHijos[lvl];
            }
            //------------------------------------------

            nodoAux.NodosHijos.Add(new Nodo(hijo));
            treeView.SelectedNode.Nodes.Add(hijo);

        }

        //Obtiene el Nodo del modelo correspondiente al nodo del treeview seleccionado
        //(Sirve paara agregar carpeta a categoria, tambien usada en actualizar grillaPrincipal y para quitar nodo seleccionado)
        public Nodo MapearNodoSeleccionado() 
        {
            List<int> lvls = new();

            TreeNode treeNodeSeleccionado = treeView.SelectedNode;
            while (treeNodeSeleccionado.Parent != null)
            {
                lvls.Insert(0, treeNodeSeleccionado.Index);
                treeNodeSeleccionado = treeNodeSeleccionado.Parent;
            }

            Nodo nodoAux = nodoRaiz;
            foreach (int lvl in lvls)
            {
                nodoAux = nodoAux.NodosHijos[lvl];
            }

            return nodoAux;

        }

        //Carga la vista del treeview segun el Nodo Modelo;
        public void CargarRaizATreeView() 
        {
            try
            {
                //Limpiamos el TreeView
                treeView.Nodes.Clear();

                //Agregarmos una raiz al treeview
                treeView.Nodes.Add(nodoRaiz.Nombre);

                //Usamos el método auxiliar AgregarTreeNodes para ir agregando recursivamente los nodos
                AgregarTreeNodes(treeView.Nodes[0], nodoRaiz);

                //Expandimos el treeview para que se vean todos los nodos
                treeView.ExpandAll();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error al cargar el archivo JSON:\n"+ ex.Message);                
            }
            
        }

        //Método Auxiliar que permite agregar los treeNodes recursivamente a partir de un objeto "Nodo"
        private void AgregarTreeNodes(TreeNode treeNode, Nodo nodo) 
        {
            TreeNode treeNodeAux;
            Nodo nodoAux;
            List<Nodo> listaNodosHijos = new List<Nodo>();

            if (nodo.NodosHijos.Count > 0) //Si nodo actual tiene hijos
            {
                listaNodosHijos = nodo.NodosHijos;
                for (int i = 0; i <= listaNodosHijos.Count - 1; i++)
                {
                    nodoAux = nodo.NodosHijos[i];
                    treeNode.Nodes.Add(new TreeNode(nodoAux.Nombre));
                    treeNodeAux = treeNode.Nodes[i];
                    AgregarTreeNodes(treeNodeAux, nodoAux);
                }
            }
            else treeNode.Text = nodo.Nombre;            
        }

        //Método para quitár nodo seleccionado tanto en el treeview como en el modelo
        public void QuitarNodoSeleccionado() 
        {
            Nodo nodoAeliminar = MapearNodoSeleccionado();

            Nodo nodoPadre = ObtenerPadreSeleccionado(nodoAeliminar);

            foreach (Nodo nodoaux in nodoPadre.NodosHijos)
            {
                if (nodoaux.Nombre == nodoAeliminar.Nombre) 
                {
                    nodoPadre.NodosHijos.Remove(nodoaux);
                    break;
                }
                  
            }            

        }

        //Método Aux para quitar nodo Seleccionado
        private Nodo ObtenerPadreSeleccionado(Nodo nodo)
        {
            List<int> lvls = new();

            TreeNode treeNodeSeleccionado = treeView.SelectedNode;
            while (treeNodeSeleccionado.Parent != null)
            {
                lvls.Insert(0, treeNodeSeleccionado.Index);
                treeNodeSeleccionado = treeNodeSeleccionado.Parent;
            }

            int contador = 0;
            Nodo nodoAux = nodoRaiz;
            foreach (int lvl in lvls)
            {
                contador++;
                if(contador==(lvls.Count()-1)) nodoAux = nodoAux.NodosHijos[lvl];
            }
            return nodoAux;

        }

        /*
        if (nodo.NodosHijos.Count() == 0)
        { 
            treeNode.Nodes.Add(new TreeNode(nodo.Nombre));
        }
        else
        {
            CopiarNodos(treeNode.Nodes);
        }*/

    }
}
