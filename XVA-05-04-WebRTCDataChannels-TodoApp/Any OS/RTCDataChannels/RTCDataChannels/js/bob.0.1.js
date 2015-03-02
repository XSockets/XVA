

var $ = $ || function (selector, el) {
    if (!el) el = document;
    var args = arguments;
    if (args.length === 0) return el;

    if (typeof (arguments[0]) === "function") {
        el.addEventListener("DOMContentLoaded", function (e) {
            args[0].call(this, e);
        });
        return el;
    }
    return el.querySelector(selector);
};


var Bob = Bob || {};
Bob.serializeForm = function (form) {

    if (!form) form = this;
    var data, i, len, node, ref;
    data = {};
    ref = form.elements;
    for (i = 0, len = ref.length; i < len; i++) {
        node = ref[i];
        if (!node.disabled && node.name) {
            data[node.name] = node.value;
        }
    }
    return data;
};

Bob.binders = {
    value: function (node, onchange) {
        node.addEventListener('keyup', function () {
            onchange(node.value);
        });
        return {
            updateProperty: function (value) {
                if (value !== node.value) {
                    node.value = value;
                }
            }
        };
    },
    count: function (node) {
        return {
            updateProperty: function (value) {
                node.textContent = String(value).length;
            }
        };
    },
    text: function(node) {
        return {
            updateProperty: function (value) {
                node.textContent = value;
            }
        };
    },
    click: function (node, onchange, object) {
        var previous;
        return {
            updateProperty: function (fn) {
         
                var listener = function (e) {
                  
                    fn.apply(object, arguments);
                    e.preventDefault();
                };
                if (previous) {
                    node.removeEventListener(previous);
                    previous = listener;
                }
                node.addEventListener('click', listener);
            }
        };
    }
};

Bob.apply = function (binders, callbacks) {

    function bindObject(node, binderName, object, propertyName) {
        var updateValue = function (newValue) {
            object[propertyName] = newValue;
        };
        var binder = binders[binderName](node, updateValue, object);
        binder.updateProperty(object[propertyName]);
        var observer = function (changes) {
            var changed = changes.some(function (change) {
                return change.name === propertyName;
            });
            if (changed) {
                if (typeof (callbacks.update) === "function")
                  callbacks.update(object[propertyName], changes[0]);
                binder.updateProperty(object[propertyName]);
            }
        };
        Object.observe(object, observer);
        return {
            unobserve: function () {
                Object.unobserve(object, observer);

            },
            observe: function() {
                Object.observe(object, observer);
            }
        };
    }

    function bindCollection(node, array) {
        function capture(original) {
            var before = original.previousSibling;
            var parent = original.parentNode;
            var node = original.cloneNode(true);
            original.parentNode.removeChild(original);
            return {
                insert: function () {
                    var newNode = node.cloneNode(true);
                    parent.insertBefore(newNode, before);
                    return newNode;
                }
            };
        }

        delete node.dataset.repeat;
        var parent = node.parentNode;
        var captured = capture(node);
        var bindItem = function (element) {
            return bindModel(captured.insert(), element);
        };
        var bindings = array.map(bindItem);
        var observer = function (changes) {
            changes.forEach(function (change) {
                var index = parseInt(change.name, 10), child;
                if (isNaN(index)) return;
                if (change.type === 'add') {
                    if (typeof (callbacks.add) === "function")
                        callbacks.add(array[index]);
                    bindings.push(bindItem(array[index]));
                } else if (change.type === 'update') {
                    bindings[index].unobserve();
                    bindModel(parent.children[index], array[index]);
                } else if (change.type === 'delete') {
                    if (typeof (callbacks.remove) === "function")
                        callbacks.remove(change.oldValue);

                    bindings.pop().unobserve();
                    child = parent.children[index];
                    child.parentNode.removeChild(child);
                }
            });
        };
        Object.observe(array, observer);
        return {
            unobserve: function () {
                Object.unobserve(array, observer);
            },
            observe: function () {
                Object.observe(object, observer);
            }


        };
    }

    function bindModel(container, object) {
        function isDirectNested(node) {
            node = node.parentElement;
            while (node) {
                if (node.dataset.repeat) {
                    return false;
                }
                node = node.parentElement;
            }
            return true;
        }

        function onlyDirectNested(selector) {
            var collection = container.querySelectorAll(selector);
            return Array.prototype.filter.call(collection, isDirectNested);
        }

        var bindings = onlyDirectNested('[data-bind]').map(function (node) {
            var parts = node.dataset.bind.split(' ');
            return bindObject(node, parts[0], object, parts[1]);
        }).concat(onlyDirectNested('[data-repeat]').map(function (node) {
            return bindCollection(node, object[node.dataset.repeat]);
        }));

        return {
            unobserve: function () {
                bindings.forEach(function (binding) {
                    binding.unobserve();
                });
            },
            observe: function () {
                bindings.forEach(function (binding) {
                    binding.observe();
                });
            }
        };
    }
    return {
        bind: bindModel
    };
}
