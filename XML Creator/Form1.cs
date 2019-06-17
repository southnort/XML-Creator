using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;


namespace XML_Creator
{
    public partial class Form1 : Form
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\";
        string toEIS_name = "Исходящее в ЕИС.xml";
        string fromEIS_name = "Ответ из ЕИС.xml";
        string GUID_name = "GUID.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateFile(string fileName, string text)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Вы забыли скопировать содержимое файла");
                return;
            }

            string lPath = path + fileName;
            if (File.Exists(lPath))
            {
                MessageBox.Show("Файл \"" + fileName + "\" уже существует. Его нужно удалить или переместить с рабочего стола");
            }

            else
            {
                string[] str = text.Split('\n');

                File.WriteAllLines(lPath, str);

                richTextBox1.Clear();
            }
        }

        private void CreateZipArchive()
        {
            string zipFileName = path + textBox1.Text + "_" + textBox2.Text +
                (textBox3.Text == string.Empty ? "" : ("_" + textBox3.Text)) + ".zip";

            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("cp866")))
            {
                zip.AddFile(path + toEIS_name, "");
                zip.AddFile(path + fromEIS_name, "");
                zip.AddFile(path + GUID_name, "");

                zip.Save(zipFileName);
                MessageBox.Show("Архив создан: \n" + zipFileName);
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            CreateFile(toEIS_name,richTextBox1.Text);
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            CreateFile(fromEIS_name, richTextBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateFile(GUID_name, richTextBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("Нужно заполнить ИНН заказчика и № обращения");
                return;
            }

            if (!File.Exists(path + fromEIS_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + fromEIS_name);
                return;
            }

            if (!File.Exists(path + toEIS_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + toEIS_name);
                return;
            }

            if (!File.Exists(path + GUID_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + GUID_name);
                return;
            }

            CreateZipArchive();
        }

        

       

       
    }
}
