(function (app) {
    app.controller('dmvethangAddController', dmvethangAddController);

    dmvethangAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function dmvethangAddController(apiService, $scope, notificationService, $state) {
        $scope.dmVeThang = {
            dangsudung: true
        }

        $scope.AddDmVeThang = addDmVeThang;

        function addDmVeThang() {
            apiService.post('api/dmvethang/create', $scope.dmVeThang,
                function (result) {
                    notificationService.displaySuccess(result.data.Ten + ' đã được thêm mới.');
                    $state.go('dmvethangs');
                }, function () {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }
})(angular.module('epk.vethangs'));