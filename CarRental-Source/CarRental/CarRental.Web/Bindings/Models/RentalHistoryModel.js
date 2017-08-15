(function (cr) {
   let RentalHistoryModel = function (rentalId, carId, dateRented, dateDue, dateReturned) {

      let self = this;

      self.RentalId = ko.observable(rentalId);
      self.Car = ko.observable(carId);
      self.DateRented =ko.observable(dateRented);
      self.DateDue = ko.observable(dateDue);
      self.DateReturned = ko.observable(dateReturned);
   };
   cr.RentalHistoryModel = RentalHistoryModel;
}(window.CarRental));
