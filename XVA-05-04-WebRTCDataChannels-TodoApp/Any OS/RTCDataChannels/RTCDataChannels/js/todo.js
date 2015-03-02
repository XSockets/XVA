var TodoApp = (function () {
    var instance = function () {
        var self = this;
        var todo = function (topic, description) {
            this.id = (new Date()).getTime();
            this.topic = topic;
            this.description = description;
            this.created = new Date();
            this.modified = this.created;
            this.remove = function (e) {
                self.removeTodo(this.id);
            };
        }
        this.todos = [];
        this.removeTodo = function(id) {
            var index = -1;
            for (var i = 0; i < this.todos.length; i++) {
                if (this.todos[i].id === id) {
                    index = i;                  
                }
            }
          if(index > -1)
            this.todos.splice(index, 1);
        };

        this.createTodo = function (topic, description) {
            return new todo(topic, description);
        };

        this.addTodo = function (obj) {
            var exists = this.todos.filter(function (item) {
                return item.id === obj.id;
            });
            if (exists.length === 0) {
                var t = new todo(); // Create a new and map the incomming props
                for (var p in obj) {
                    t[p] = obj[p];
                };
                this.todos.push(t);
            }
        };
        this.updateTodo = function (id, key, value) {
            var index = -1;
            for (var i = 0; i < this.todos.length; i++) {
                if (this.todos[i].id === id) {
                    index = i;
                    break;;
                }
            }
            this.todos[index][key] = value;
        }
    };
    return instance;

})();