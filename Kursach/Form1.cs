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
        }
        bool isSaved = true;
        List<Hardware> items = new List<Hardware>();
        List<Hardware> view = new List<Hardware>();
        BindingSource Source = new BindingSource();
        List<string> Sections = new List<string>();
        List<string> Types = new List<string>();
        List<string> Firms = new List<string>();
        bool isInc = false;

        private void loadItems() //load items from file
        {
            FileStream db = new FileStream("db.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xser = new XmlSerializer(typeof(List<Hardware>));
            items = (List<Hardware>)xser.Deserialize(db);
            db.Close();
        }
        private void fieldsUpdate()
        {
            Sections.Clear();
            foreach (Hardware item in items)
            {
                if (!listBox1.Items.Contains(item.Section))
                {
                    listBox1.Items.Add(item.Section);
                }
                if (!Sections.Contains(item.Section))
                {
                    Sections.Add(item.Section);
                }
            }
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (!Sections.Contains(listBox1.Items[i]))
                {
                    listBox1.Items.RemoveAt(i);
                    i--;
                } 
            }

            Types.Clear();
            foreach (Hardware item in view)
            {
                if (!listBox2.Items.Contains(item.Type))
                {
                    listBox2.Items.Add(item.Type);
                }
                if (!Types.Contains(item.Type))
                {
                    Types.Add(item.Type);
                }
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (!Types.Contains(listBox2.Items[i]))
                {
                    listBox2.Items.RemoveAt(i);
                    i--;
                }
            }

        }

        private void saveItems(object sender, EventArgs e) //save items to file
        {
            FileStream db = new FileStream("db.xml", FileMode.Create, FileAccess.Write);
            XmlSerializer xser = new XmlSerializer(typeof(List<Hardware>));
            xser.Serialize(db, items);
            db.Close();

            isSaved = true;
        }
        private void updateItemsId(int index)
        {
            for (int i = index; i < items.Count; i++)
            {
                items[i].id = i;
            }
        }
        private void loadView()
        {
            view.Clear();
            if (listBox1.SelectedItems.Count > 0)
            {
                foreach (string item in listBox1.SelectedItems)
                {
                    view.AddRange(items.FindAll(x=>x.Section == item));
                }
            } else
            {
                view.AddRange(items);
            }


            if (listBox2.SelectedItems.Count > 0)
            {
                List<Hardware> tempView = new List<Hardware>();
                foreach (string item in listBox2.SelectedItems)
                {
                    tempView.AddRange(view.FindAll(x=>x.Type == item));
                }
                view.Clear();
                view.AddRange(tempView);
            }

            switch (toolStripComboBox1.Text)
            {
                case "Раздел":
                    view.Sort((x, y) => (isInc ? 1 : -1) * x.Section.CompareTo(y.Section));
                    break;
                case "Тип":
                    view.Sort((x, y) => (isInc ? 1 : -1) * x.Type.CompareTo(y.Type));
                    break;
                case "Фирма/Производитель":
                    view.Sort((x, y) => (isInc ? 1 : -1) * x.Firm.CompareTo(y.Firm));
                    break;
                case "Модель":
                    view.Sort((x, y) => (isInc ? 1 : -1) * x.Model.CompareTo(y.Model));
                    break;
                case "Цена":
                    view.Sort((x, y) => (isInc ? 1 : -1) * x.Price.CompareTo(y.Price));
                    break;
                default:
                    MessageBox.Show("Что-то пошло не так при сортировке", "Ooops!");
                    break;
            }
            Source.ResetBindings(false);
        }
        private void deleteItem(int id)
        {
            items.RemoveAt(id);
            updateItemsId(id);
            isSaved = false;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Size.Width - 16;
            dataGridView1.Height = this.Size.Height - 296;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadItems();
            Source.DataSource = view;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = Source;
            toolStripComboBox1.SelectedIndex = 4;

            loadView();
            fieldsUpdate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool isData = dataGridView1.CurrentRow == null;
            toolStripStatusLabel2.Text = DateTime.Now.ToString("hh:mm:ss");
            toolStripStatusLabel5.Text = (dataGridView1.CurrentRow == null) ? "0" : (dataGridView1.CurrentRow.Index + 1).ToString();
            toolStripStatusLabel7.Text = dataGridView1.Rows.Count.ToString();
            toolStripDeletButton.Enabled = (dataGridView1.CurrentRow == null) ? false : true;
            toolStripEditButton.Enabled = (dataGridView1.CurrentRow == null || toolStripAddButton.Checked) ? false : true;
            toolStripFirstRowButton.Enabled = (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == 0) ? false : true;
            toolStripPrevRowButton.Enabled = (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == 0) ? false : true;
            toolStripNextRowButton.Enabled = (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == dataGridView1.Rows.Count - 1) ? false : true;
            toolStripLastRowButton.Enabled = (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == dataGridView1.Rows.Count - 1) ? false : true;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void toolStripDeleteButton_Click(object sender, EventArgs e)
        {
            deleteItem(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            loadView();
            fieldsUpdate();
        }
        private void toolStripEditButton_CheckedChanged(object sender, EventArgs e)
        {
            toolStripConfirmEditButton.Enabled = toolStripCancelEditButton.Enabled = !toolStripConfirmEditButton.Enabled;
            toolStripAddButton.Enabled = !toolStripAddButton.Enabled;
            dataGridView1.Enabled = !dataGridView1.Enabled;
        }
        private void toolStripConfirmEditButton_Click(object sender, EventArgs e)
        {
            toolStripEditButton.Checked = toolStripAddButton.Checked = false;
            isSaved = false;
        }
        private void toolStripCancelEditButton_Click(object sender, EventArgs e)
        {
            toolStripEditButton.Checked = toolStripAddButton.Checked = false;
        }
        private void toolStripFirstRowButton_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[1, 0];
        }
        private void toolStripPrevButton_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.CurrentRow.Index - 1];
        }
        private void toolStripNextButton_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.CurrentRow.Index + 1];
        }
        private void toolStripLastRowButton_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[1, dataGridView1.Rows.Count - 1];
        }
        private void toolStripAddButton_CheckedChanged(object sender, EventArgs e)
        {
            toolStripConfirmEditButton.Enabled = toolStripCancelEditButton.Enabled = !toolStripConfirmEditButton.Enabled;
            dataGridView1.Enabled = !dataGridView1.Enabled;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                var closeDialogResult = MessageBox.Show("Сохранить изменения перед выходом?", "Закрыть", MessageBoxButtons.YesNoCancel);
                if (closeDialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (closeDialogResult == DialogResult.Yes)
                {
                    saveItems(dataGridView1, e);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            isInc = !isInc;
            toolStripButton1.Text = isInc ? "↑" : "↓";
            toolStripButton1.ToolTipText = isInc ? "По возрастанию" : "По убыванию";
            loadView();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadView();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadView();
            fieldsUpdate();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadView();
            //fieldsUpdate();
        }
    }
}
