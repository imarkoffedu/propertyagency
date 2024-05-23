using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MongoDB.Driver;

namespace propertyagency
{
    public partial class PropertiesForm : Form
    {
        public Realtor realtor;
        
        void UpdateProperties()
        {
            var properties = Program.properties.Find(p => p.RealtorId == realtor.Id).ToList();
            propertiesDataGridView.DataSource = properties;
        }
        
        public PropertiesForm(Realtor realtor)
        {
            this.realtor = realtor;
            InitializeComponent();
            
            // ініціали ріелтора
            var initials = realtor.Surname + " " + realtor.Name + " " + realtor.Patronymic;
            initialsLabel.Text = initials;

            // позиції ініціалів
            var point = initialsLabel.Location;
            point.X = logoutButton.Location.X - initialsLabel.Width - 5;
            initialsLabel.Location = point;
            
            // відобразити нерухомість
            UpdateProperties();
            
            // сховати стовпець Id
            propertiesDataGridView.Columns["Id"].Visible = false;
            propertiesDataGridView.Columns["RealtorId"].Visible = false;
            
            // Створюємо словник з назвами заголовків
            Dictionary<string, string> columnHeaders = new Dictionary<string, string>
            {
                { "PropertyTypeId", "Тип" },
                { "Address", "Адреса" },
                { "Area", "Площа" },
                { "Price", "Ціна" },
                { "Status", "Статус" },
                { "Description", "Опис" },
                { "OwnerId", "ID власника" }
            };

            // Проходимося по словнику та встановлюємо заголовки стовпців
            foreach (var columnHeader in columnHeaders)
            {
                if (propertiesDataGridView.Columns.Contains(columnHeader.Key))
                    propertiesDataGridView.Columns[columnHeader.Key].HeaderText = columnHeader.Value;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var addPropertyForm = new AddPropertyForm(realtor);
            addPropertyForm.ShowDialog();

            UpdateProperties();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете вийти?", 
                "Вихід", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                var loginForm = new LoginForm();
                loginForm.Show();
                Hide();
            }
        }

        private void clientsButton_Click(object sender, EventArgs e)
        {
            var clientsForm = new Clients.ClientsForm(realtor);
            clientsForm.ShowDialog();

            UpdateProperties();
        }

        private void PropertiesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void propertiesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var property = propertiesDataGridView.CurrentRow.DataBoundItem as Property;

            var addPropertyForm = new AddPropertyForm(realtor, property);
            addPropertyForm.ShowDialog();

            UpdateProperties();
        }
    }
}