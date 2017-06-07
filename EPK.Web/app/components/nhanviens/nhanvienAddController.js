(function (app) {
    app.controller('nhanvienAddController', nhanvienAddController);

    nhanvienAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function nhanvienAddController(apiService, $scope, notificationService, $state) {
        $scope.nhanVien = {
            dangsudung: true
        }

        $scope.AddNhanVien = addNhanVien;

        function addNhanVien() {
            apiService.post('api/nhanvien/create', $scope.nhanVien,
                function (result) {
                    notificationService.displaySuccess(result.data.Ten + ' đã được thêm mới.');
                    $state.go('nhanviens');
                }, function () {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }

        function loadDmNhanVien() {
            apiService.get('api/dmnhanvien/getallparents', null, function (result) {
                $scope.dmNhanViens = result.data;
            }, function () {
                console.log('Không thể tải danh mục nhân viên');
            });
        }

        loadDmNhanVien();
    }
})(angular.module('epk.nhanviens'));