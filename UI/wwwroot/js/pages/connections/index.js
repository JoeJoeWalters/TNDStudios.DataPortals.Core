var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.dataConnections.page()
    },
    computed: {
    },
    methods: {

        // Add a new item to the definition list
        add: function () {

            // Push the new item
            app.page.connections.push(new tndStudios.models.dataConnections.dataConnection());
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
            alert('Failed to retrieve the list of provider types')
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
                app.page.connections = data.data; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadConnectionsFailure: function () {
            alert('Failed to retrieve existing connections list')
        },
    }
});

app.load();