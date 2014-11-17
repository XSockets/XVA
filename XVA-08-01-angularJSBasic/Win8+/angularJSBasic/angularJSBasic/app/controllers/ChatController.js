angular.module("myControllers",[]).controller("ChatController", [
    "$scope","xs",
        function ($scope, xs) {
            var chatMessageModel = function(message) {
                this.message = message;
                this.created = new Date();
            };
            $scope.chatMessages = [];
            $scope.chatMessage = "";
            var controller = xs.controller("Chat");
            controller.on("chatMessage", function (model) {
                $scope.chatMessages.unshift(model);
            });
            $scope.sendChatMessage = function() {
                controller.invoke("chatMessage",new chatMessageModel($scope.chatMessage));
            };
        }
]);