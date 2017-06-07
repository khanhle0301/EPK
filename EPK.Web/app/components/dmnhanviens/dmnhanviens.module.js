/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.dmnhanviens', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('dmnhanviens', {
                url: "/dmnhanviens",
                parent: 'base',
                templateUrl: "/app/components/dmnhanviens/dmnhanvienListView.html",
                controller: "dmnhanvienListController"
            })
            .state('dmnhanvien_add', {
                url: "/dmnhanvien_add",
                parent: 'base',
                templateUrl: "/app/components/dmnhanviens/dmnhanvienAddView.html",
                controller: "dmnhanvienAddController"
            })
            .state('dmnhanvien_edit', {
                url: "/dmnhanvien_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/dmnhanviens/dmnhanvienEditView.html",
                controller: "dmnhanvienEditController"
            });
    }
})();