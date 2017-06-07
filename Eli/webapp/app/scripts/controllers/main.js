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

  controller.$inject = ['$http', 'languageIdentifier', 'signatureLoaderService'];

  function controller($http, languageIdentifier, signatureLoaderService) {

    var vm = this;

    vm.input = "";
    vm.loadingSignatures = true;
    vm.showIdentifyingLanguage = false;
    vm.errorBanner = false;
    vm.error = '';

    vm.identify = identify;
    vm.getRandomColor = getRandomColor;

    var signatureFileNames = ["am", "geeze", "tg"];
    var signaturePath = './json';

    var signatures = [];

    signatureLoaderService.getSignaturesFromZip('./signature/sig')
      .then(function (sig) {

        signatures = sig;
        vm.loadingSignatures = false
      }, function (err) {
        vm.errorBanner = true;
        vm.error = err;
      });

    function identify() {

      vm.scores = vm.Language = null;

      var result = languageIdentifier.identify(signatures, vm.input);
      if (!result) {
        return;
      }

      vm.scores = result.Scores;
      vm.Language = result.MostLikelyLanguage;

      vm.scores.forEach(function (val) { val.Color = getRandomColor() });
    }

    function getRandomColor() {
      var color = 'ffffff';
      while (color.startsWith('ff') || color == '000000')
        color = ('00000' + (Math.random() * (1 << 24) | 0).toString(16)).slice(-6);
      return "#" + color;
    }
  }

  var colors = ["#FF113D", "#208075", "#10ADED", "#60061E", "#0D355A", "#661177", "#130", "#346135", "#ACC02D"];

})();

