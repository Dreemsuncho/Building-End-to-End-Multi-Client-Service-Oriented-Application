using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Account : ObjectBase
    {
        private int _accountId;
        private string _loginEmail;
        private string _firstName;
        private string _lastName;
        private string _address;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _creditCard;
        private string _expDate;

        public int AccountId
        {
            get { return this._accountId; }
            set
            {
                if (this._accountId != value)
                {
                    this._accountId = value;
                    base.OnPropertyChanged(() => this.AccountId);
                }
            }
        }

        public string LoginEmail
        {
            get { return this._loginEmail; }
            set
            {
                if (this._loginEmail != value)
                {
                    this._loginEmail = value;
                    base.OnPropertyChanged(() => this.LoginEmail);
                }
            }
        }

        public string FirstName
        {
            get { return this._firstName; }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    base.OnPropertyChanged(() => this.FirstName);
                }
            }
        }

        public string LastName
        {
            get { return this._lastName; }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    base.OnPropertyChanged(() => this.LastName);
                }
            }
        }

        public string Address
        {
            get { return this._address; }
            set
            {
                if (this._address != value)
                {
                    this._address = value;
                    base.OnPropertyChanged(() => this.Address);
                }
            }
        }

        public string City
        {
            get { return this._city; }
            set
            {
                if (this._city != value)
                {
                    this._city = value;
                    base.OnPropertyChanged(() => this.City);
                }
            }
        }

        public string State
        {
            get { return this._state; }
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    base.OnPropertyChanged(() => this.State);
                }
            }
        }

        public string ZipCode
        {
            get { return this._zipCode; }
            set
            {
                if (this._zipCode != value)
                {
                    this._zipCode = value;
                    base.OnPropertyChanged(() => this.ZipCode);
                }
            }
        }

        public string CreditCard
        {
            get { return this._creditCard; }
            set
            {
                if (this._creditCard != value)
                {
                    this._creditCard = value;
                    base.OnPropertyChanged(() => this.CreditCard);
                }
            }
        }

        public string ExpDate
        {
            get { return this._expDate; }
            set
            {
                if (this._expDate != value)
                {
                    this._expDate = value;
                    base.OnPropertyChanged(() => this.ExpDate);
                }
            }
        }
    }
}
