/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.thongkenhanhs', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('thongkenhanh_tatcas', {
                url: "/thongkenhanh_tatcas",
                parent: 'base',
                templateUrl: "/app/components/thongkenhanhs/thongkenhanhtatcaListView.html",
                controller: "thongkenhanhtatcaListController"
            })
            .state('thongkenhanh_maytinhs', {
                url: "/thongkenhanh_maytinhs",
                parent: 'base',
                templateUrl: "/app/components/thongkenhanhs/thongkenhanhmaytinhListView.html",
                controller: "thongkenhanhmaytinhListController"
            })
            .state('thongkenhanh_nhanviens', {
                url: "/thongkenhanh_nhanviens",
                parent: 'base',
                templateUrl: "/app/components/thongkenhanhs/thongkenhanhnhanvienListView.html",
                controller: "thongkenhanhnhanvienListController"
            })

    }
})();