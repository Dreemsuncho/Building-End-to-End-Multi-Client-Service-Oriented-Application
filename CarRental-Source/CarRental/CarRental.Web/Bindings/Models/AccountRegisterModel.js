(function (cr) {
	cr.AccountRegisterModel = {};
}(window.CarRental));

(function (cr) {
	let Step1 = function () {
		let self = this;

		self.FirstName = ko.observable("").extend({
			required: { message: "First name is required" }
		});
		self.LastName = ko.observable("").extend({
			required: { message: "Last name is required" }
		});
		self.Address = ko.observable("").extend({
			required: { message: "Address is required" }
		});
		self.City = ko.observable("").extend({
			required: { message: "City is required" }
		});
		self.State = ko.observable("").extend({
			required: { message: "State is required" }
		});
		self.ZipCode = ko.observable("").extend({
			required: { message: "Zip code is required" }
		});
	};
	cr.AccountRegisterModel.Step1 = Step1;
}(window.CarRental));

(function (cr) {
	let Step2 = function () {
		let self = this;

		self.LoginEmail = ko.observable("").extend({
			required: { message: "Login email is required" },
			email: { message: "Login is not a valid email" }
		});
		self.Password = ko.observable("").extend({
			required: { message: "Password is required" },
			minLength: { message: "Password must be at least 6 characters", params: 6 }
		});
		self.PasswordConfirm = ko.observable("").extend({
			validation: { validator: CarRental.mustEqual, message: "Password do not match", params: self.Password }
		});
	};
	cr.AccountRegisterModel.Step2 = Step2;
}(window.CarRental));

(function (cr) {
	let Step3 = function () {
		let self = this;

		self.CreditCard = ko.observable("").extend({
			required: { message: "Credit card number is required" }
		});
		self.ExpDate = ko.observable("").extend({
			required: { message: "Expiration date is required" }
		});
	};
	cr.AccountRegisterModel.Step3 = Step3;
}(window.CarRental));