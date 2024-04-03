using PirateChan.App_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PirateChan
{
    public partial class Login : Form
    {
        private SqlConnection conn;
        public Login()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=DESKTOP-I3H1TD1\\SQLEXPRESS02;Initial Catalog=users;Integrated Security=True");
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username_txt.Text))
            {
                MessageBox.Show("Enter the username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrWhiteSpace(pwd_txt.Text))
            {
                MessageBox.Show("Enter the password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    // Trim any extra spaces from the username
                    string username = username_txt.Text.Trim();

                    SqlCommand cmd = new SqlCommand("SELECT pwd, acctype FROM customer_table WHERE username = @username", conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        // Retrieve the hashed password and user type from the database
                        string dbHashedPassword = dt.Rows[0]["pwd"].ToString().Trim();
                        string userType = dt.Rows[0]["acctype"].ToString().Trim();

                        // Directly compare the hashed passwords with case-insensitivity
                        if (string.Equals(dbHashedPassword, HashPassword(pwd_txt.Text), StringComparison.OrdinalIgnoreCase))
                        {
                            if (userType.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                            {
                                var customerDashboard = new CustomerLanding();
                                customerDashboard.ShowDialog();
                            }
                            else if (userType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                var adminDashboard = new Admin_Dashboard();
                                adminDashboard.ShowDialog();
                            }

                            // Successful login, so return
                            return;
                        }
                    }

                    // If we reach this point, either the user doesn't exist or the password is incorrect
                    MessageBox.Show("Invalid username or password");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }

            this.Hide();

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a base64 encoded string (length is longer than 50)
                string base64Hash = Convert.ToBase64String(hashedBytes);

                // Truncate the hash to 50 characters to match the database column length
                if (base64Hash.Length > 50)
                {
                    base64Hash = base64Hash.Substring(0, 50);
                }

                return base64Hash;
            }
        }




        public void login() { 
            Login log = new Login();
            log.Show();
        }

        private void registerhere_btn1_Click(object sender, EventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            this.Hide();
        }

        private void username_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login_btn.PerformClick();
            }
        }

        private void pwd_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login_btn.PerformClick();
            }
        }

        private void registerhere_btn2_Click(object sender, EventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            this.Hide();
        }
    }
}
