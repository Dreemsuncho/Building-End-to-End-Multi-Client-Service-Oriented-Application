(function (cr) {
	let MyAccountViewModel = function () {

		let self = this;

		self.viewModelHelper = new CarRental.viewModelHelper();
		self.viewMode = ko.observable(""); // account, success
		self.myAccountModel = ko.observable();

		(function init() {
			self.viewModelHelper.apiGet('api/customer/account', null,
				function (result) {
					self.myAccountModel(new CarRental.MyAccountModel(
						result.AccountId, result.LoginEmail, result.FirstName,
						result.LastName, result.Address, result.City, result.State,
						result.ZipCode, result.CreditCard, result.ExpDate)
					);
					self.viewMode('account');
				});
		}());


		self.save = function (model) {	
			let errors = ko.validation.group(model);
			let isValid = errors().length === 0;
			self.viewModelHelper.modelIsValid(isValid);

			if (isValid) {
				let unmappedModel = ko.toJS(model);

				self.viewModelHelper.apiPost('api/customer/account', unmappedModel,
					function () {
						self.viewMode('success');
					});
			}
			else {
				self.viewModelHelper.modelErrors(errors());
			}
		};
	};
	cr.MyAccountViewModel = MyAccountViewModel;
}(window.CarRental));