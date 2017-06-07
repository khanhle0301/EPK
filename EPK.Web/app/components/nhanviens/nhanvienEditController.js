(function (app) {
    app.controller('nhanvienEditController', nhanvienEditController);

    nhanvienEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function nhanvienEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.nhanvien = {}

        $scope.Updatenhanvien = updatenhanvien;

        function loadnhanvienDetail() {
            apiService.get('api/nhanvien/getbyid/' + $stateParams.id, null, function (result) {
                $scope.nhanvien = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function updatenhanvien() {
            apiService.put('api/nhanvien/update', $scope.nhanvien, updateSuccessed, udpateFailed);
        }

        function updateSuccessed() {
            notificationService.displaySuccess($scope.nhanvien.Ten + ' đã được cập nhật thành công.');

            $state.go('nhanviens');
        }
        function udpateFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        function loadDmNhanVien() {
            apiService.get('api/dmnhanvien/getallparents', null, function (result) {
                $scope.dmNhanViens = result.data;
            }, function () {
                console.log('Không thể tải danh mục nhân viên');
            });
        }

        loadDmNhanVien();

        loadnhanvienDetail();
    }
})(angular.module('epk.nhanviens'));