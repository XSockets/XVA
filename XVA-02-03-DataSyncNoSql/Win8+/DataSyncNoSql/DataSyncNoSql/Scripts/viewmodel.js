//SimpleList ViewModel
var vm = {
    items: ko.observableArray([]),
    //Call data-sync controller to delete
    tryRemove: function (f) {
        deleteItem(f);
    },
    //Call data-sync controller to update
    tryUpdate: function (f) {
        updateItem(ko.toJS(f));
    },
    //Call data-sync controller to add
    tryAdd: function () {
        var name = document.querySelector('#input-new-item').value;
        if (name.length == 0) {
            alert('You have to give the item a value');
            return;
        }
        addItem({ name: name });
    },
    //Add or update in VM
    updatedItem: function (m) {
        m.Object.name = ko.observable(m.Object.name);
        var match = ko.utils.arrayFirst(vm.items(), function (item) {
            return m.Id === item.Id;
        });

        if (!match) {
            //Add, did not exist
            vm.items.unshift(m);
        } else {
            //Existed, udpate name....
            match.Object.name(m.Object.name());
        }
    },
    //Remove from VM
    deletedItem: function (m) {
        vm.items.remove(function (item) { return item.Id == m.Id; });
    }
}