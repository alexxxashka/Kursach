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
        BindingSource Source = new BindingSource();        

        private void loadItems()
        {
            FileStream db = new FileStream("db.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xser = new XmlSerializer(typeof(List<Hardware>));
            items = (List<Hardware>)xser.Deserialize(db);
            db.Close();

            view.AddRange(items);
        }

        private void saveItems()
        {
            FileStream db = new FileStream("db.xml", FileMode.Create, FileAccess.Write);
            XmlSerializer xser = new XmlSerializer(typeof(List<Hardware>));
            xser.Serialize(db, items);
            db.Close();
        }
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Size.Width - 16;
            dataGridView1.Height = this.Size.Height - 296;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Source.DataSource = view;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = Source;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToString("hh:mm:ss");
            toolStripStatusLabel5.Text = (dataGridView1.CurrentRow.Index + 1).ToString();
            toolStripStatusLabel7.Text = dataGridView1.Rows.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //items.RemoveAt(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            //dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);            
            view.Add(new Hardware(100));
            Source.ResetBindings(false);
          //  dataGridView1.DataSource = null;
          //  dataGridView1.DataSource = Source;
            label1.Text = view.Count.ToString();
            int index = 0;
            foreach (Hardware item in items)
            {
                item.id = index++;
            }
            saveItems();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
