(function (app) {
    app.controller('lichsuvethangListController', lichsuvethangListController);

    lichsuvethangListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function lichsuvethangListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;
        $scope.loai = 'xedangky';
        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

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
                params: {
                    page: page,
                    pageSize: 10,
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    loai: $scope.loai
                }
            }
            apiService.get('api/vethang/lichsuvethang',
                config, dataLoadCompleted, dataLoadFailed);
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
    }
})(angular.module('epk.vethangs'));