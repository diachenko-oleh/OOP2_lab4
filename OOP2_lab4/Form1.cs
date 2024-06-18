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
using System.Xml.Serialization;
using System.Windows.Markup;
using System.IO;



namespace OOP2_lab4
{
    public partial class Form1 : Form
    {
        public List<Concert> concerts = new List<Concert>();
        public string organisator { get; set; }
        public DateTime date { get; set; }
        public List<Performance> performances = new List<Performance>();
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)          //Додати концерт
        {
            using (EditConcerts addConcert = new EditConcerts(null, new DateTime(), performances))
            {
                addConcert.DataSaved += (s, args) =>
                {
                    organisator = addConcert.organiser;
                    date = DateTime.Parse(addConcert.date);
                    performances = addConcert.performances;
                    concerts.Add(new Concert(organisator,date,performances));
                    listBox1.Items.Add(concerts.Last().ToString());
                };
                addConcert.ShowDialog();
            }
        }
        private void button4_Click(object sender, EventArgs e)          //Редагувати концерт
        {
            int selectedIndex = listBox1.SelectedIndex;
            ConcertDTO selectedConcert = concerts[selectedIndex].ConvertToDTO();

            using (EditConcerts editPerformance = new EditConcerts(selectedConcert.organisator, selectedConcert.concertDate, selectedConcert.performances))
            {
                editPerformance.DataSaved += (s, args) =>
                {
                    selectedConcert.organisator = editPerformance.organiser;
                    selectedConcert.concertDate = DateTime.Parse(editPerformance.date);
                    listBox1.Items[selectedIndex] = selectedConcert.ConvertFromDTO().ToString();
                    concerts[selectedIndex] = selectedConcert.ConvertFromDTO();
                };

                editPerformance.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)      // Зберегти в файл
        {

            SaveData();

        }
        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            using (StreamWriter writer = new StreamWriter("concert.xml"))
            {
                serializer.Serialize(writer, Concert.ConvertToString(concerts));
                MessageBox.Show("Data is saved");
            }
        }
        private void button1_Click(object sender, EventArgs e)          // Прочитати з файлу
        {
            List<Concert> concerts = new List<Concert>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            using (StreamReader reader = new StreamReader("concert.xml"))
            {
                concerts= Concert.ConvertFromString((List<string>)serializer.Deserialize(reader));
            }
            listBox1.Items.Clear();
            foreach (var item in concerts)
            {
                listBox1.Items.Add(item);
            }
            
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                button4.Enabled = true; 
            }
            else
            {
                button4.Enabled = false; 
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(listBox1.SelectedItem.ToString());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("Зберегти дані?", "", MessageBoxButtons.YesNo);
            if (result==DialogResult.Yes)
            {
                SaveData();
            }
            base.OnClosing(e);
        }

    }
}
