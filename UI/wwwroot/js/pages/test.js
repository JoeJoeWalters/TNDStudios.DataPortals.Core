Vue.component('item-header',
    {
        methods:
        {
            remove: function () {
                this.$props["removeclick"](this);
            }
        },
        props: ['data', 'removeclick'],
        template: '<div v-on:click="remove" v-bind:title="data.description">{{ data.name }}{{ data.removed }}</div>'
    });

var app = new Vue({
    el: '#test',
    data: {
        definition:
        {
        }
    },
    computed: {
        computedExample: function () {
            return "";
        }
    },
    methods: {

        // Remove the definition item from the list
        remove: function (toRemove) {
            toRemove.data.removed = true; // Mark the item as removed
        },

        // Load the definition data from the end point
        load: function () {
            tndStudios.utils.api.call(
                'api/test/definition',
                'GET',
                {},
                app.loadSuccess,
                app.loadFailure);
        },

        // Load was successful, assign the data
        loadSuccess: function (data) {
            if (data.data) {
                app.$data.definition = data.data; // Assign the Json package to the data definition

                // Add additional functional items needed for the operation of the model
                $.each(app.$data.definition.itemProperties, function (i, prop) {
                    Vue.set(prop, "removed", false);
                });
            };
        },

        // Load was unsuccessful, inform the user
        loadFailure: function () {
            alert('Failed to retrieve definition of the data')
        }
    }
});