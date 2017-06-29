/// <reference path="/Assets/libs/angular/angular.js" />

(function () {
    angular.module('epk.vethangs', ['epk.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('lichsuvethangs', {
                url: "/lichsuvethangs",
                parent: 'base',
                templateUrl: "/app/components/vethangs/lichsuvethangListView.html",
                controller: "lichsuvethangListController"
            })
        .state('vethangs', {
            url: "/vethangs",
            parent: 'base',
            templateUrl: "/app/components/vethangs/vethangListView.html",
            controller: "vethangListController"
        });
    }
})();