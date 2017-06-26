/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.thongkegiahans', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('thongkegiahans', {
                url: "/thongkegiahans",
                parent: 'base',
                templateUrl: "/app/components/thongkegiahans/thongkegiahanListView.html",
                controller: "thongkegiahanListController"
            })
    }
})();