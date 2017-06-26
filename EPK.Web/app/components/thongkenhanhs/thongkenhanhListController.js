(function (app) {
    app.controller('thongkenhanhListController', thongkenhanhListController);

    thongkenhanhListController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function thongkenhanhListController($scope, apiService, notificationService, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.nhanvien = [];
        $scope.maytinh = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.batDau = new Date();
        $scope.ketThuc = new Date();

        $scope.sort = 'ngay';
        $scope.loaiTK = 'tatca';

        $scope.xethang = true;
        $scope.vanglai = true;
        $scope.selectAllNV = selectAllNV;
        $scope.selectAllMT = selectAllMT;
        $scope.search = search;

        $scope.getThongKes = getThongKes;
        $scope.exportExcel = exportExcel;
        $scope.getNhanViens = getNhanViens;
        $scope.switchSelect = switchSelect;
        $scope.getMayTinhs = getMayTinhs;

        function switchSelect() {
            switch ($scope.loaiTK) {
                case 'tatca':
                    tatca();
                    break;
                case 'maytinh':
                    maytinh();
                    break;
                case 'nhanvien':
                    nhanvien();
                    break;
                default:
                    tatca();
            }
        }

        function getMayTinhs() {
            apiService.get('api/maytinh/getall', null, function (result) {
                $scope.maytinh = result.data;
            }, function () {
                console.log('Cannot get all');
            });
        }

        function tatca() {
            $('#tblMayTinh').hide();
            $('#tblNhanVien').hide();
            $('#btnThongKe').removeAttr('disabled');
            $('#tblData').removeAttr('class');
            $('#tblData').attr('class', 'col-md-12');
        }

        function maytinh() {
            $scope.getMayTinhs();
            $('#tblMayTinh').show();
            $('#tblNhanVien').hide();
            $('#tblData').removeAttr('class');
            $('#tblData').attr('class', 'col-md-10');
        }

        function nhanvien() {
            $scope.getNhanViens();
            $('#tblMayTinh').hide();
            $('#tblNhanVien').show();
            $('#tblData').removeAttr('class');
            $('#tblData').attr('class', 'col-md-10');
        }


        function exportExcel() {
            var config = {
                params: {
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc)
                }
            }
            apiService.get('/api/thongkenhanh/ExportXls', config, function (response) {
                if (response.status = 200) {
                    window.location.href = response.data.Message;
                }
            }, function (error) {
                notificationService.displayError(error);

            });
        }

        function convert(str) {
            var date = new Date(str),
                mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                day = ("0" + date.getDate()).slice(-2);
            return [date.getFullYear(), mnth, day].join("-");
        }

        $scope.isAll = false;
        function selectAllNV() {
            if ($scope.isAll === false) {
                angular.forEach($scope.nhanvien, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.nhanvien, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("nhanvien", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selectednv = checked;
                $('#btnThongKe').removeAttr('disabled');
            } else {
                $('#btnThongKe').attr('disabled', 'disabled');
            }
        }, true);

        function selectAllMT() {
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
                $scope.selectedmt = checked;
                $('#btnThongKe').removeAttr('disabled');
            } else {
                $('#btnThongKe').attr('disabled', 'disabled');
            }
        }, true);

        function getNhanViens() {
            apiService.get('api/applicationUser/getall', null, function (result) {
                $scope.nhanvien = result.data;
            }, function () {
                console.log('Cannot get all');
            });
        }

        function getThongKes(page) {
            $scope.loading = true;
            page = page || 0;

            var maytinh = null;
            var nhanvien = null;

            if ($scope.loaiTK == 'tatca') {
                maytinh = '';
                nhanvien = '';
            } else {
                if ($scope.loaiTK == 'nhanvien') {
                    maytinh = '';
                    var listId = [];
                    $.each($scope.selectednv, function (i, item) {
                        listId.push(item.Id);
                    });
                    nhanvien = JSON.stringify(listId);
                } else {
                    nhanvien = '';
                    var listId = [];
                    $.each($scope.selectedmt, function (i, item) {
                        listId.push(item.Id);
                    });
                    maytinh = JSON.stringify(listId);
                }
            }

            var listId = [];
            $.each($scope.selectedmt, function (i, item) {
                listId.push(item.Id);
            });

            var config = {
                params: {
                    page: page,
                    pageSize: 10,
                    batDau: convert($scope.batDau),
                    ketThuc: convert($scope.ketThuc),
                    vanglai: $scope.vanglai,
                    thang: $scope.xethang,
                    sort: $scope.sort,
                    maytinh: maytinh,
                    nhanvien: nhanvien
                }
            }
            apiService.get('api/thongkenhanh/getall', config, dataLoadCompleted, dataLoadFailed);
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
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function search() {
            getThongKes();
        }

        $scope.switchSelect();
    }
})(angular.module('epk.thongkenhanhs'));