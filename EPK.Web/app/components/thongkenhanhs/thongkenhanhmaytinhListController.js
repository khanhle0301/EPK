(function (app) {
    app.controller('thongkenhanhmaytinhListController', thongkenhanhmaytinhListController);

    thongkenhanhmaytinhListController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function thongkenhanhmaytinhListController($scope, apiService, notificationService, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.maytinh = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date(2015, 01, 01);
        $scope.ketThuc = new Date(2020, 01, 01);

        $scope.sort = 'ngay';

        $scope.xethang = true;
        $scope.vanglai = true;
        $scope.selectAll = selectAll;
        $scope.search = search;

        $scope.getThongKes = getThongKes;

        $scope.getMayTinhs = getMayTinhs;

        function convert(str) {
            var date = new Date(str),
                mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                day = ("0" + date.getDate()).slice(-2);
            return [date.getFullYear(), mnth, day].join("-");
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.maytinh, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.maytinh, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("maytinh", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnThongKe').removeAttr('disabled');
            } else {
                $('#btnThongKe').attr('disabled', 'disabled');
            }
        }, true);



        function getMayTinhs() {
            apiService.get('api/maytinh/getall', null, function (result) {
                $scope.maytinh = result.data;
            }, function () {
                console.log('Cannot get all');
            });
        }

        function getThongKes(page) {
            $scope.loading = true;
            page = page || 0;

            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.Id);
            });

            var config = {
                param: {
                    page: page,
                    pageSize: 10,
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    vanglai: $scope.vanglai,
                    thang: $scope.xethang,
                    sort: $scope.sort,
                    maytinh: JSON.stringify(listId),
                    nhanvien: ''
                }
            }
            apiService.get('api/thongkenhanh/getall?page=' + config.param.page + '&pageSize=' + config.param.pageSize + '&batDau='
                + config.param.batDau + '&ketThuc=' + config.param.ketThuc + '&vanglai=' + config.param.vanglai + '&thang=' +
                config.param.thang + '&sort=' + config.param.sort + '&maytinh=' + config.param.maytinh + '&nhanvien=' + config.param.nhanvien,
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

        $scope.getMayTinhs();
    }
})(angular.module('epk.thongkenhanhs'));