using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        private int _rentalId;
        private int _accountId;
        private int _carId;
        private DateTime _dateRented;
        private DateTime _dateDue;
        private DateTime? _dateReturned;

        public int RentalId
        {
            get { return this._rentalId; }
            set
            {
                if (this._rentalId != value)
                {
                    this._rentalId = value;
                    base.OnPropertyChanged(() => this.RentalId);
                }
            }
        }

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

        public int CarId
        {
            get { return this._carId; }
            set
            {
                if (this._carId != value)
                {
                    this._carId = value;
                    base.OnPropertyChanged(() => this.CarId);
                }
            }
        }

        public DateTime DateRented
        {
            get { return this._dateRented; }
            set
            {
                if (this._dateRented != value)
                {
                    this._dateRented = value;
                    base.OnPropertyChanged(() => this.DateRented);
                }
            }
        }

        public DateTime DateDue
        {
            get { return this._dateDue; }
            set
            {
                if (this._dateDue != value)
                {
                    this._dateDue = value;
                    base.OnPropertyChanged(() => this.DateDue);
                }
            }
        }

        public DateTime? DateReturned
        {
            get { return this._dateReturned; }
            set
            {
                if (this._dateReturned != value)
                {
                    this._dateReturned = value;
                    base.OnPropertyChanged(() => this.DateReturned);
                }
            }
        }
    }
}
