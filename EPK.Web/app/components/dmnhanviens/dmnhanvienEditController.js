(function (app) {
    app.controller('dmnhanvienEditController', dmnhanvienEditController);

    dmnhanvienEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function dmnhanvienEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.dmNhanvien = {}

        $scope.UpdateDmNhanVien = updateDmNhanVien;

        function loadDmNhanVienDetail() {
            apiService.get('api/dmnhanvien/getbyid/' + $stateParams.id, null, function (result) {
                $scope.dmNhanvien = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function updateDmNhanVien() {
            apiService.put('api/dmnhanvien/update', $scope.dmNhanvien, updateSuccessed, udpateFailed);
        }

        function updateSuccessed() {
            notificationService.displaySuccess($scope.dmNhanvien.Ten + ' đã được cập nhật thành công.');

            $state.go('dmnhanviens');
        }
        function udpateFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        loadDmNhanVienDetail();
    }
})(angular.module('epk.dmnhanviens'));