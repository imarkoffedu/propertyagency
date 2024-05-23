using System;
using System.Windows.Forms;
using MongoDB.Driver;

namespace propertyagency
{
    static class Program
    {
        public static IMongoCollection<Client> clients;
        public static IMongoCollection<Realtor> realtors;
        public static IMongoCollection<PropertyType> propertyTypes;
        public static IMongoCollection<Property> properties;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()  
        {
            var mongoUri = Config.mongoUri;

            IMongoClient client;
            
            try
            {
                client = new MongoClient(mongoUri);
                var database = client.GetDatabase("propertyagency");
                clients = database.GetCollection<Client>("clients");
                realtors = database.GetCollection<Realtor>("realtors");
                propertyTypes = database.GetCollection<PropertyType>("propertyTypes");
                properties = database.GetCollection<Property>("properties");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}