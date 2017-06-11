(function (app) {
    app.controller('dmvethangEditController', dmvethangEditController);

    dmvethangEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function dmvethangEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.dmVeThang = {}

        $scope.UpdateDmVeThang = updateDmVeThang;

        function loadDmVeThangDetail() {
            apiService.get('api/dmvethang/getbyid/' + $stateParams.id, null, function (result) {
                $scope.dmVeThang = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function updateDmVeThang() {
            apiService.put('api/dmvethang/update', $scope.dmVeThang, updateSuccessed, udpateFailed);
        }

        function updateSuccessed() {
            notificationService.displaySuccess($scope.dmVeThang.Ten + ' đã được cập nhật thành công.');

            $state.go('dmvethangs');
        }
        function udpateFailed(response) {
            notificationService.displayError(response.data.Message);
        }

        loadDmVeThangDetail();
    }
})(angular.module('epk.dmvethangs'));