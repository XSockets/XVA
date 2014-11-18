angular.module("myControllers",[]).controller("ChatController", [
    "$scope","xs",
        function ($scope, xs) {
            var chatMessageModel = function(message) {
                this.message = message;
                this.created = new Date();
            };
            $scope.chatMessages = [];
            $scope.chatMessage = "";
            // get the controller
            var controller = xs.controller("chat");
            controller.onopen.then(function (c) {
                $scope.connectInfo = JSON.stringify(c);
            });
            controller.on("chatMessage", function (model) {
                $scope.chatMessages.unshift(model);
            });
            $scope.sendChatMessage = function() {
                controller.invoke("chatMessage",new chatMessageModel($scope.chatMessage));
            };
        }
]);