using System;
using MongoDB.Driver;
using System.Windows.Forms;

namespace propertyagency
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var email = emailTextBox.Text;
            var password = passwordTextBox.Text;
            
            var realtor = Program.realtors.Find(r => r.Email == email && r.Password == password).FirstOrDefault();
            if (realtor != null)
            {
                var propertiesForm = new PropertiesForm(realtor);
                propertiesForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Неправильна електронна пошта або пароль",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void registerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}