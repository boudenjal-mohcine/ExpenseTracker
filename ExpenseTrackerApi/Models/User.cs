using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTracker.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public decimal MaxMonthlyExpenses { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
