using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using AFFA.src.DEPO.DataBase.Context;
using AFFA.src.DEPO.DataBase.Entity;
using AFFA.src.Helper;

namespace AFFA.src.UI
{
    public partial class RegisterForm : Form
    {
        private readonly AFFADbContext _dbContext = new AFFADbContext();
        private TextBox txtUsername, txtPassword;
        private ComboBox cmbRole;
        private Button btnRegister;
        private Label lblUsername, lblPassword, lblRole;

        public RegisterForm()
        {
            Component();
        }

        private void Component()
        {
            // Initialize components
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.cmbRole = new ComboBox();
            this.btnRegister = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblRole = new Label();

            this.SuspendLayout();

            // Username Label
            this.lblUsername.Text = "İstifadəçi adı:";
            this.lblUsername.Location = new Point(20, 20);
            this.lblUsername.Size = new Size(100, 20);

            // Username TextBox
            this.txtUsername.Location = new Point(120, 20);
            this.txtUsername.Size = new Size(200, 20);

            // Password Label
            this.lblPassword.Text = "Şifrə:";
            this.lblPassword.Location = new Point(20, 60);
            this.lblPassword.Size = new Size(100, 20);

            // Password TextBox (password is hidden)
            this.txtPassword.Location = new Point(120, 60);
            this.txtPassword.Size = new Size(200, 20);
            this.txtPassword.UseSystemPasswordChar = true;

            // Role Label
            this.lblRole.Text = "Rol:";
            this.lblRole.Location = new Point(20, 100);
            this.lblRole.Size = new Size(100, 20);

            // Role ComboBox
            this.cmbRole.Location = new Point(120, 100);
            this.cmbRole.Size = new Size(200, 20);
            this.cmbRole.Items.Add("customer");
            this.cmbRole.Items.Add("admin"); // Example of another role

            // Register Button
            this.btnRegister.Text = "Qeydiyyatdan Keç";
            this.btnRegister.Location = new Point(120, 140);
            this.btnRegister.Size = new Size(200, 30);
            this.btnRegister.Click += new EventHandler(this.btnRegister_Click);

            // Add controls to form
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnRegister);

            // Form Settings
            this.ClientSize = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Qeydiyyat";

            this.ResumeLayout(false);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Bütün sahələri doldurun!");
                return;
            }

            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                MessageBox.Show("Bu istifadəçi adı artıq mövcuddur!");
                return;
            }

            PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string passwordSalt);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            if (role == "customer")
            {
                var customer = new Customer
                {
                    CustomerName = username,
                    Balance = 0 
                };
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();
            }

            MessageBox.Show("Qeydiyyat uğurla tamamlandı!");
            this.Close();
        }
    }
}
