using System;
using System.Data.SqlClient;
using System.Windows.Forms;

public partial class UserRegistrationForm : Form
{
    // Database connection string - adjust as per your SQL Server setup
    // Example for local SQL Server Express:
    // string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SarasaviLibraryDB;Integrated Security=True;";
    // Example for localDB as in search result [5]:
    string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Path\To\Your\SarasaviLibraryDB.mdf;Integrated Security=True";
    // IMPORTANT: Replace C:\Path\To\Your\SarasaviLibraryDB.mdf with the actual path to your .mdf file if using LocalDB.

    public UserRegistrationForm()
    {
        InitializeComponent();
        // Populate ComboBox for Sex if you are using one
        // cmbSex.Items.AddRange(new string[] { "Male", "Female", "Other" });
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        // Retrieve data from form controls
        string userNumber = txtUserNumber.Text.Trim();
        string name = txtName.Text.Trim();
        string sex = cmbSex.SelectedItem?.ToString(); // Or get from RadioButton
        string nic = txtNIC.Text.Trim();
        string address = txtAddress.Text.Trim();

        // Basic Validation: Ensure all fields are filled
        if (string.IsNullOrEmpty(userNumber) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(nic) || string.IsNullOrEmpty(address))
        {
            MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            try
            {
                cn.Open();

                // Step 1: Check if User Number or NIC already exists [5]
                // Assuming you have a table named 'Users' with columns: UserNumber, Name, Sex, NIC, Address
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE UserNumber = @UserNumber OR NIC = @NIC";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, cn))
                {
                    checkCmd.Parameters.AddWithValue("@UserNumber", userNumber);
                    checkCmd.Parameters.AddWithValue("@NIC", nic);

                    int userExists = (int)checkCmd.ExecuteScalar();
                    if (userExists > 0)
                    {
                        MessageBox.Show("A user with this User Number or NIC already exists. Please try a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Step 2: Insert the new user if not already existing [5]
                string insertQuery = "INSERT INTO Users (UserNumber, Name, Sex, NIC, Address) VALUES (@UserNumber, @Name, @Sex, @NIC, @Address)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, cn))
                {
                    cmd.Parameters.AddWithValue("@UserNumber", userNumber);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Sex", sex);
                    cmd.Parameters.AddWithValue("@NIC", nic);
                    cmd.Parameters.AddWithValue("@Address", address);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Optionally, clear the form or close it
                        ClearForm();
                        // this.Close(); // Or navigate to login form
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ClearForm()
    {
        txtUserNumber.Clear();
        txtName.Clear();
        cmbSex.SelectedIndex = -1; // Reset ComboBox
        txtNIC.Clear();
        txtAddress.Clear();
        txtUserNumber.Focus();
    }

    // Placeholder for controls - Add these to your form designer
    // private System.Windows.Forms.TextBox txtUserNumber;
    // private System.Windows.Forms.TextBox txtName;
    // private System.Windows.Forms.ComboBox cmbSex;
    // private System.Windows.Forms.TextBox txtNIC;
    // private System.Windows.Forms.TextBox txtAddress;
    // private System.Windows.Forms.Button btnRegister;
}
