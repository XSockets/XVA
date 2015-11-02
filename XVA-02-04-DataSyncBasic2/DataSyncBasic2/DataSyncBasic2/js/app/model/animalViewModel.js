//////////////////////
// ANIMAL VM
//////////////////////
var AnimalViewModel = (function () {
    var vm = function () {
        var self = this;
        var knockify = function (o, t) {
            if (!t) {               
                for (var p1 in o) {
                    if (o.hasOwnProperty(p1)) {
                        o[p1] = ko.observable(o[p1]);
                    }
                }
                return o;
            }
            else {
                var js = ko.toJS(o);
                for (var p2 in js) {
                    if (js.hasOwnProperty(p2)) {
                        t[p2](js[p2]);
                    }
                }
                return t;
            }            
        }
        this.Animals = ko.observableArray([]);
        this.UpdateAnimal = function (animal) {
            //create observable
            animal = knockify(animal);
            var match = ko.utils.arrayFirst(self.Animals(), function (item) {
                return animal.Id() === item.Id();
            });
            if (!match) {
                //Add, did not exist
                self.Animals.unshift(animal);
            } else {
                //Existed, update 
                //copy observable to another observable - object, target
                knockify(animal,match);
            }
        }
        this.RemoveAnimal = function (animal) {
            this.Animals.remove(function (item) { return item.Id() == animal.Id; });
        }
    }    
    return vm;
})();