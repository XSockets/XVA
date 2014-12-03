//////////////////////
// ANIMAL VIEW
//////////////////////
var AnimalView = (function () {
    var model = new AnimalViewModel();

    //View
    var view = function (ctrl) {                

        //Callbacks from controller when the service send new data
        var onInit = function (animals) {
            animals.forEach(model.UpdateAnimal);
        };
        var onAddOrUpdate = function (animal) {
            model.UpdateAnimal(animal);
        };
        var onDelete = function (animal) {
            model.RemoveAnimal(animal);
        };

        var controller = ctrl || new AnimalController(onAddOrUpdate, onDelete, onInit);
        
        this.addOrUpdate = function (animal) {
            controller.addOrUpdate(animal);
        };
        this.remove = function (animal) {
            controller.remove(animal);
        };
        
        //UI Events
        //new 
        document.querySelector('#btn-animal-add').addEventListener('click', function () {
            //Do not save if the name is missing
            if (document.querySelector('#input-animal-name').value.trim().length == 0) {
                alert("You have to provide a name for the animal");
                return;
            }
            controller.addOrUpdate({
                Name: document.querySelector('#input-animal-name').value,
                Age: document.querySelector('#input-animal-age').value,
                Cage: document.querySelector('#input-animal-cage').value,
                Hungry: document.querySelector('#input-animal-hungry').checked
            });
        });
    };
    //Apply KO
    ko.applyBindings(model);
    return view;
})();