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

        public FormAgregarCarpetaEnCategoria(List<String> categorias)
        {
            InitializeComponent();
            if (categorias == null)
            {
                MessageBox.Show("Error");
            }
            
            foreach (String categoria in categorias)
            {
                comboBox1.Items.Add(categoria);
            }
            /*
            foreach (String lugar in lugares)
            {
                comboBox2.Items.Add(lugar);
            }
            //label2.Text=((Form1)this.Owner).*/
        }

        public DirectoryInfo Directorio { get => directorio; set => directorio = value; }
        public List<string> Lugares { get => lugares; set => lugares = value; }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Form1)this.Owner).NodoAuxiliar = new NodoAuxiliar(this.Directorio, comboBox1.Text, comboBox2.Text);

        }
    }
}
