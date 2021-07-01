using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP_Prog3_Catalogador
{
    class NodoAdaptador
    {
        private bool controlRaizCreada = false;
        private TreeView treeView;
        private Nodo nodoRaiz;

        public NodoAdaptador()
        { 
        }

        public void AgregarNodoRaiz(String nombre, TreeView treeView, Nodo nodoRaiz)            
        {
            if (controlRaizCreada == true) return; //no se puede crear mas de una raiz; yo deberia crear un error

            this.treeView = treeView;
            this.nodoRaiz = nodoRaiz;
            treeView.Nodes.Add(nombre);
            nodoRaiz.Nombre = nombre;
            controlRaizCreada = true;
        }

        public void AgregarNodoHijo(String hijo) 
        {
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
            
            nodoAux.NodosHijos.Add(new Nodo(hijo));
            treeView.SelectedNode.Nodes.Add(hijo);

        }

    }
}
