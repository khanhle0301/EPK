(function (app) {
    app.controller('lichsuListController', lichsuListController);

    lichsuListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function lichsuListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

        $scope.search = search;

        $scope.deletels = deletels;

        function deletels() {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {

                apiService.del('api/lichsu/delete',
                    null,
                    function (result) {
                        notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                        getThongKes();
                    },
                    function () {
                        notificationService.displayError('Xóa không thành công');
                    });
            });
        }

        $scope.exportExcel = exportExcel;

        function exportExcel() {
            var config = {
                params: {
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                }
            }
            apiService.get('/api/lichsu/ExportXls', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }

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
                    ketThuc: convert($scope.ketThuc)
                }
            }
            apiService.get('api/lichsu/getall',
                config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;

            if (result.data.TotalCount > 0) {
                $('#btnExport').removeAttr('disabled');
                $('#btnDelete').removeAttr('disabled');
            }
            else {
                $('#btnExport').attr('disabled', 'disabled');
                $('#btnDelete').attr('disabled', 'disabled');
            }

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
})(angular.module('epk.lichsus'));