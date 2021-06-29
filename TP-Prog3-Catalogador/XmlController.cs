using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TP_Prog3_Catalogador
{
    class XmlController
    {
        public String crearXml(String nombreArchivoXml) {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("Catalogador");
            doc.AppendChild(rootNode);

            //Categorias (nodo lv-2)
            XmlNode categoriesNode = doc.CreateElement("Categorias");
            XmlAttribute cantidad = doc.CreateAttribute("Cantidad");
            cantidad.Value = "0";
            categoriesNode.Attributes.Append(cantidad);
            rootNode.AppendChild(categoriesNode);

            //Lugares (nodo lv-1)
            XmlNode placesNode = doc.CreateElement("Lugares");
            XmlAttribute cantidad2 = doc.CreateAttribute("Cantidad");
            cantidad2.Value = "0";
            placesNode.Attributes.Append(cantidad2);
            rootNode.AppendChild(placesNode);
            
            doc.Save(nombreArchivoXml);
            return nombreArchivoXml;
        }

        public void cargarXml( XmlReader xmlReader, TreeView treeView) 
        {
            

        }



    }
}
