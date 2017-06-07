/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.nhanviens', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('nhanviens', {
                url: "/nhanviens",
                parent: 'base',
                templateUrl: "/app/components/nhanviens/nhanvienListView.html",
                controller: "nhanvienListController"
            })
            .state('nhanvien_add', {
                url: "/nhanvien_add",
                parent: 'base',
                templateUrl: "/app/components/nhanviens/nhanvienAddView.html",
                controller: "nhanvienAddController"
            })
            .state('nhanvien_edit', {
                url: "/nhanvien_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/nhanviens/nhanvienEditView.html",
                controller: "nhanvienEditController"
            });
    }
})();