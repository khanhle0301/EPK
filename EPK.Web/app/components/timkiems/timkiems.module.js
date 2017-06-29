/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.timkiems', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('timkiems', {
                url: "/timkiems",
                parent: 'base',
                templateUrl: "/app/components/timkiems/timkiemListView.html",
                controller: "timkiemListController"
            })
    }
})();