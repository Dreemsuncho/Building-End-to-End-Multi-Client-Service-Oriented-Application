(function (cr) {
	let AccountLoginViewModel = function (returnUrl) {
		let self = this;

		self.accountModel = new CarRental.AccountLoginModel();
		self.viewModelHelper = new CarRental.viewModelHelper();

		self.login = function (model) {
			let errors = ko.validation.group(model);
			let isValid = errors().length == 0;
			self.viewModelHelper.modelIsValid(isValid);

			if (isValid) {
				let unmappedModel = ko.toJS(model);

				self.viewModelHelper.apiPost('api/account/login', unmappedModel,
					function () {
						if (returnUrl != '' && returnUrl.length > 1)
							window.location.href = CarRental.rootPath + returnUrl;
						else
							window.location.href = CarRental.rootPath;
					});
			}
			else {
				self.viewModelHelper.modelErrors(errors());
			}
		}
	};
	cr.AccountLoginViewModel = AccountLoginViewModel;
}(window.CarRental))