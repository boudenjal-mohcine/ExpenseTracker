using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTracker.Models
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string? Name { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonIgnore]
        public User? User { get; set; }
    }
}
