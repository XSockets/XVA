//Add myApp module
angular.module("myApp", ['ngRoute', 'myControllers','xsockets']);

//Configure myApp module
angular.module('myApp').config(['$locationProvider', '$routeProvider','xsProvider',
    function ($locationProvider, $routeProvider, xsProvider) {
        // just set up one simple route to our view and tell it to use the ChatController
        $routeProvider.when('/chat/', {
            templateUrl: '/app/views/chat.html',
            controller: 'ChatController'
        }).otherwise({
            redirectTo: '/chat'
        });
        //Configure XSockets
        xsProvider.url = "ws://127.0.0.1:4502";
        xsProvider.controllers = ["chat"];        
    }
]);


