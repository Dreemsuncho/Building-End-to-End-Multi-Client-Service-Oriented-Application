﻿using CarRental.Business.Entities;

namespace CarRental.Data.Contracts.DTOs
{
    public class CustomerRentalInfo
    {
        public Account Customer { get; set; }
        public Car Car { get; set; }
        public Rental Rental { get; set; }
    }
}
