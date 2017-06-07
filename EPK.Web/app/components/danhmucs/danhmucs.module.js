/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.danhmucs', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('loaives', {
                url: "/loaives",
                parent: 'base',
                templateUrl: "/app/components/danhmucs/loaiveListView.html",
                controller: "loaiveListController"
            });
    }
})();