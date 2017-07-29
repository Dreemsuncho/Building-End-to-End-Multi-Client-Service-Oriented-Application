using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        private int _reservationId;
        private int _accountId;
        private int _carId;
        private DateTime _returnDate;
        private DateTime _rentalDate;

        public int ReservationId
        {
            get { return this._reservationId; }
            set
            {
                if (this._reservationId != value)
                {
                    this._reservationId = value;
                    base.OnPropertyChanged(() => this.ReservationId);
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

        public DateTime RentalDate
        {
            get { return this._rentalDate; }
            set
            {
                if (this._rentalDate != value)
                {
                    this._rentalDate = value;
                    base.OnPropertyChanged(() => this.RentalDate);
                }
            }
        }

        public DateTime ReturnDate
        {
            get { return this._returnDate; }
            set
            {
                if (this._returnDate != value)
                {
                    this._returnDate = value;
                    base.OnPropertyChanged(() => this.ReturnDate);
                }
            }
        }
    }
}
