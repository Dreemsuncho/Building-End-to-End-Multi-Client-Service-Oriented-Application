﻿window.CarRental = {};

(function (cr) {
	cr.initialId = {};
}(window.CarRental));

(function (cr) {
	cr.initialState = {};
}(window.CarRental));

(function (cr) {
	cr.rootPath = {};
}(window.CarRental));

(function (cr) {
	let mustEqual = function (val, other) {
		return val == other;
	};
	cr.mustEqual = mustEqual;
}(window.CarRental));

(function (cr) {
	let datePattern = /^(0[1-9]|1[0-2])\/(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/[0-9]{4}$/;
	cr.datePattern = datePattern;
}(window.CarRental));

(function (cr) {
	let viewModelHelper = function () {
		let self = this;

		self.modelIsValid = ko.observable(true);
		self.modelErrors = ko.observableArray();
		self.isLoading = ko.observable(false);

		self.statePopped = false;
		self.stateInfo = {};

		self.apiGet = function (url, data, success, failure, always) {
			self.isLoading(true);
			self.modelIsValid(true);

			$.get(CarRental.rootPath + url, data)
				.done(success)
				.fail(function (result) {
					failure != null
						? failure(result)
						: defaultFail(result);
				})
				.always(function () {
					always != null
						? always()
						: self.isLoading(false);
				});
		};

		self.apiPost = function (url, data, success, failure, always) {
			self.isLoading(true);
			self.modelIsValid(true);

			$.post(CarRental.rootPath + url, data)
				.done(success)
				.fail(function (result) {
					failure != null
						? failure(result)
						: defaultFail(result)
				})
				.always(function () {
					always != null
						? always()
						: self.isLoading(false);
				});
		}

		function defaultFail(result) {
			if (result.status != 400)
				self.modelErrors([result.status + ':' + result.statusText + ':' + result.responseText]);
			else
				self.modelErrors(JSON.parse(result.responseText));
			self.modelIsValid(false);
		}

		self.pushUrlState = function (code, title, id, url) {
			self.stateInfo = { State: { Code: code, Id: id }, Title: title, Url: CarRental.rootPath + url };
		}

		self.handleUrlState = function (initialState) {
			if (!self.statePopped) {
				if (initialState != '') {
					history.replaceState(self.stateInfo.State, self.stateInfo.Title, self.stateInfo.Url);
					initialState = '';
				}
				else {
					history.pushState(self.stateInfo.State, self.stateInfo.Title, self.stateInfo.Url);
				}
			}
			else {
				self.statePopped = false;
			}

			return initialState;
		}
	};
	cr.viewModelHelper = viewModelHelper;
}(window.CarRental));


ko.bindingHandlers.datepicker = {
	init: function (element, valueAccessor, allBindingsAccessor) {
		//initialize datepicker with some optional options
		var options = allBindingsAccessor().datepickerOptions || {};
		$(element).datepicker(options);

		//handle the field changing
		ko.utils.registerEventHandler(element, "change", function () {
			var observable = valueAccessor();
			var newDate = $(element).datepicker("getDate");
			// newDate format is 2013-01-11T06:11:00.000Z
			observable(moment(newDate).format('MM/DD/YYYY'));
		});

		//handle disposal (if KO removes by the template binding)
		ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
			$(element).datepicker("destroy");
		});

	},
	// get the value from the viewmodel and format it for display
	update: function (element, valueAccessor) {
		var value = ko.utils.unwrapObservable(valueAccessor());
		var date = moment(value);
		current = $(element).datepicker("getDate");

		if (value != null) {
			if (value - current !== 0) {
				var date = moment(value);
				$(element).val(date.format("L"));
			}
		}
	}
};

ko.bindingHandlers.loadingWhen = {
	// any ViewModel using this extension needs a property called isLoading
	// the div tag to contain the loaded content uses a data-bind="loadingWhen: isLoading"
	init: function (element) {
		var $element = $(element),
			currentPosition = $element.css("position")
		$loader = $("<div>").addClass("loading-loader").hide();

		//add the loader
		$element.append($loader);

		//make sure that we can absolutely position the loader against the original element
		if (currentPosition == "auto" || currentPosition == "static")
			$element.css("position", "relative");

		//center the loader
		$loader.css({
			position: "absolute",
			top: "50%",
			left: "50%",
			"margin-left": -($loader.width() / 2) + "px",
			"margin-top": -($loader.height() / 2) + "px"
		});
	},
	update: function (element, valueAccessor) {
		var isLoading = ko.utils.unwrapObservable(valueAccessor()),
			$element = $(element),
			$childrenToHide = $element.children(":not(div.loading-loader)"),
			$loader = $element.find("div.loading-loader");

		if (isLoading) {
			$childrenToHide.css("visibility", "hidden").attr("disabled", "disabled");
			$loader.show();
		}
		else {
			$loader.fadeOut("fast");
			$childrenToHide.css("visibility", "visible").removeAttr("disabled");
		}
	}
};

ko.extenders.numeric = function (target, precision) {
	//create a writeable computed observable to intercept writes to our observable
	var result = ko.computed({
		read: target,  //always return the original observables value
		write: function (newValue) {
			var current = target(),
				roundingMultiplier = Math.pow(10, precision),
				newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
				valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

			//only write if it changed
			if (valueToWrite !== current) {
				target(valueToWrite);
			} else {
				//if the rounded value is the same, but a different value was written, force a notification for the current field
				if (newValue !== current) {
					target.notifySubscribers(valueToWrite);
				}
			}
		}
	});

	//initialize with current value to make sure it is rounded appropriately
	result(target());

	//return the new computed observable
	return result;
};

ko.bindingHandlers.date = {
	init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
		// TODO: this should be able to support foreign date formats
		var jsonDate = valueAccessor(); // 2013-02-19T00:00:00
		var ret = $.datepicker.formatDate('mm-dd-yy', new Date(jsonDate()));
		element.innerHTML = ret;
	},
	update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
	}
};

// Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
// Could be stored in a separate utility library
ko.bindingHandlers.fadeVisible = {
	init: function (element, valueAccessor) {
		// Initially set the element to be instantly visible/hidden depending on the value
		var value = valueAccessor();
		$(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
	},
	update: function (element, valueAccessor) {
		// Whenever the value subsequently changes, slowly fade the element in or out
		var value = valueAccessor();
		ko.unwrap(value) ? $(element).fadeIn(500) : $(element).fadeOut(0);
	}
};
