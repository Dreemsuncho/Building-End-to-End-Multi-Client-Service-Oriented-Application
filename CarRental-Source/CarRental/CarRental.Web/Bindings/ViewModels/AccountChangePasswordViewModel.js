(function (cr) {
	let AccountChangePasswordViewModel = function (loginEmail) {
		let self = this;

		self.viewModelHelper = new CarRental.viewModelHelper();
		self.changePasswordModel = new CarRental.AccountChangePasswordModel();
		self.viewMode = ko.observable('changepw'); // changepw, success

		self.changePassword = function (model) {
			let errors = ko.validation.group(model);
			let isValid = errors().length == 0;
			self.viewModelHelper.modelIsValid(isValid);

			if (isValid) {
				let unmappedModel = ko.toJS(model);
				unmappedModel.LoginEmail = loginEmail;

				self.viewModelHelper.apiPost('api/account/changepw', unmappedModel,
					function (result) {
						self.viewMode('success');
					});
			}
			else {
				self.viewModelHelper.modelErrors(errors());
			}
		}
	};
	cr.AccountChangePasswordViewModel = AccountChangePasswordViewModel;
}(window.CarRental))