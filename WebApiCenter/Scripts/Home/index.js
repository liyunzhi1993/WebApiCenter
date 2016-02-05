var app = angular.module('index', []).controller('indexCtrl', function ($scope, $http) {
    $scope.submit = function () {
        $http({
            method: 'POST',
            url: '/Home/Login',
            data: {"account":$("#account").val(),"password":$("#password").val()}
        })
            .success(function (data) {
                if (!data.success) {
                    $scope.error = data.message;
                } else {
                    $scope.error = "";
                    window.location.href = "/Client/Index";
                }
            });
    };
});