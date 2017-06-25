(function (app) {
    app.controller('thongkenhanhtatcaListController', thongkenhanhtatcaListController);

    thongkenhanhtatcaListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function thongkenhanhtatcaListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date(2015, 01, 01);
        $scope.ketThuc = new Date(2020, 01, 01);

        $scope.sort = 'ngay';

        $scope.xethang = true;
        $scope.vanglai = true;

        $scope.search = search;

        $scope.getThongKes = getThongKes;

        function convert(str) {
            var date = new Date(str),
                mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                day = ("0" + date.getDate()).slice(-2);
            return [date.getFullYear(), mnth, day].join("-");
        }


        function getThongKes(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                param: {
                    page: page,
                    pageSize: 10,
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    vanglai: $scope.vanglai,
                    thang: $scope.xethang,
                    sort: $scope.sort
                }
            }
            apiService.get('api/thongkenhanh/getall?page=' + config.param.page + '&pageSize=' + config.param.pageSize + '&batDau='
                + config.param.batDau + '&ketThuc=' + config.param.ketThuc + '&vanglai=' +
                config.param.vanglai + '&thang=' + config.param.thang + '&sort=' + config.param.sort,
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

        //$scope.getThongKes();
    }
})(angular.module('epk.thongkenhanhs'));