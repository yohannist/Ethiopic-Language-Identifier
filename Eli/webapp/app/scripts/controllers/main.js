'use strict';

/**
 * @ngdoc function
 * @name webappApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the webappApp
 */

(function () {

  angular.module('eliApp')
    .controller('MainCtrl', controller);

  controller.$inject = ['$http', 'languageIdentifier'];

  function controller($http, languageIdentifier) {

    var vm = this;

    vm.input = "";
    vm.loadingSignatures = true;
    vm.showIdentifyingLanguage = false;
    vm.errorBanner = false;
    vm.error = '';

    vm.identify = identify;

    var signatureFileNames = ["amharic", "geeze", "tigrigna"];
    var signaturePath = './json';

    var signatures = [];

    angular.forEach(signatureFileNames, function (val, index) {
      var req = {
        method: 'GET',
        url: signaturePath + '/' + val + '.json'
      };

      $http(req).then(function (data) {
        signatures.push(data.data);
        vm.loadingSignatures = index < signatureFileNames.length - 1;
      }, function (error) {
        vm.error = error;
        vm.errorBanner = true;
      });

    });


    function identify() {
      var result = languageIdentifier.identify(signatures, vm.input);
      vm.scores = result;
      console.info(result);
      // debugger;
    }

  }
})();

