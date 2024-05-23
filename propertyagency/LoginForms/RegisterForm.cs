using System;
using MongoDB.Driver;
using System.Windows.Forms;

namespace propertyagency
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }
        private void loginLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            var surname = surnameTextBox.Text;
            var name = nameTextBox.Text;
            var patronymic = patronymicTextBox.Text;
            var email = emailTextBox.Text;
            var phone = phoneTextBox.Text;
            var password = passwordTextBox.Text;
            var confirmPassword = repeatPasswordTextBox.Text;
            
            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не співпадають");
                return;
            }

            if (surname == "" || name == "" || patronymic == "" || email == "" || phone == "" || password == "")
            {
                MessageBox.Show("Заповніть всі поля");
                return;
            }
            
            var realtor = new Realtor(surname, name, patronymic, email, phone, password);
            
            try
            {
                Program.realtors.InsertOne(realtor);
                MessageBox.Show("Реєстрація успішна");
                
                loginLinkLabel_LinkClicked(null, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loginLinkLabel_LinkClicked(null, null);
        }
    }
}