(function (app) {
    app.controller('thongkegiahanListController', thongkegiahanListController);

    thongkegiahanListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function thongkegiahanListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

        $scope.search = search;
        $scope.exportExcel = exportExcel;

        function exportExcel() {
            var config = {
                params: {
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                }
            }
            apiService.get('/api/thongkegiahan/ExportXls', config, function (response) {
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
            apiService.get('api/thongkegiahan/getall',
                config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;

            if (result.data.TotalCount > 0) {
                $('#btnExport').removeAttr('disabled');
            }
            else {
                $('#btnExport').attr('disabled', 'disabled');
            }

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;

            $scope.soLuotXe = result.data.SumThongKeGiaHan.SoLuotXe;
            $scope.tongtien = result.data.SumThongKeGiaHan.TongIien;
            $scope.listLoaiGiaVe = result.data.SumThongKeGiaHan.ListLoaiGiaVe;

            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function search() {
            getThongKes();
        }
    }
})(angular.module('epk.thongkegiahans'));