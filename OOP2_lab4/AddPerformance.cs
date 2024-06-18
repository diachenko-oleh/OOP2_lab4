using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibraryOOP_lab4;

namespace OOP2_lab4
{
    public partial class AddPerformance : Form
    {
        public AuthorDTO author { get; set; }
        public WorkType Work { get; set; }

        public event EventHandler DataSaved;
        public AddPerformance(Author author,WorkType work)
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(WorkType));
            if (author!=null)
            {
                this.author = author.ConvertToDTO();
                textBox1.Text = this.author.name.ToString();
                textBox2.Text = this.author.surname.ToString();
                comboBox1.SelectedItem = work.ToString();
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (author == null)
            {
                author = new AuthorDTO("", ""); 
            }
            author.name = textBox1.Text;
            author.surname = textBox2.Text;
            Work = (WorkType)comboBox1.SelectedItem;
            DataSaved?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
