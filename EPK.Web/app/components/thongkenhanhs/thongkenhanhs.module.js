/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.thongkenhanhs', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('thongkenhanhs', {
                url: "/thongkenhanhs",
                parent: 'base',
                templateUrl: "/app/components/thongkenhanhs/thongkenhanhListView.html",
                controller: "thongkenhanhListController"
            });
    }
})();