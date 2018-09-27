var app = new Vue({
    el: '#test',
    data: {
        definition: new tndStudios.models.dataItems.dataItemDefinition()
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

        // Add a new item to the definition list
        add: function () {

            // Push the new item
            app.definition.itemProperties.push(new tndStudios.models.dataItems.dataItemProperty());
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
                app.definition = data.data; // Assign the Json package to the data definition

                // Add additional functional items needed for the operation of the model
                $.each(app.definition.itemProperties, function (i, prop) {
                    Vue.set(prop, "removed", false); // Flag to indicate if the item has been marked as removed
                    Vue.set(prop, "dirty", false); // Flag to indicate that there is a change to the definition
                });
            };
        },

        // Load was unsuccessful, inform the user
        loadFailure: function () {
            alert('Failed to retrieve definition of the data')
        }
    }
});