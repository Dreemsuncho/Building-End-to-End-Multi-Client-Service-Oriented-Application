(function (cr) {
   let ReserveCarViewModel = function () {

      let self = this;
      let initialState = "reserve";

      self.viewModelHelper = new CarRental.viewModelHelper();
      self.viewMode = ko.observable(initialState); // reserve, carlist, success
      self.reservationModel = ko.observable(new CarRental.ReserveCarModel());
      self.cars = ko.observableArray();
      self.reservationNumber = ko.observable();

      (function init() {
         self.viewMode.subscribe(function () {
            self.viewModelHelper.pushUrlState(self.viewMode(), null, null, 'customer/reserve');
            initialState = self.viewModelHelper.handleUrlState(initialState);
         });

         if (Modernizr.history) {
            window.onpopstate = function (arg) {
               if (arg.state != null) {
                  self.viewModelHelper.statePopped = true;
                  self.viewMode(arg.state.Code);
               }
            }
         }
      }());

      let pickupDate = null;
      let returnDate = null;

      self.availableCars = function (model) {
         let errors = ko.validation.group(model);
         let isValid = errors().length === 0;
         self.viewModelHelper.modelIsValid(isValid);

         if (isValid) {
            let unmappedModel = ko.toJS(model);
            self.viewModelHelper.apiGet('api/reservation/availablecars', unmappedModel,
               function (result) {
                  self.cars(result);
                  self.viewMode('carlist');
                  pickupDate = model.PickupDate();
                  returnDate = model.ReturnDate();
               });
         } else {

            self.viewModelHelper.modelErrors(errors());
         }
      }

      self.selectCar = function (car) {
         let model = { PickupDate: pickupDate, ReturnDate: returnDate, CarId: car.CarId };
         self.viewModelHelper.apiPost('api/reservation/reservecar', model,
            function (reservation) {
               self.viewMode('success');
               self.reservationNumber(reservation.ReservationId);
            });
      };

      self.reservationDates = function () {
         self.viewMode('reserve');
      }
   };
   cr.ReserveCarViewModel = ReserveCarViewModel;
}(window.CarRental));
