/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.thongkechitiets', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('thongkechitiets', {
                url: "/thongkechitiets",
                parent: 'base',
                templateUrl: "/app/components/thongkechitiets/thongkechitietListView.html",
                controller: "thongkechitietListController"
            });
    }
})();