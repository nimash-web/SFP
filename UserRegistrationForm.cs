using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    // Borrower.cs
    public class Borrower
    {
        public int UserNumber { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }

        public Borrower(int userNumber, string name, string sex, string nationalId, string address)
        {
            UserNumber = userNumber;
            Name = name;
            Sex = sex;
            NationalID = nationalId;
            Address = address;
        }
    }

    // DatabaseHelper.cs
    public class DatabaseHelper
    {
        private string connectionString = "Data Source=YOUR_SERVER;Initial Catalog=UserRegistrationDB;Integrated Security=True";

        public bool AddBorrower(Borrower borrower)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Borrowers (UserNumber, Name, Sex, NationalID, Address) " +
                               "VALUES (@UserNumber, @Name, @Sex, @NationalID, @Address)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserNumber", borrower.UserNumber);
                command.Parameters.AddWithValue("@Name", borrower.Name);
                command.Parameters.AddWithValue("@Sex", borrower.Sex);
                command.Parameters.AddWithValue("@NationalID", borrower.NationalID);
                command.Parameters.AddWithValue("@Address", borrower.Address);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    // Form1.cs
    public partial class Form1 : Form
    {
        private TextBox txtUserNumber;
        private TextBox txtName;
        private TextBox txtSex;
        private TextBox txtNationalID;
        private TextBox txtAddress;
        private Button btnRegister;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUserNumber = new TextBox { Location = new System.Drawing.Point(150, 20), Width = 200 };
            this.txtName = new TextBox { Location = new System.Drawing.Point(150, 60), Width = 200 };
            this.txtSex = new TextBox { Location = new System.Drawing.Point(150, 100), Width = 200 };
            this.txtNationalID = new TextBox { Location = new System.Drawing.Point(150, 140), Width = 200 };
            this.txtAddress = new TextBox { Location = new System.Drawing.Point(150, 180), Width = 200 };
            this.btnRegister = new Button { Text = "Register", Location = new System.Drawing.Point(150, 220), Width = 200 };

            this.btnRegister.Click += new EventHandler(this.btnRegister_Click);

            this.Controls.Add(new Label { Text = "User Number:", Location = new System.Drawing.Point(50, 20) });
            this.Controls.Add(txtUserNumber);
            this.Controls.Add(new Label { Text = "Name:", Location = new System.Drawing.Point(50, 60) });
            this.Controls.Add(txtName);
            this.Controls.Add(new Label { Text = "Sex:", Location = new System.Drawing.Point(50, 100) });
            this.Controls.Add(txtSex);
            this.Controls.Add(new Label { Text = "National ID:", Location = new System.Drawing.Point(50, 140) });
            this.Controls.Add(txtNationalID);
            this.Controls.Add(new Label { Text = "Address:", Location = new System.Drawing.Point(50, 180) });
            this.Controls.Add(txtAddress);
            this.Controls.Add(btnRegister);

            this.Text = "User Registration";
            this.ClientSize = new System.Drawing.Size(400, 300);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                int userNumber = int.Parse(txtUserNumber.Text);
                string name = txtName.Text;
                string sex = txtSex.Text;
                string nationalId = txtNationalID.Text;
                string address = txtAddress.Text;

                Borrower borrower = new Borrower(userNumber, name, sex, nationalId, address);
                DatabaseHelper db = new DatabaseHelper();
                bool success = db.AddBorrower(borrower);

                if (success)
                    MessageBox.Show("Registration successful.");
                else
                    MessageBox.Show("Registration failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

