(function (app) {
    app.controller('dmnhanvienAddController', dmnhanvienAddController);

    dmnhanvienAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function dmnhanvienAddController(apiService, $scope, notificationService, $state) {
        $scope.dmNhanvien = {
            dangsudung: true
        }

        $scope.AddDmNhanVien = addDmNhanVien;

        function addDmNhanVien() {
            apiService.post('api/dmnhanvien/create', $scope.dmNhanvien,
                function (result) {
                    notificationService.displaySuccess(result.data.Ten + ' đã được thêm mới.');
                    $state.go('dmnhanviens');
                }, function () {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('epk.dmnhanviens'));