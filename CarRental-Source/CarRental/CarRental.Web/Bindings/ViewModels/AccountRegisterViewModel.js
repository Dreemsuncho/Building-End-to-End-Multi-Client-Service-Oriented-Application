(function (cr) {
	let AccountRegisterViewModel = function () {

		let self = this;
		let initialState = 'step1';

		self.viewModelHelper = new CarRental.viewModelHelper();
		self.conditionalSteps = function (params) {
			switch (self.viewMode()) {
				case 'step1':
					return params[0]
					break;
				case 'step2':
					return params[1]
					break;
				case 'step3':
					return params[2]
					break
				case 'confirm':
					return params[3]
			}
		}
		self.step1 = ko.observable();
		self.step2 = ko.observable();
		self.step3 = ko.observable();
		self.viewMode = ko.observable(); // step1, step2, step3, confirm, welcome

		(function initialize() {
			let model = CarRental.AccountRegisterModel;
			self.step1(new model.Step1());
			self.step2(new model.Step2());
			self.step3(new model.Step3());

			self.viewMode(initialState);

			self.viewMode.subscribe(function () {
				self.viewModelHelper.pushUrlState(self.viewMode(), null, null, 'account/register');
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


		self.prevStep = function (model) {
			self.viewModelHelper.modelIsValid(true);

			self.conditionalSteps([
				null,
				() => self.viewMode('step1'),
				() => self.viewMode('step2'),
				() => self.viewMode('step3')
			])();
		};


		self.nextStep = function (model) {
			let errors = self.conditionalSteps([
				ko.validation.group(self.step1),
				ko.validation.group(self.step2),
				ko.validation.group(self.step3)
			]);

			let modelIsValid = errors().length == 0;
			self.viewModelHelper.modelIsValid(modelIsValid);

			if (modelIsValid) {
				let unmappedModel = ko.toJS(model);
				let viewMode = self.viewMode();

				self.viewModelHelper.apiPost('api/account/register/' + viewMode, unmappedModel,
					function () {
						self.conditionalSteps([
							() => self.viewMode('step2'),
							() => self.viewMode('step3'),
							() => self.viewMode('confirm')
						])();
					});
			}
			else {
				self.viewModelHelper.modelErrors(errors());
			}
		};


		self.createAccount = function () {
			let unmappedModel;

			unmappedModel = $.extend(unmappedModel, ko.toJS(self.step1))
			unmappedModel = $.extend(unmappedModel, ko.toJS(self.step2))
			unmappedModel = $.extend(unmappedModel, ko.toJS(self.step3))

			self.viewModelHelper.apiPost('api/account/register', unmappedModel,
				function () {
					self.viewMode('welcome');
				});
		};
	};
	cr.AccountRegisterViewModel = AccountRegisterViewModel;
}(window.CarRental));