//////////////////////
// ANIMAL CTRL
//////////////////////
var AnimalController = (function () {

    var controller = function (onAddOrUpdate, onDelete, onInit) {

        //RealTime Service
        var service = new AnimalService();

        //RealTime events from our service
        service.AnimalController.on('init:animalmodel', function (animals) {
            onInit(animals);
        });
        service.AnimalController.onOpen = function() {
            service.AnimalController.subscribe('update:animalmodel',
                function(animal) {
                    onAddOrUpdate(animal);
                });
            service.AnimalController.subscribe('add:animalmodel',
                function(animal) {
                    onAddOrUpdate(animal);
                });
            service.AnimalController.subscribe('delete:animalmodel',
                function(animal) {
                    onDelete(animal);
                });
        };

        //Exposed methods
        this.addOrUpdate = function (animal) {
            console.log('add', animal);
            service.AnimalController.invoke('update', animal);
        };
        this.remove = function (animal) {
            console.log('remove', animal.Id());
            service.AnimalController.invoke('delete', { Id: animal.Id() });
        };
    };
    return controller;
})();