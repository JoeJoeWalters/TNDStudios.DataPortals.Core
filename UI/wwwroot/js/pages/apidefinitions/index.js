var app = new Vue({
    el: '#contentcontainer',
    data: {
        apiDefinition: [],
        apiDefinitions: new tndStudios.models.apiDefinitions.apiDefinition()
    },
    computed: {
    },
    methods: {

        // Add a new item to the definition list
        add: function () {

            // Push the new item
            app.connections.push(new tndStudios.models.apiDefinitions.apiDefinition());
        },

        load: function () {
            tndStudios.utils.api.call(
                '/api/managedapi/definition',
                'GET',
                null,
                app.loadSuccess,
                app.loadFailure);
        },

        // Load was successful, assign the data
        loadSuccess: function (data) {
            if (data.data) {
                app.apiDefinitions = data.data; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadFailure: function () {
            alert('Failed to retrieve existing api definitions list')
        },
    }
});

app.load();