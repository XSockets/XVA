//Angular Modules (only chatcontroller included here)
angular.module("myControllers", []).controller("ChatController", [
    "$scope","xs",
        function ($scope, xs) {
            //model
            var chatMessageModel = function(message) {
                this.message = message;
                this.created = new Date();
            };
            //The messages...
            $scope.chatMessages = [];
            //Message to send
            $scope.chatMessage = "";
            // get the controller
            var controller = xs.controller("chat");
            controller.onopen.then(function (c) {
                $scope.connectInfo = JSON.stringify(c);
            });
            //The server calls this method (or actually the callback fn)
            controller.on("chatMessage", function (model) {
                $scope.chatMessages.unshift(model);
            });
            //Call the chatMessage method on the server side controller
            $scope.sendChatMessage = function() {
                controller.invoke("chatMessage",new chatMessageModel($scope.chatMessage));
            };
        }
]);