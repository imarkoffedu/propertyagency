using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MongoDB.Driver;

namespace propertyagency.Clients
{
    public partial class ClientsForm : Form
    {
        public Client selectedClient;  // вибраний клієнт, якщр вікно викликалося для вибору клієнта
        private bool select;           // чи викликалося вікно для вибору клієнта

        private Realtor realtor;       // поточний ріелтор

        private void updateClients()
        {
            var clients = Program.clients.Find(c => true).ToList();
            clientsDataGridView.DataSource = clients;
        }

        /// <summary>
        /// Головна форма клієнтів
        /// </summary>
        /// <param name="select">Чи викликалося це вікно для вибору клієнта</param>
        /// <param name="_client">клас клієнта для його передачі у форму</param>
        public ClientsForm(Realtor realtor, bool select = false, Client _client = null)
        {
            InitializeComponent();
            
            this.realtor = realtor;
            this.select = select;
            this.selectedClient = _client;

            var clients = Program.clients.Find(c => true).ToList();
            
            clientsDataGridView.DataSource = clients;
            
            // сховати стовпець Id
            clientsDataGridView.Columns["Id"].Visible = false;
            
            // словник з назвами заголовків
            var columnHeaders = new Dictionary<string, string>
            {
                { "Surname", "Прізвище" },
                { "Name", "Ім'я" },
                { "Patronymic", "По батькові" },
                { "Email", "Ел. пошта" },
                { "Phone", "Телефон" }
            };
            
            // Проходимося по словнику та встановлюємо заголовки стовпців
            foreach (var columnHeader in columnHeaders)
            {
                if (clientsDataGridView.Columns.Contains(columnHeader.Key))
                    clientsDataGridView.Columns[columnHeader.Key].HeaderText = columnHeader.Value;
            }
        }

        private void addClientButton_Click(object sender, EventArgs e)
        {
            var addClientForm = new AddClientForm(realtor);
            addClientForm.ShowDialog();

            updateClients();
        }

        private void clientsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var client = clientsDataGridView.CurrentRow.DataBoundItem as Client;
            
            // якщо вікно викликалося для вибору клієнта
            if (select)
            {
                selectedClient = client;
                Close();
                return;
            }

            // відкрити форму редагування клієнта
            var addClientForm = new AddClientForm(realtor, client);
            addClientForm.ShowDialog();

            updateClients();
        }
    }
}