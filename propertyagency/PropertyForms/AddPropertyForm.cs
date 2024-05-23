using MongoDB.Driver;
using System;
using System.Windows.Forms;

namespace propertyagency
{
    public partial class AddPropertyForm : Form
    {
        Realtor realtor;
        Client client;
        Property property; // для редагування нерухомості
        
        public AddPropertyForm(Realtor realtor, Property property = null)
        {
            InitializeComponent();

            this.realtor = realtor;

            PropertyType[] propertyTypes = Program.propertyTypes.Find(_ => true).ToList().ToArray();
            
            foreach (var propertyType in propertyTypes)
            {
                typesComboBox.Items.Add(propertyType.Type);
            }

            if (property != null)
            {
                this.property = property;

                typesComboBox.Text = Program.propertyTypes.Find(x => x.Id == property.PropertyTypeId).FirstOrDefault().Type; // тип нерухомості за Id
                addressTextBox.Text = property.Address;
                areaTextBox.Text = property.Area.ToString();
                priceTextBox.Text = property.Price.ToString();
                statusTextBox.Text = property.Status;
                descriptionTextBox.Text = property.Description;

                // редагування назви кнопки на власника
                client = Program.clients.Find(c => c.Id == property.OwnerId).FirstOrDefault();
                if (client != null) {
                    ownerButton.Text = client.Surname + " " + client.Name + " " + client.Patronymic;
                }

                addPropertyButton.Text = "Оновити нерухомість";
                this.Text = "Оновлення нерухомості";
                deletePropertyButton.Visible = true;
            }
        }

        private void ownerButton_Click(object sender, EventArgs e)
        {
            // відкрити форму вибору власника
            var selectOwnerForm = new Clients.ClientsForm(realtor, true, client);
            selectOwnerForm.ShowDialog();

            // отримати власника з форми
            client = selectOwnerForm.selectedClient;

            if (client != null)
            {
                // змінити текст кнопки
                string initials = client.Surname + " " + client.Name + " " + client.Patronymic;
                ownerButton.Text = initials;
            }
        }

        private void addPropertyButton_Click(object sender, EventArgs e)
        {
            string type = typesComboBox.Text;
            string address = addressTextBox.Text;
            float area = float.Parse(areaTextBox.Text);
            int price = int.Parse(priceTextBox.Text);
            string status = statusTextBox.Text;
            string description = descriptionTextBox.Text;

            if (type == "" || address == "" || area == 0 || status == "" || price == 0 || description == "")
            {
                MessageBox.Show("Заповніть всі поля");
            }

            else
            {
                PropertyType propertyType = Program.propertyTypes.Find(x => x.Type == type).FirstOrDefault(); // знайти тип нерухомості
                
                var propertyTypeId = propertyType.Id;
                var clientId = client.Id;
                var realtorId = realtor.Id;

                Property property = new Property(propertyTypeId, address, area, price, status, realtorId, clientId, description);

                // оновлення нерухомості
                if (this.property != null)
                {
                    property.Id = this.property.Id;

                    try
                    {
                        var filter = Builders<Property>.Filter.Eq(p => p.Id, property.Id);
                        Program.properties.ReplaceOne(filter, property);

                        MessageBox.Show("Нерухомість оновлено.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка оновлення нерухомості: \n{ex.Message}");
                    }

                    return;
                }

                // додавання нерухомості
                else
                {
                    try
                    {
                        Program.properties.InsertOne(property);
                        MessageBox.Show("Нерухомість додана.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                    }
                    catch (MongoWriteException ex)
                    {
                        MessageBox.Show($"Помилка додавання нерухомості: \n{ex.Message}");
                    }
                }
            }
        }

        private void deletePropertyButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити нерухомість?", 
                "Видалення нерухомості", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                try
                {
                    var filter = Builders<Property>.Filter.Eq(p => p.Id, property.Id);
                    Program.properties.DeleteOne(filter);

                    MessageBox.Show("Нерухомість видалена.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка видалення нерухомості: \n{ex.Message}");
                }
            }
        }
    }
}