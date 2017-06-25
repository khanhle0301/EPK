(function (app) {
    app.controller('thongkechitietListController', thongkechitietListController);

    thongkechitietListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function thongkechitietListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];

        $scope.listXe = [];

        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

        $scope.sendmail = true;

        $scope.trongbai = 'trongbai';

        $scope.xethang = true;
        $scope.vanglai = true;

        $scope.type = 'chitiet';

        $scope.search = search;
        $scope.exportExcel = exportExcel;

        function exportExcel() {
            var config = {
                params: {
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    type: $scope.type,
                    trongbai: $scope.trongbai
                }
            }
            apiService.get('/api/thongkechitiet/ExportXls', config, function (response) {
                if (response.status = 200) {
                    $("#myModal").removeClass("in");
                    $(".modal-backdrop").remove();
                    $("#myModal").hide();
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
                    ketThuc: convert($scope.ketThuc),
                    vanglai: $scope.vanglai,
                    thang: $scope.xethang,
                    sendmail: $scope.sendmail,
                    trongbai: $scope.trongbai
                }
            }
            apiService.get('api/thongkechitiet/getall', config, dataLoadCompleted, dataLoadFailed);
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
            $scope.soLuotXe = result.data.SoLuotXe;
            $scope.xeVangLai = result.data.XeVangLai;
            $scope.xeThang = result.data.XeThang;
            $scope.listXeVangLai = result.data.ListXeVangLai;

            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function search() {
            getThongKes();
        }
    }
})(angular.module('epk.thongkechitiets'));