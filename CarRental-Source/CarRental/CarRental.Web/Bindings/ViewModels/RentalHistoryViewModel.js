(function (cr) {
   let RentalHistoryViewModel = function () {

      let self = this;

      self.viewModelHelper = new CarRental.viewModelHelper();
      self.rentals = ko.observableArray();

      self.viewModelHelper.apiGet('api/reservation/history', null,
         function (result) {
            let errors = ko.validation.group(result);
            let isValid = errors().length === 0;
            self.viewModelHelper.modelIsValid(isValid);

            result.forEach(r => {
               let rentalModel = new CarRental.RentalHistoryModel(r.RentalId, r.CarId, r.DateRented, r.DateDue, r.DateReturned);
               self.rentals.push(rentalModel);
            });
         });
   };
   cr.RentalHistoryViewModel = RentalHistoryViewModel;
}(window.CarRental));
