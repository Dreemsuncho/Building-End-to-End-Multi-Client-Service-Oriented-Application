
(function (cr) {
   let AccountRegisterViewModel = function () {

      let self = this;
      let initialState = 'step1';

      self.viewModelHelper = new CarRental.viewModelHelper();
      let model = CarRental.AccountRegisterModel;
      self.step1 = ko.observable(new model.Step1());
      self.step2 = ko.observable(new model.Step2());
      self.step3 = ko.observable(new model.Step3());
      self.viewMode = ko.observable(initialState); // step1, step2, step3, confirm, welcome

      (function init() {
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

         switch (self.viewMode()) {
         case 'step2':
            self.viewMode('step1');
            break;
         case 'step3':
            self.viewMode('step2');
            break;
         case 'confirm':
            self.viewMode('step3');
         }
      };

      self.nextStep = function (model) {
         let errors = [];

         switch (self.viewMode()) {
         case 'step1':
            errors = ko.validation.group(self.step1);
            break;
         case 'step2':
            errors = ko.validation.group(self.step2);
            break;
         case 'step3':
            errors = ko.validation.group(self.step3)
         }

         let modelIsValid = errors().length == 0;
         self.viewModelHelper.modelIsValid(modelIsValid);

         if (modelIsValid) {
            let unmappedModel = ko.toJS(model);
            let viewMode = self.viewMode();

            self.viewModelHelper.apiPost('api/account/register/' + viewMode, unmappedModel,
               function () {
                  switch (self.viewMode()) {
                  case 'step1':
                     self.viewMode('step2');
                     break;
                  case 'step2':
                     self.viewMode('step3');
                     break;
                  case 'step3':
                     self.viewMode('confirm');
                  }
               });
         } else {
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
               window.location.href = CarRental.rootPath;
            });
      };
   };
   cr.AccountRegisterViewModel = AccountRegisterViewModel;
}(window.CarRental));
