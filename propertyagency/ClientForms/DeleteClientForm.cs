using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;

namespace propertyagency.Clients
{
    public partial class DeleteClientForm : Form
    {
        private Client client;
        private List<Property> properties;

        public DeleteClientForm(Client client, List<Property> properties)
        {
            InitializeComponent();

            this.client = client;
            this.properties = properties;

            IconPictureBox.Image = System.Drawing.Icon.FromHandle(SystemIcons.Question.Handle).ToBitmap();
            messageLabel.Text = $"{client.Surname} {client.Name} {client.Patronymic} пов'язаний з такими нерухомостями. Що бажаєте з ними зробити?";

            dataGridView.DataSource = properties;

            // сховати стовпець Id
            dataGridView.Columns["Id"].Visible = false;
            dataGridView.Columns["RealtorId"].Visible = false;
            dataGridView.Columns["OwnerId"].Visible = false;

            // Створюємо словник з назвами заголовків
            Dictionary<string, string> columnHeaders = new Dictionary<string, string>
            {
                { "PropertyTypeId", "Тип" },
                { "Address", "Адреса" },
                { "Area", "Площа" },
                { "Price", "Ціна" },
                { "Status", "Статус" },
                { "Description", "Опис" }
            };

            // Проходимося по словнику та встановлюємо заголовки стовпців
            foreach (var columnHeader in columnHeaders)
            {
                if (dataGridView.Columns.Contains(columnHeader.Key))
                    dataGridView.Columns[columnHeader.Key].HeaderText = columnHeader.Value;
            }
        }

        private void deletePropertiesButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити усі пов'язані з клієнтом нерухомості?", 
                "Видалення нерухомостей", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );  

            if (result == DialogResult.Yes)
            {
                foreach (var property in properties)
                {
                    try
                    {
                        Program.properties.DeleteOne(p => p.Id == property.Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                MessageBox.Show("Нерухомості видалено!");
                Close();
            }
        }

        private void keepPropertiesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "При видаленні клієнта усі нерухомості, які йому належать, стануть без власника.", 
                "Видалення нерухомостей", 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }
    }
}
