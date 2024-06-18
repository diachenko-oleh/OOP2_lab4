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
    public partial class EditConcerts : Form
    {
        public string organiser { get; set; }
        public string date { get; set; }
        public Author author { get; set; }
        public WorkType work { get; set; }

        public List<Performance> performances = new List<Performance>();

        public event EventHandler DataSaved;
        public EditConcerts(string organisator, DateTime date, List<Performance> performances)
        {
            InitializeComponent();
            button3.Enabled = false;
            this.organiser = organisator;
            this.date = date.ToString();
            this.performances = performances;
            if (organiser != null)
            {
                textBox1.Text = organisator;
                textBox2.Text = this.date;
                listBox1.Items.Clear();
                foreach (var performance in performances)
                {
                    listBox1.Items.Add(performance.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)      // Редагувати концерт
        {
            organiser = textBox1.Text;
            date = textBox2.Text;
            DataSaved?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)      //Додати виступ
        {
            using (AddPerformance addPerformance = new AddPerformance(new Author("",""), 0))
            {
                addPerformance.DataSaved += (s, args) => {
                    author = addPerformance.author.ConvertFromDTO();
                    work = addPerformance.Work;
                    performances.Add(new Performance(author, work));
                    listBox1.Items.Add(performances.Last().ToString());
                };
                addPerformance.ShowDialog();
            }
        }
        private void button3_Click(object sender, EventArgs e)      //Редагувати виступ
        {
            int selectedIndex = listBox1.SelectedIndex;
            PerformanceDTO selectedPerformance = performances[selectedIndex].ConvertToDTO();

            using (AddPerformance editPerformance = new AddPerformance(selectedPerformance.author, selectedPerformance.work))
            {
                editPerformance.DataSaved += (s, args) =>
                {
                    selectedPerformance.author = editPerformance.author.ConvertFromDTO();
                    selectedPerformance.work = editPerformance.Work;
                    listBox1.Items[selectedIndex] = selectedPerformance.ConvertFromDTO().ToString();
                    performances[selectedIndex] = selectedPerformance.ConvertFromDTO();
                };

                editPerformance.ShowDialog();
            }

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
