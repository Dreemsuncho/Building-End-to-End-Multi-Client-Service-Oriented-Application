(function (cr) {
	let ReserveCarModel = function () {

		let self = this;

		self.PickupDate = ko.observable("").extend({
			required: { message: 'Pickup date is required' },
			pattern: { message: 'Pickup date is an invalid format (must be MM/DD/YYYY)', params: CarRental.datePattern }
		});
		self.ReturnDate = ko.observable("").extend({
			required: { message: 'Return date is required' },
			pattern: { message: 'Return date is an invalid format (must be MM/DD/YYYY)', params: CarRental.datePattern }
		});
	};
	cr.ReserveCarModel = ReserveCarModel;
}(window.CarRental));