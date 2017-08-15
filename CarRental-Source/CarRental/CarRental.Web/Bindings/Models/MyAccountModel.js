(function (cr) {
	let MyAccountModel = function (accountId, loginEmail, firstName, lastName, address, city, state, zipCode, creditCard, expDate) {

		var self = this;

		self.AccountId = ko.observable(accountId);   // not going to display, just a place to store this
		self.LoginEmail = ko.observable(loginEmail); // not going to display, just a place to store this
		self.FirstName = ko.observable(firstName).extend({
			required: { message: 'First name is required' }
		});
		self.LastName = ko.observable(lastName).extend({
			required: { message: 'Last name is required' }
		});
		self.Address = ko.observable(address).extend({
			required: { message: 'Address is required' }
		});
		self.City = ko.observable(city).extend({
			required: { message: 'City is required' }
		});
		self.State = ko.observable(state).extend({
			required: { message: 'State is required' }
		});
		self.ZipCode = ko.observable(zipCode).extend({
			required: { message: 'Zip code is required' },
			pattern: { message: 'Zip code is in an invalid format', params: /^\d{5}$/ }
		});
		self.CreditCard = ko.observable(creditCard).extend({
			required: { message: 'Credit card number is required' },
			pattern: { message: 'Credit card is invalid', params: /^\d{16}$/ }
		});

		let formattedExpDate = expDate.substr(0, 2) + '/' + expDate.substr(2, 2) + '/' + expDate.substr(4);
		self.ExpDate = ko.observable(formattedExpDate).extend({
			required: { message: 'Expiration date is required' },
			pattern: { message: 'Expiration date is an invalid format (must be MM/DD/YYYY)', params: CarRental.datePattern }
		});
	};
	cr.MyAccountModel = MyAccountModel;
}(window.CarRental));