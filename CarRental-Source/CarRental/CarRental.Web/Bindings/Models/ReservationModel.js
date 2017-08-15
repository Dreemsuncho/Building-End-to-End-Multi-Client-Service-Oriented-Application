(function (cr) {
   let ReservationModel = function (reservationId, car, rentalDate, returnDate) {

      let self = this;

      self.ReservationId = ko.observable(reservationId);
      self.Car = ko.observable(car);
      self.RentalDate = ko.observable(rentalDate);
      self.ReturnDate = ko.observable(returnDate);

      self.CancelRequest = ko.observable(false);
   };
   cr.ReservationModel = ReservationModel;
}(window.CarRental));
