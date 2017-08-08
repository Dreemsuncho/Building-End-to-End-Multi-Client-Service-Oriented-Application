(function (cr) {
	let AccountChangePasswordModel = function () {
		let self = this;

		self.LoginEmail = ko.observable("");

		self.OldPassword = ko.observable("").extend({
			required: { message: 'Old password is required' },
			minLength: { message: 'Old password must be at least 6 characters', params: 6 }
		});

		self.NewPassword = ko.observable("").extend({
			required: { message: 'Old password is required' },
			minLength: { message: 'Old password must be at least 6 characters', params: 6 }
		});
	};
	cr.AccountChangePasswordModel = AccountChangePasswordModel;
}(window.CarRental))