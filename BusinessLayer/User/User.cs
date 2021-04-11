using System;
using System.Collections.Generic;
using Budgets.BusinessLayer.Wallets;

namespace Budgets.BusinessLayer.User
{
    public class User
    {
        public Guid Guid { get; }
        public string Login { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<Wallet> Wallets { get; }
        public List<SharedWallet> SharedWallets { get; }
        public List<Category> Categories { get; }

        // public string FullName
        // {
        //     get
        //     {
        //         string result = FirstName;
        //         if (!string.IsNullOrWhiteSpace(LastName))
        //         {
        //             if (!string.IsNullOrWhiteSpace(FirstName))
        //                 result += ", ";
        //
        //             result += LastName;
        //         }
        //
        //         return result;
        //     }
        // }

        public User(Guid id, string login, string firstName, string lastName, string email)
        {
            Guid = id;
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Email = email;

            Wallets = new List<Wallet>();
            SharedWallets = new List<SharedWallet>();
            Categories = new List<Category>();
        }


        /**
         * @param index  begins with 1
         */
        public SharedWallet ShareWallet(int index)
        {
            return new SharedWallet(Wallets[index - 1]);
        }

        public bool Validate()
        {
            bool res = true;

            if (string.IsNullOrWhiteSpace(FirstName))
                res = false;
            if (string.IsNullOrWhiteSpace(LastName))
                res = false;
            if (string.IsNullOrWhiteSpace(Email))
                res = false;

            return res;
        }
    }
}
