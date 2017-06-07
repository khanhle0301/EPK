(function (app) {
    'use strict';

    app.controller('applicationUserEditController', applicationUserEditController);

    applicationUserEditController.$inject = ['$scope', 'ApiService', 'notificationService', '$location', '$stateParams'];

    function applicationUserEditController($scope, ApiService, notificationService, $location, $stateParams) {
        $scope.account = {}


        $scope.updateAccount = updateAccount;

        function updateAccount() {
            ApiService.put('/Api/applicationUser/update', $scope.account, addSuccessed, addFailed);
        }
        function loadDetail() {
            ApiService.get('/Api/applicationUser/detail/' + $stateParams.id, null,
            function (result) {
                $scope.account = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.account.FullName + ' đã được cập nhật thành công.');

            $location.url('application_users');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        function loadGroups() {
            ApiService.get('/Api/applicationGroup/getlistall',
                null,
                function (response) {
                    $scope.groups = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                });

        }

        loadGroups();
        loadDetail();
    }
})(angular.module('epk.application_users'));
