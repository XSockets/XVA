angular.module("myControllers").controller("AnimalController",
    ['$scope', 'xs','xsDataSync',
    function ($scope, xs,dataSync) {
        var ds = new dataSync(xs.controller("animal"));

        $scope.animals = [];
        $scope.current = {};

        var findById = function (id) {
            var l = $scope.animals.length;
            var match = -1;
            for (var i = 0; i < l; i++) {
                if ($scope.animals[i].Id === id) {
                    match = i;
                    break;
                }
            }
            return match;
        };

        // *** methods
        $scope.edit = function(data) {
            $scope.current = data;
        };

        $scope.add = function () {
            $scope.current = { id: 0 };
        };

        $scope.addOrUpdate = function () {
            ds.updateItem($scope.current);
        };

        $scope.delete = function(entity) {
            ds.deleteItem(entity);
        };

        // *** events

        // get the existing data..
        ds.oninit = function(data) {
            data.forEach(function (entity) {
                $scope.animals.unshift(entity);
            });
        };
        // fires both on update / add
        ds.onupdated = function (data) {
            console.log(data);
            var index = findById(data.Id);
            console.log(index);
            if (index == -1) {
                $scope.animals.unshift(data);
            }
            else $scope.animals[index] = data;
        };
        // fires when something is deleted
        ds.ondeleted = function (data) {
            var index = findById(data.Id);
            $scope.animals.splice(index, 1);
        };
    }
    ]);