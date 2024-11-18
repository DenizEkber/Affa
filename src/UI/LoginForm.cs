using AFFA.src.DEPO.DataBase.Context;
using AFFA.src.Helper;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AFFA.src.UI
{
    public partial class LoginForm : Form
    {
        TextBox txtUsername, txtPassword;
        Button btnLogin;
        Label lblUsername, lblPassword;
        private readonly AFFADbContext _dbContext = new AFFADbContext();

        public LoginForm()
        {
            Component();
        }

        private void Component()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();

            this.SuspendLayout();

            this.lblUsername.Text = "İstifadəçi Adı:";
            this.lblUsername.Location = new Point(20, 20);

            this.txtUsername.Location = new Point(120, 20);
            this.txtUsername.Size = new Size(200, 20);

            this.lblPassword.Text = "Şifrə:";
            this.lblPassword.Location = new Point(20, 60);

            this.txtPassword.Location = new Point(120, 60);
            this.txtPassword.Size = new Size(200, 20);
            this.txtPassword.PasswordChar = '*';

            this.btnLogin.Text = "Daxil Ol";
            this.btnLogin.Location = new Point(120, 100);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            this.ClientSize = new Size(350, 150);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPassword);
            this.ResumeLayout(false);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == txtUsername.Text);

            if (user != null && PasswordHelper.VerifyPasswordHash(txtPassword.Text, user.PasswordHash, user.PasswordSalt))
            {
                switch (user.Role)
                {
                    case "admin":
                        AdminForm adminForm = new AdminForm();
                        adminForm.Show();
                        break;

                    case "customer":
                        var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerName == user.Username);
                        if (customer != null)
                        {
                            CustomerForm customerForm = new CustomerForm(customer);
                            customerForm.Show();
                        }
                        break;

                    case "referee":
                        AdminForm refereeForm = new AdminForm(); 
                        refereeForm.Show();
                        break;
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Yanlış istifadəçi adı və ya şifrə!");
            }
        }
    }
}
