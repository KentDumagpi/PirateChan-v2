using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PirateChan
{
    public partial class CustomerLanding : Form
    {
        public CustomerLanding()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void figurine_btn_Click(object sender, EventArgs e)
        {
            // Check if an instance of Figurines form already exists
            Figurines existingForm = Application.OpenForms.OfType<Figurines>().FirstOrDefault();

            if (existingForm != null)
            {
                // If an instance exists, bring it to the front
                existingForm.BringToFront();
            }
            else
            {
                // If not, create a new instance and show it
                Figurines fig = new Figurines();
                fig.Show();
            }

            // Hide the current form
            this.Hide();
        }

        private void plushy_btn_Click(object sender, EventArgs e)
        {
            Plushy pls = new Plushy();
            pls.Show();
            this.Hide();
        }
    }
}
