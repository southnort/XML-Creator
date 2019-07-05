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

        private void CreateFile(string filePath, string text)
        {
            string[] str = text.Split('\n');
            File.WriteAllLines(filePath, str);

            richTextBox1.Clear();

        }

        private void TryCreateZipArchive()
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("Нужно заполнить ИНН заказчика и № обращения", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(path + fromEIS_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + fromEIS_name, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(path + toEIS_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + toEIS_name, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(path + GUID_name))
            {
                MessageBox.Show("Не найден файл на рабочем столе " + GUID_name, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string zipFileName = path + CreateFileName(textBox1.Text, textBox2.Text, textBox3.Text);
            CreateZipArchive(zipFileName);
        }

        private string CreateFileName(string inn, string number, string addinional)
        {
            string result =

            inn + "_" + number + "_" +
            (addinional == string.Empty ? "" : ("_" + addinional)) + ".zip";

            return result
                .Replace("\\", "-")
                .Replace("/", "-")
                .Replace(":", "-")
                .Replace("*", "-")
                .Replace("?", "-")
                .Replace("\"", "-")
                .Replace("<", "-")
                .Replace(">", "-")
                .Replace("|", "-");
        }

        private void CreateZipArchive(string zipFileName)
        {
            if (File.Exists(zipFileName))
                CreateZipArchive(zipFileName + "(1)");

            else
            {
                using (ZipFile zip = new ZipFile(Encoding.GetEncoding("cp866")))
                {
                    zip.AddFile(path + toEIS_name, "");
                    zip.AddFile(path + fromEIS_name, "");
                    zip.AddFile(path + GUID_name, "");

                    zip.Save(zipFileName);
                    richTextBox1.Clear();

                    File.Delete(path + toEIS_name);
                    File.Delete(path + fromEIS_name);
                    File.Delete(path + GUID_name);

                    MessageBox.Show("Архив создан: \n" + zipFileName, "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                if (textBox3.Text == string.Empty)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                }                
            }
        }

        private void HandleText(string text)
        {
            string toEISPath = path + toEIS_name;
            string fromEISPath = path + fromEIS_name;
            string GUIDPath = path + GUID_name;

            if (!File.Exists(toEISPath))
                CreateFile(toEISPath, text);
            else if (!File.Exists(fromEISPath))
                CreateFile(fromEISPath, text);
            else if (!File.Exists(GUIDPath))
            {
                CreateFile(GUIDPath, text);
                TryCreateZipArchive();
            }
            else
                TryCreateZipArchive();

        }





        private void addText_Click(object sender, EventArgs e)
        {
            try
            {
                string text = richTextBox1.Text == string.Empty ?
                Clipboard.GetText() :
                richTextBox1.Text;

                //richTextBox1.Text = Clipboard.GetText();

                if (richTextBox1.Text == string.Empty)
                    richTextBox1.Text = text;


                HandleText(text);

            }

            catch (Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            TryCreateZipArchive();
        }
    }
}
