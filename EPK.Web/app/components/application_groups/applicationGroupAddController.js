(function (app) {
    'use strict';

    app.controller('applicationGroupAddController', applicationGroupAddController);

    applicationGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function applicationGroupAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.group = {
            Id: 0,
            DangSuDung: true,
            Roles: []
        }

        $scope.addAppGroup = addApplicationGroup;

        function addApplicationGroup() {
            apiService.post('/api/applicationGroup/create', $scope.group, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.group.Name + ' đã được thêm mới.');

            $location.url('application_groups');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayError(response);
        }
        function loadRoles() {
            apiService.get('/api/applicationRole/getall',
                null,
                function (response) {
                    $scope.roles = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách quyền.');
                });

        }

        loadRoles();

    }
})(angular.module('epk.application_groups'));