using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLZadanie4
{
    public partial class Form1 : Form
    {
        Logic lg = new Logic();
        public Form1()
        {
            InitializeComponent();
            //skrytie stlpcov a nasledne je potrebne vymenovat stlpce na zobrazenie
            dgwPerson.AutoGenerateColumns = false;
            dgwPerson.DataSource = lg.FillDataSet();
            dgwPerson.DataMember = "Person";
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            lg.InsertBussinessEntity();
            string name = txtFistName.Text;
            string surname = txtLastName.Text;
            lg.InsertPerson(name, surname);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                int.TryParse(txtId.Text, out id);
                string name = txtFistName.Text;
                string surname = txtLastName.Text;
                lg.UpdatePerson(id, name, surname);
            }
            catch
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                int.TryParse(txtId.Text, out id);                
                lg.DeletePerson(id);
            }
            catch
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
