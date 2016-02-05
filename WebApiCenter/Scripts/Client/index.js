var app = angular.module('index', []).controller('indexCtrl', function ($scope, $http) {
    //初始化
    function init()
    {
        $scope.isnew = true;
        $scope.error = true;
        $scope.incomplete = true;
        $scope.id = "";
        $scope.clientname = "";
        $scope.clientid = "";
        $scope.clientsecret = "";
        $scope.reqip = "";
    }
    $scope.createsecret = function () {
        $http({
            method: 'POST',
            url: '/Client/CreateSecret'
        })
            .success(function (data) {
                if (data != null) {
                    $scope.clientid = data.clientid;
                    $scope.clientsecret = data.clientsecret;
                } else {
                    error(data);
                }
            });
    };
    $scope.setnew = function () {
        init();
        $("#collapseOne").collapse("show");
        $("#collapseTwo").collapse("hide");
        $("#collapseThree").collapse("hide");
        $scope.createsecret();
    }
    //获取客户端列表
    function getclientlist() {
        $http.get("/Client/GetClientList")
        .success(function (data) {
            if (data!=null) {
                $scope.clientlist = data;
            } else {
                error(data);
            }
        });
    };
    //获取权限列表
    function getapilist() {
        $http.get("/Client/GetApiList")
        .success(function (data) {
            if (data != null) {
                $scope.apilist = data;
            } else {
                error(data);
            }
        });
    };
    $scope.setnew();
    getclientlist();
    getapilist();
    $scope.$watch('clientname', function () { $scope.check(); });
    $scope.$watch('clientid', function () { $scope.check(); });
    $scope.$watch('clientsecret', function () { $scope.check(); });
    $scope.check = function () {
        if ($scope.clientname == "" || $scope.clientid == "" || $scope.clientsecret == "") {
            $scope.error = true;
            return;
        }
        $scope.error = false;
        $scope.incomplete = false;
    };
    $scope.submit = function () {
        $http({
            method: 'POST',
            url: '/Client/Save',
            data: { id:$scope.id,"clientname": $scope.clientname, "clientid": $scope.clientid, "clientsecret": $scope.clientsecret, "reqip": $scope.reqip,"isnew":$scope.isnew }
        })
            .success(function (data) {
                if (data.success) {
                    getclientlist();
                    if ($scope.isnew) {
                        $scope.setnew();
                    }
                    toastr.success(data.message);
                } else {
                    error(data);
                }
            });
    };
    $scope.save = function () {
        $http({
            method: 'POST',
            url: '/Client/Save',
            data: { id: $scope.id, "clientname": $scope.clientname, "clientid": $scope.clientid, "clientsecret": $scope.clientsecret, "reqip": $scope.reqip, "isnew": $scope.isnew }
        })
            .success(function (data) {
                if (data.success) {
                    getclientlist();
                    toastr.success(data.message);
                } else {
                    error(data);
                }
            });
    };
    //删除
    $scope.del = function (id) {
        $scope.setnew();
        $http({
            method: 'POST',
            url: '/Client/Del',
            data: { "id": id }
        })
            .success(function (data) {
                if (data.success) {
                    getclientlist();
                    toastr.success(data.message);
                } else {
                    error(data);
                }
            });
    };
    //获取当前编辑的API权限
    function getauthorization() {
        $http({
            method: 'POST',
            url: '/Client/GetAuthorization',
            data: { "clientsid": $scope.id }
        })
         .success(function (data) {
             if (data != null) {
                 if (data.apilibraryids == undefined) {
                     $scope.apilibraryids = "";
                 } else {
                     $scope.apilibraryids = data.apilibraryids;
                 }
                 checkin($scope.apilibraryids);
             } else {
                 error(data);
             }
         });
    };
    //插入权限
    $scope.insertauthorization=function() {
        $scope.apilibraryids = checkout();
        $http({
            method: 'POST',
            url: '/Client/InsertAuthorization',
            data: { "clientsid": $scope.id, "apilibraryids": $scope.apilibraryids }
        })
         .success(function (data) {
             if (data.success) {
                 getclientlist();
                 toastr.success(data.message);
             } else {
                 error(data);
             }
         });
    };
    //编辑
    $scope.edit = function (id,clientname, clientid, clientsecret, reqip) {
        $scope.isnew = false;
        $scope.id = id;
        $scope.clientname = clientname;
        $scope.clientid = clientid;
        $scope.clientsecret = clientsecret;
        $scope.reqip = reqip;
        getauthorization();
        $("#collapseOne").collapse("show");
        $("#collapseTwo").collapse("show");
        $("#collapseThree").collapse("hide");
    };
    $scope.changestatus = function (id) {
        $http({
            method: 'POST',
            url: '/Client/ChangeStatus',
            data: { "id": id }
        })
            .success(function (data) {
                if (data.success) {
                    getclientlist();
                    toastr.success(data.message);
                } else {
                    error(data);
                }
            });
    };
    //上传文档
    function upload() {
        var options = {
            type: "POST",
            url: '/Client/Upload',
            data: { apiid: $scope.apiid },
            success: function (data) {
                data = eval("("+data+")");
                if (data.success) {
                    getapilist();
                    toastr.success(data.message);
                } else {
                    error(data);
                }
            }
        };
        $('#form').ajaxSubmit(options);
        toastr.success("上传中，请稍后..");
    };
    $scope.select = function (id) {
        $scope.apiid = id;
        $("#file").click();
    };
    $("#file").change(function () {
        upload();
    });
    $scope.test = function (id, apiname) {
        $scope.apiid = id;
        $scope.apiname = apiname;
        $("#collapseThree").collapse("show");
        $('#collapseOne').collapse('hide');
        $('#collapseTwo').collapse('hide');
        $scope.method = "Score.QueryScoreByMembercode";
        $scope.params = "&membercode=13901648617";
        $scope.url = "http://"+window.location.hostname+":"+window.location.port+"/api";
    };
    $scope.runtest = function () {
        $http({
            method: 'POST',
            url: '/Client/RunTest',
            data: { "method": $scope.method, "paramsStr": $scope.params, "clientid": $scope.clientid, "clientsecret": $scope.clientsecret, "url": $scope.url, "apiid": $scope.apiid }
        })
            .success(function (data) {
                if (data != null) {
                    toastr.success("请求成功");
                    if ($scope.params.indexOf("format=json") > 0 || $scope.params.indexOf("format")==-1) {
                        $scope.response = JSON.stringify(data);
                    } else {
                        $scope.response = data;
                    }
                } else {
                    error(data);
                }
            });
        toastr.success("请求中，请稍后..");
    };
});