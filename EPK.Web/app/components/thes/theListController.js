(function (app) {
    app.controller('theListController', theListController);

    theListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function theListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.getThes = getThes;

        function getThes(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                param: {
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('api/the/getall?page=' + config.param.page + '&pageSize=' + config.param.pageSize,
                null, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.getThes();
    }
})(angular.module('epk.thes'));