var app = new Vue({
    el: '#contentcontainer',
    data: {
        connections: [],
        connection: new tndStudios.models.dataConnections.dataConnection()
    },
    computed: {
    },
    methods: {

        // Add a new item to the definition list
        add: function () {

            // Push the new item
            app.connections.push(new tndStudios.models.dataConnections.dataConnection());
        },
        
        load: function () {
            tndStudios.utils.api.call(
                '/api/data/connection',
                'GET',
                null,
                app.loadSuccess,
                app.loadFailure);
        },

        // Load was successful, assign the data
        loadSuccess: function (data) {
            if (data.data) {
                app.connections = data.data; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadFailure: function () {
            alert('Failed to retrieve existing connections list')
        },
    }
});

app.load();