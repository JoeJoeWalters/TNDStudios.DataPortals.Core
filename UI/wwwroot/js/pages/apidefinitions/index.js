var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.apiDefinitions.page()
    },
    computed: {

        // Searchable list of the data definitions
        filteredApiDefinitions() {
            return this.page.apiDefinitions.filter(function (item) {
                return item.name.toLowerCase().indexOf(app.page.searchCriteria.toLowerCase()) > -1
            });
        },

        // Is the editor item saved?
        isSavedVisible() {

            if (this.page.editor.id != null &&
                this.page.editor.id != "")
                return "visible";
            else
                return "hidden";
        }
    },
    methods: {

        // Add a new item to the definition list
        newApiDefinition: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editorItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editorItem) {

            // Copy the data to the api definition editor from the selected object
            app.page.editor.fromObject(editorItem);
            app.page.editorItem = editorItem; // Reference to the origional item being edited
        },

        // Delete the current data definition
        deleteApiDefinition: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real api definition just in case
            // someone clicked the delete before it was saved
            if (idString != "") {
                
                // Call the delete function
                tndStudios.models.apiDefinitions.delete(
                    app.page.packageId,
                    idString,
                    app.deleteCallback);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Delete callback
        deleteCallback: function (success, data) {

            if (success) {

                // Clear the editor (as it was showing the api definition when it was deleted)
                app.page.editor.clear();

                // Remove the item from the api definitions list
                app.page.apiDefinitions = app.page.apiDefinitions.filter(
                    function (apiDefinition) {
                        return apiDefinition.id !== app.page.editorItem.id;
                    });

                app.page.editorItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Api Definition Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definition Deletion Failed");
        },

        // Save the contents of the editor object 
        saveApiDefinition: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // Call the Save function
                tndStudios.models.apiDefinitions.save(
                    app.page.packageId,
                    app.page.editor.toObject(),
                    app.saveCallback);
            }

        },

        // Save callback, assign the appropriate items
        saveCallback: function (success, data) {

            if (success) {

                // Some data came back?
                if (data.data) {

                    // Update the editor itself (possibily with new links or id if a new item)
                    app.page.editor.fromObject(data.data);

                    // Were we editing an existing item?
                    if (app.page.editorItem != null) {

                        // Update the existing item
                        app.page.editorItem.fromObject(data.data);
                    }
                    else {

                        // Add the new item to the list
                        app.page.editorItem = new tndStudios.models.apiDefinitions.apiDefinition(data.data);
                        app.page.apiDefinitions.push(app.page.editorItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Api Definition Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definition Could Not Be Saved");
        },

        // Start the load process
        load: function () {

            // Start loading the api definitions list
            app.loadApiDefinitions();

            // Start loading the connections list
            app.loadConnections();

            // Start loading the data definitions list
            app.loadDataDefinitions();
        },
        
        // load api definitions from the server
        loadApiDefinitions: function () {

            // Call the Save endpoint
            tndStudios.models.apiDefinitions.list(
                app.page.packageId,
                null,
                app.loadApiDefinitionsCallback);
        },

        // Load was successful, assign the data
        loadApiDefinitionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.apiDefinitions = []; // clear the api definitions array

                    // Add the api definition objects back in with wrapper for additional functions
                    data.data.forEach(function (apiDefinition) {
                        app.page.apiDefinitions.push(new tndStudios.models.apiDefinitions.apiDefinition(apiDefinition)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Api Definitions Could Not Be Loaded");
        },

        // Load a list of connections for this package that can be used
        loadConnections: function () {

            // Load the list of available connections
            tndStudios.models.dataConnections.list(
                app.page.packageId,
                null,
                app.loadConnectionsCallback);
        },

        // Load callback, assign the data
        loadConnectionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.dataConnections = []; // clear the connections array

                    // Add the connection objects back in with wrapper for additional functions
                    data.data.forEach(function (connection) {
                        app.page.dataConnections.push(new tndStudios.models.dataConnections.dataConnection(connection)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Connections Could Not Be Loaded");
        },

        // Load a list of data definitions for this package that can be used
        loadDataDefinitions: function () {
            
            // Load the list of available connections
            tndStudios.models.dataDefinitions.list(
                app.page.packageId,
                null,
                app.loadDataDefinitionsCallback);
        },

        // Load callback, assign the data
        loadDataDefinitionsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.dataDefinitions = []; // clear the data definitions array

                    // Add the data definitions objects back in with wrapper for additional functions
                    data.data.forEach(function (dataDefinition) {
                        app.page.dataDefinitions.push(new tndStudios.models.dataDefinitions.dataItemDefinition(dataDefinition)); // Assign the Json package to the data definition
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Data Definitions Could Not Be Loaded");
        },

    }
});

// Create the validation rules for the main editor form
var validator = $("#editorForm").validate(
    {
        rules:
        {
            name: { required: true },
            dataConnection: { required: true },
            dataDefinition: { required: true }
        },
        messages:
        {
            name: "Name Of Api Definition Required",
            dataConnection: "Please specify the connection to use",
            dataDefinition: "Please specify the data definition to use"
        }
    });

// Start the load process to initialise the form
app.load();