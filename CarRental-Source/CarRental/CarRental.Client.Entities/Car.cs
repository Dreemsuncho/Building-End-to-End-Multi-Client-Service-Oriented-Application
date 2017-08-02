using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
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
            set
            {
                if (this._carId != value)
                {
                    this._carId = value;
                    base.OnPropertyChanged(() => this.CarId);
                }
            }
        }

        public string Description
        {
            get { return this._description; }
            set
            {
                if (this._description != value)
                {
                    this._description = value;
                    base.OnPropertyChanged(() => this.Description);
                }
            }
        }

        public string Color
        {
            get { return this._color; }
            set
            {
                if (this._color != value)
                {
                    this._color = value;
                    base.OnPropertyChanged(() => this.Color);
                }
            }
        }

        public int Year
        {
            get { return this._year; }
            set
            {
                if (this._year != value)
                {
                    this._year = value;
                    base.OnPropertyChanged(() => this.Year);
                }
            }
        }

        public decimal RentalPrice
        {
            get { return this._rentalPrice; }
            set
            {
                if (this._rentalPrice != value)
                {
                    this._rentalPrice = value;
                    base.OnPropertyChanged(() => this.RentalPrice);
                }
            }
        }

        public bool CurrentlyRented
        {
            get { return this._currentlyRented; }
            set
            {
                if (this._currentlyRented != value)
                {
                    this._currentlyRented = value;
                    base.OnPropertyChanged(() => this.CurrentlyRented);
                }
            }
        }


        #region Validation 

        protected override IValidator GetValidator()
        {
            return new CarValidator();
        }

        private class CarValidator : AbstractValidator<Car>
        {
            public CarValidator()
            {
                base.RuleFor(car => car.Description).NotEmpty();
                base.RuleFor(car => car.Color).NotEmpty();
                base.RuleFor(car => car.RentalPrice).GreaterThan(0);
                base.RuleFor(car => car.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year + 1);
            }
        }

        #endregion
    }
}
