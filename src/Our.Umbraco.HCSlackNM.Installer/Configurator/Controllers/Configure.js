angular.module("umbraco").controller("HCSlackNM.Loader", function ($scope, $http, $log) {

    var getDataUrl = "/Umbraco/backoffice/HealthCheckSlackNotificationMethod/Installer/GetParameters";
    var postDataUrl = "/Umbraco/backoffice/HealthCheckSlackNotificationMethod/Installer/PostParameters";

    $scope.saved = false;

    // Ajax request to controller for data-
    $http.get(getDataUrl).then(function (response) {
      $scope.parameters = response.data;
    });

    $scope.submitForm = function (e) {
      e.preventDefault();

      $http.post(postDataUrl, $scope.parameters)
        .then(function (response) {

          var status;
          status = response.data;

          $scope.status = status;

          if (status !== "ConnectionError") {
            $scope.saved = true;
          }

        });
    };

    $scope.capitalizeFirstLetter = function (string) {
      return string.charAt(0).toUpperCase() + string.slice(1);
    };

    $scope.getInputType = function (param) {
      return param.toUpperCase() === "USEDEFAULTROUTE" || param.toUpperCase() === "USEPRIVATECONTAINER" ? "checkbox" : "text";
    };
});
