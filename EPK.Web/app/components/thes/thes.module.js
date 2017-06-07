/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.thes', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('thes', {
                url: "/thes",
                parent: 'base',
                templateUrl: "/app/components/thes/theListView.html",
                controller: "theListController"
            });
    }
})();