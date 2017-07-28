using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Car
    {
        private int _carId;
        private string _description;
        private string _color;
        private int _year;
        private decimal _rentalPrice;
        private bool _currentlyRented;

        public int CarId
        {
            get { return this._carId; }
            set { this._carId = value; }
        }

        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        public string Color
        {
            get { return this._color; }
            set { this._color = value; }
        }

        public int Year
        {
            get { return this._year; }
            set { this._year = value; }
        }

        public decimal RentalPrice
        {
            get { return this._rentalPrice; }
            set { this._rentalPrice = value; }
        }

        public bool CurrentlyRented
        {
            get { return this._currentlyRented; }
            set { this._currentlyRented = value; }
        }
    }
}
