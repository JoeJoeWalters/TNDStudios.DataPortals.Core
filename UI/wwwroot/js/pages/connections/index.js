var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.dataConnections.page()
    },
    computed: {
    },
    methods: {

        // Add a new item to the definition list
        new: function () {

            // Clear the editor
            app.page.editor.clear();
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the connection editor from the selected object
            app.page.editor.fromObject(editItem);
            app.page.editItem = editItem; // Reference to the origional item being edited
        },

        // Save the contents of the editor object 
        save: function () {

            // The the api call to save the connection
            tndStudios.utils.api.call(
                '/api/data/connection',
                'POST',
                app.page.editor.toObject(),
                app.saveSuccess,
                app.saveFailure
            );
        },

        // Save was successful, assign the appropriate items
        saveSuccess: function (data) {

            // Some data came back?
            if (data.data) {

                // Update the editor itself (possibily with new links or id if a new item)
                app.page.editor.fromObject(data.data);

                // Were we editing an existing item?
                if (app.page.editItem != null) {

                    // Update the existing item
                    app.page.editItem.fromObject(data.data);
                }
                else {

                    // Add the new item to the list
                    app.page.connections.push(new tndStudios.models.dataConnections.dataConnection(data.data));
                }
            };
        },

        // Save was unsuccessful, inform the user
        saveFailure: function () {
            //alert('Failed save the connection');
        },


        // Test the connection of a given connection
        test: function (testItem) {
            alert('Testing Object');
        },

        // Start the load process
        load: function () {

            // Load the provider types
            app.loadProviderTypes();

            // Start loading the connections list
            app.loadConnections();
        },

        // Load the available provider types
        loadProviderTypes: function () {

            // The the api call to load the provider types
            tndStudios.utils.api.call(
                '/api/data/providers',
                'GET',
                null,
                app.loadProviderTypesSuccess,
                app.loadProviderTypesFailure
            );
        },

        // Load was successful, assign the data
        loadProviderTypesSuccess: function (data) {
            if (data.data) {
                app.page.providerTypes = data.data; // Assign the Json package to the provider types
            };
        },

        // Load was unsuccessful, inform the user
        loadProviderTypesFailure: function () {
            //alert('Failed to retrieve the list of provider types')
        },

        // load connections from the server
        loadConnections: function () {

            // Start the api call to load the connections
            tndStudios.utils.api.call(
                '/api/data/connection',
                'GET',
                null,
                app.loadConnectionsSuccess,
                app.loadConnectionsFailure);
        },

        // Load was successful, assign the data
        loadConnectionsSuccess: function (data) {
            if (data.data) {

                app.page.connections = []; // clear the connections array

                // Add the connection objects back in with wrapper for additional functions
                data.data.forEach(function (connection)
                {
                    app.page.connections.push(new tndStudios.models.dataConnections.dataConnection(connection)); // Assign the Json package to the data definition
                });
            };
        },

        // Load was unsuccessful, inform the user
        loadConnectionsFailure: function () {
            //alert('Failed to retrieve existing connections list')
        },
    }
});

app.load();