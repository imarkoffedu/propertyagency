using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace propertyagency
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public Client(string surname, string name, string patronymic, string email, string phone)
        {
            this.Surname = surname;
            this.Name = name;
            this.Patronymic = patronymic;
            this.Email = email;
            this.Phone = phone;
        }
    }
    
    public class Realtor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        
        public Realtor(string surname, string name, string patronymic, string email, string phone, string password)
        {
            this.Surname = surname;
            this.Name = name;
            this.Patronymic = patronymic;
            this.Email = email;
            this.Phone = phone;
            this.Password = password;
        }
    }
    
    public class PropertyType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Type { get; set; }
        
        public PropertyType(string type)
        {
            this.Type = type;
        }
    }
    
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PropertyTypeId { get; set; }
        public string Address { get; set; }
        public float Area { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string RealtorId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }
        public string Description { get; set; }
        
        public Property(string propertyTypeId, string address, float area, int price, string status, string realtorId, string ownerId, string description)
        {
            this.PropertyTypeId = propertyTypeId;
            this.Address = address;
            this.Area = area;
            this.Price = price;
            this.Status = status;
            this.RealtorId = realtorId;
            this.OwnerId = ownerId;
            this.Description = description;
        }
        
    }
}