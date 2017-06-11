(function (app) {
    app.controller('dmvethangListController', dmvethangListController);

    dmvethangListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function dmvethangListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.totalCount = 0;

        $scope.getDmVeThangs = getDmVeThangs;

        $scope.deleteDmNhanVien = deleteDmNhanVien;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.Id);
            });
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        listId: JSON.stringify(listId)
                    }
                }
                apiService.del('api/dmvethang/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi.');
                    getDmVeThangs();
                }, function (error) {
                    notificationService.displayError('Xóa không thành công');
                });
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.data, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.data, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("data", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteDmNhanVien(id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/dmvethang/delete',
                    config,
                    function () {
                        notificationService.displaySuccess('Xóa thành công');
                        getDmVeThangs();
                    },
                    function () {
                        notificationService.displayError('Xóa không thành công');
                    });
            });
        }


        function getDmVeThangs(page) {
            $scope.loading = true;
            page = page || 0;
            var config = {
                params: {
                    page: page,
                    pageSize: 5
                }
            }
            apiService.get('api/dmvethang/getall',
                config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            if (result.data.TotalCount === 0) {
                notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
            }
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        $scope.getDmVeThangs();
    }
})(angular.module('epk.dmvethangs'));