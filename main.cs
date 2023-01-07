using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace Менеджер_паролей
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        }

        private void Connection(string src, ref XmlDocument doc) => doc.Load(src);
        

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Size = this.Size - new Size(27, 47);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            notifyIcon1.Icon = new Icon("castle.ico");

            XmlDocument src = new XmlDocument();
            Connection("data.xml", ref src);

            Color[] colors = new Color[] { Color.Lavender, Color.White };

            if (src.DocumentElement.ChildNodes.Count >= 1)
            {
                label1.Text = "";
                int index = 0;

                foreach (XmlNode item in src.DocumentElement.ChildNodes)
                {
                    Label lbl = new Label();

                    lbl.Name = "label" + index;
                    lbl.Text = item.Name;
                    lbl.BackColor = index % 2 == 0 ? colors[0] : colors[1];
                    lbl.Width = this.Width - 27;
                    lbl.Height = 20;
                    lbl.Click += label_Click;
                    lbl.Location = new Point(0, 20*index);

                    tabControl1.TabPages[0].Controls.Add(lbl);

                    contextMenuStrip1.Items.Add(lbl.Text, null, label_Click);

                    index++;
                }
            }

            else
            {
                label1.Text = "Пусто...\nПерейдите в окно \"Добавить\", чтобы добавить новый ресурс";
            }

            contextMenuStrip1.Items.Add("Закрыть", null, close);
            contextMenuStrip1.Items[contextMenuStrip1.Items.Count - 1].BackColor = Color.Gray;
        }
             
        private void label_Click(object sender, EventArgs e)
        {
            XmlDocument src = new XmlDocument();
            Connection("data.xml", ref src);

            try
            {
                Clipboard.SetText(src.GetElementsByTagName(((Label)sender).Text)[0].InnerText);
            }
            catch
            {
                Clipboard.SetText(src.GetElementsByTagName(((ToolStripMenuItem)sender).Text)[0].InnerText);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlDocument src = new XmlDocument();
            Connection("data.xml", ref src);

            foreach (XmlNode item in src.DocumentElement.ChildNodes)
            {
                if (item.Name == textBox1.Text)
                {
                    MessageBox.Show("Ошибка! Ресурс с таким названием уже существует", "Ошибка");
                    return;
                }
            }

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Ошибка! Заполните все поля", "Ошибка");
            }
            else
            {
                XmlElement cur = src.CreateElement(textBox1.Text);
                cur.InnerText = textBox2.Text;

                src.DocumentElement.AppendChild(cur);

                src.Save("data.xml");

                Application.Restart();
            }
        }

        private void close(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
