
angular.module("myControllers", []);
var myApp = angular.module("myApp", ['ngRoute', 'xsockets', 'myControllers']);

myApp.config(['$locationProvider', '$routeProvider', 'xsProvider',
    function ($locationProvider, $routeProvider, xsProvider) {
        // just set up one simple route
        $routeProvider.when('/animal/', {
            templateUrl: '/app/views/animal/default.htm',
            controller: 'AnimalController'
        }).otherwise({
            redirectTo: '/animal'
        });

        xsProvider.url = "ws://" + location.host;
        xsProvider.controllers = ["animal"];
    }
]);