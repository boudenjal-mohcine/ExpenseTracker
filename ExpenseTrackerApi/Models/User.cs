using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTracker.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public decimal MaxMonthlyExpenses { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
