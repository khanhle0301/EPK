(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == true)
                return 'Đang sử dụng';
            else
                return 'Khóa';
        }
    });
})(angular.module('epk.common'));
