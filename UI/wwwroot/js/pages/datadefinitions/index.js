var app = new Vue({
    el: '#contentcontainer',
    data: {
        dataDefinitions: [],
        dataDefinition: new tndStudios.models.dataDefinitions.dataItemDefinition()
    },
    computed: {
    },
    methods: {

        // Add a new item to the definition list
        add: function () {

            // Push the new item
            app.dataDefinitions.push(new tndStudios.models.dataDefinitions.dataItemDefinition());
        },

        load: function () {
            tndStudios.utils.api.call(
                '/api/data/definition',
                'GET',
                null,
                app.loadSuccess,
                app.loadFailure);
        },

        // Load was successful, assign the data
        loadSuccess: function (data) {
            if (data.data) {
                app.dataDefinitions = data.data; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadFailure: function () {
            alert('Failed to retrieve existing data definitions list')
        },
    }
});

app.load();