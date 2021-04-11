using System;
using System.Collections.Generic;

namespace Budgets.BusinessLayer
{
    public class Transaction
    {
        private int _id;
        private decimal _sum;
        private string _currency;
        private Category _category;
        private string _description;
        private DateTime _date;
        private List<string> _files;

        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        public decimal Sum
        {
            get => _sum;
            set => _sum = value;
        }

        public string Currency
        {
            get => _currency;
            set => _currency = value;
        }

        public Category Category
        {
            get => _category;
            set => _category = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public List<string> Files
        {
            get => _files;
            set => _files = value;
        }


        public Transaction()
        {
            Files = new List<string>();
        }

        public Transaction(int id) : this()
        {
            Id = id;
        }


        public bool Validate()
        {
            bool res = true;

            if (string.IsNullOrWhiteSpace(_currency))
                res = false;
            if (_category == null)
                res = false;

            return res;
        }
    }
}
