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
using System.Xml.Serialization;

namespace Kursach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadItems();
        }
        List<Hardware> items = new List<Hardware>();
        List<Hardware> view = new List<Hardware>();
        private void loadItems()
        {
            FileStream db = new FileStream("db.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xser = new XmlSerializer(typeof(List<Hardware>));
            items = (List<Hardware>)xser.Deserialize(db);
            db.Close();
        }
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Size.Width - 16;
            dataGridView1.Height = this.Size.Height - 296;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            view.AddRange(items);
            foreach (Hardware item in items)
            {
                dataGridView1.Rows.Add(item.Section, item.Type, item.Firm, item.Model, item.Price, item.Comment);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToString("hh:mm:ss");
            toolStripStatusLabel5.Text = (dataGridView1.CurrentRow.Index + 1).ToString();
            toolStripStatusLabel7.Text = dataGridView1.Rows.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows[1].Selected = true; 
            dataGridView1.CurrentCell = dataGridView1[0, 1];
        }
    }
}
