/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.lichsus', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('lichsus', {
                url: "/lichsus",
                parent: 'base',
                templateUrl: "/app/components/lichsus/lichsuListView.html",
                controller: "lichsuListController"
            })
    }
})();