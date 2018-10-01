var app = new Vue({
    el: '#test',
    data: {
        definition: new tndStudios.models.dataItems.dataItemDefinition(),
        values: new tndStudios.models.dataItems.dataItemValues()
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
        loadDefinition: function () {
            tndStudios.utils.api.call(
                'api/test/definition',
                'GET',
                "String Value",
                app.loadDefinitionSuccess,
                app.loadDefinitionFailure);
        },

        // Load was successful, assign the data
        loadDefinitionSuccess: function (data) {
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
        loadDefinitionFailure: function () {
            alert('Failed to retrieve definition of the data')
        },

        loadData: function () {
            tndStudios.utils.api.call(
                'api/test/data',
                'POST',
                app.definition,
                app.loadDataSuccess,
                app.loadDataFailure);
        },

        // Load was successful, assign the data
        loadDataSuccess: function (data) {
            if (data.data) {
                app.values = data.data; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadDataFailure: function () {
            alert('Failed to retrieve value for the data')
        },

        loadAnalysis: function () {

            // Create some new form data
            var formData = new FormData();
            formData.append('upload', $('#upload')[0].files[0]); // myFile is the input type="file" control

            var _url = '/api/data/analyse/file';

            $.ajax({
                url: _url,
                type: 'POST',
                data: formData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (result) {
                },
                error: function (jqXHR) {
                    alert('Shit went sideways ..');
                },
                complete: function (jqXHR, status) {
                    app.loadAnalysisSuccess(jqXHR.responseJSON.data);
                }
            });
        },

        // Load was successful, assign the data
        loadAnalysisSuccess: function (data) {
            if (data) {
                app.definition = data.definition; // Assign the Json package to the data definition
                app.values = data.values; // Assign the Json package to the data definition
            };
        },

        // Load was unsuccessful, inform the user
        loadAnalysisFailure: function () {
            alert('Failed to retrieve value for the data');
        },
    }
});