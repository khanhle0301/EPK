(function (app) {
    app.controller('timkiemListController', timkiemListController);

    timkiemListController.$inject = ['$scope', 'apiService', 'notificationService'];

    function timkiemListController($scope, apiService, notificationService) {
        $scope.loading = true;
        $scope.data = [];
        $scope.hinhanhvao = [];
        $scope.hinhanhra = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.keyword = '';
        $scope.trongbai = true;
        $scope.ngoaibai = true;
        $scope.thoigian = 'vao';

        $scope.xethang = true;
        $scope.vanglai = true;

        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

        $scope.search = search;
        $scope.getTimKiems = getTimKiems;
        $scope.getImage = getImage;

        $scope.showImage = showImage;
        function showImage(item) {
            var res = item.split('-');
            $('.tr-click').removeAttr('style', 'background-color:gray');
            $('.tr-click-' + res[0]).attr('style', 'background-color:gray');
            getImage(item);
        };

        function getImage(mahinhanh) {
            $scope.loading = true;
            var config = {
                params: {
                    maHinhAnh: mahinhanh
                }
            }
            apiService.get('api/timkiem/showimage',
                config, dataLoadCompletedImage, dataLoadFailedImage);
        }

        function dataLoadCompletedImage(result) {
            $scope.hinhanhvao = result.data;
            $scope.hinhanhra = result.data;
            $scope.loading = false;
        }
        function dataLoadFailedImage(response) {
            notificationService.displayError(response.data.Message);
        }


        function convert(str) {
            var date = new Date(str),
                mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                day = ("0" + date.getDate()).slice(-2);
            return [date.getFullYear(), mnth, day].join("-");
        }

        function getTimKiems(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    maThe: $scope.keyword,
                    bienSo: $scope.keyword,
                    trongbai: $scope.trongbai,
                    ngoaibai: $scope.ngoaibai,
                    thoigian: $scope.thoigian,
                    xethang: $scope.xethang,
                    vanglai: $scope.vanglai
                }
            }
            apiService.get('api/timkiem/timkiem',
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
            $('.tblHinhAnhVao').html('');
            $('.tblHinhAnhRa').html('');
            getTimKiems();
        }
    }
})(angular.module('epk.timkiems'));