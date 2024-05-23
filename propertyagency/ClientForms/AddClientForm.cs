using MongoDB.Driver;
using System.Windows.Forms;

namespace propertyagency.Clients
{
    public partial class AddClientForm : Form
    {
        private Client client;
        private Realtor realtor;

        public AddClientForm(Realtor realtor, Client client = null)
        {
            InitializeComponent();

            this.client = client;
            this.realtor = realtor;

            if (client != null)
            {
                surnameTextBox.Text = client.Surname;
                nameTextBox.Text = client.Name;
                patronymicTextBox.Text = client.Patronymic;
                emailTextBox.Text = client.Email;
                phoneTextBox.Text = client.Phone;

                addClientButton.Text = "Оновити дані клієнта";
                this.Text = "Оновлення даних клієнта";
                deleteClientButton.Visible = true;
            }
        }

        private void addClientButton_Click(object sender, System.EventArgs e)
        {
            string surname = surnameTextBox.Text;
            string name = nameTextBox.Text;
            string patronymic = patronymicTextBox.Text;
            string email = emailTextBox.Text;
            string phone = phoneTextBox.Text;

            if (string.IsNullOrWhiteSpace(surname) || 
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Якщо клієнт був переданий у конструктор, то оновлюємо його дані
            if (client != null)
            {
                client.Surname = surname;
                client.Name = name;
                client.Patronymic = patronymic;
                client.Email = email;
                client.Phone = phone;

                try
                {
                    var filter = Builders<Client>.Filter.Eq(c => c.Id, client.Id);
                    Program.clients.ReplaceOne(filter, client);

                    MessageBox.Show("Дані клієнта оновлено.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return;
            }
            // Якщо клієнт не був переданий у конструктор, то створюємо нового
            else {
                Client clientToAdd = new Client(surname, name, patronymic, email, phone);


                try
                {
                    Program.clients.InsertOne(clientToAdd);
                    MessageBox.Show("Клієнта додано.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void deleteClientButton_Click(object sender, System.EventArgs e)
        {
            var clientProperties = Program.properties.Find(p => p.OwnerId == client.Id).ToList();

            // код, що реалізує функціонал видалення клієнта разом з нерухомістю
            // якщо клієнт має нерухомість
            if (clientProperties.Count > 0)
            {
                var otherRealtorsProperties = clientProperties.FindAll(p => p.RealtorId != realtor.Id);

                // якщо клієнт має нерухомість, яка належить іншим ріелторам
                if (otherRealtorsProperties.Count > 0)
                {
                    MessageBox.Show(
                        "Ви не можете видалити клієнта, оскільки він має нерухомість, яка належить іншим ріелторам",
                        "Видалення клієнта",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning
                        );
                    return;
                }
                
                // відкрити форму видалення клієнта разом з нерухомістю
                var deleteClientForm = new Clients.DeleteClientForm(client, clientProperties);
                deleteClientForm.ShowDialog();
            }

            // попередити про видалення
            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити клієнта?", 
                "Видалення клієнта", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var filter = Builders<Client>.Filter.Eq(c => c.Id, client.Id);
                    Program.clients.DeleteOne(filter);

                    MessageBox.Show("Клієнта видалено.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}