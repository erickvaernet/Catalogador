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
    public partial class Form1 : Form    {

        private XmlReader xmlDocumentReader;
        private XmlController xmlController = new XmlController();

        public Form1()
        {
            InitializeComponent();
            treeView1.Nodes.Add("Categorias");
            treeView2.Nodes.Add("Lugares");

        }  

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            saveFileDialog1.InitialDirectory= Directory.GetCurrentDirectory();
            DialogResult resultado = saveFileDialog1.ShowDialog();

            if (resultado==DialogResult.OK &&!String.IsNullOrWhiteSpace(saveFileDialog1.FileName)) 
            {
                String rutaNuevoDocXml = xmlController.crearXml(saveFileDialog1.FileName);
                xmlDocumentReader= XmlReader.Create(rutaNuevoDocXml);
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            DialogResult resultado= openFileDialog1.ShowDialog();

            if (resultado== DialogResult.OK && !String.IsNullOrWhiteSpace(saveFileDialog1.FileName))
            {
                xmlDocumentReader = XmlReader.Create(openFileDialog1.FileName);
            }
        }

        
    }
}
