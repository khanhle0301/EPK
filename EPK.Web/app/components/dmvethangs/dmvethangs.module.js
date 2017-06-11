/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.dmvethangs', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('dmvethangs', {
                url: "/dmvethangs",
                parent: 'base',
                templateUrl: "/app/components/dmvethangs/dmvethangListView.html",
                controller: "dmvethangListController"
            })
            .state('dmvethang_add', {
                url: "/dmvethang_add",
                parent: 'base',
                templateUrl: "/app/components/dmvethangs/dmvethangAddView.html",
                controller: "dmvethangAddController"
            })
            .state('dmvethang_edit', {
                url: "/dmvethang_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/dmvethangs/dmvethangEditView.html",
                controller: "dmvethangEditController"
            });
    }
})();