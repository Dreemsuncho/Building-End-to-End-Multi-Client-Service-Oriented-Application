(function (cr) {
   let CurrentReservationViewModel = function () {

      let self = this;

      self.viewModelHelper = new CarRental.viewModelHelper();
      self.reservations = ko.observableArray();

      self.initialize = function () {
         self.loadReservations();
      };

      self.loadReservations = function () {
         self.viewModelHelper.apiGet('api/reservation/getopen', null,
            function (result) {
               self.reservations.removeAll();

               result.forEach(r => {
                  let reservationModel = new CarRental.ReservationModel(
                     r.ReservationId, r.Car, r.RentalDate, r.ReturnDate);

                  self.reservations.push(reservationModel);
               });
            });
      };

      self.requestCancelReservation = function (reservation) {
         reservation.CancelRequest(true);
      };

      self.undoCancelRequest = function (reservation) {
         reservation.CancelRequest(false);
      }

      self.cancelReservation = function (reservation) {
         self.viewModelHelper.apiPost('api/reservation/cancel', { '': reservation.ReservationId() },
            function () {
               self.reservations.remove(reservation);
            });
      };

      self.initialize();
   };
   cr.CurrentReservationViewModel = CurrentReservationViewModel;
}(window.CarRental));
