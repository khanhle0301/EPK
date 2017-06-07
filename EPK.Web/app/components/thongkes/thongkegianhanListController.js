(function (app) {
    app.controller('thongkegiahanListController', thongkegiahanListController);

    thongkegiahanListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function thongkegiahanListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.fromDate = '01/01/2015';
        $scope.toDate = '01/01/2020';

        $scope.search = search;

        $scope.getThongKes = getThongKes;

        function getThongKes(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                param: {
                    page: page,
                    pageSize: 10,
                    fromDate: $scope.fromDate,
                    toDate: $scope.toDate
                }
            }
            apiService.get('api/thongke/thongkegiahan?page=' + config.param.page + '&pageSize=' + config.param.pageSize + '&fromDate=' + config.param.fromDate + '&toDate=' + config.param.toDate,
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

        function search() {
            getThongKes();
        }

        $scope.getThongKes();
    }
})(angular.module('epk.thongkes'));