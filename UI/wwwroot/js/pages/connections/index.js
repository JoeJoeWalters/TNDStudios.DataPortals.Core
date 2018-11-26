var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.dataConnections.page()
    },
    computed: {

        // Searchable list of the connections
        filteredConnections() {
            return this.page.connections.filter(function (item) {
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
        newConnection: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the connection editor from the selected object
            app.page.editor.fromObject(editItem);
            app.page.editItem = editItem; // Reference to the origional item being edited
        },

        // Delete the current connection
        deleteConnection: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real connection just in case
            // someone clicked the delete before it was saved
            if (idString != "") {
                
                tndStudios.models.dataConnections.delete(
                    app.page.packageId,
                    idString,
                    app.deleteSuccess,
                    app.deleteFailure);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Test was successful
        deleteSuccess: function (data) {

            // Clear the editor (as it was showing the connection when it was deleted)
            app.page.editor.clear();

            // Remove the item from the connections list
            app.page.connections = app.page.connections.filter(
                function (connection)
                {
                    return connection.id !== app.page.editItem.id;
                });

            app.page.editItem = null; // No longer attached to an editing object

            // Notify the user
            tndStudios.utils.ui.notify(1, "Connection Deleted Successfully");
        },

        // Test failed
        deleteFailure: function () {

            // Notify the user
            tndStudios.utils.ui.notify(0, "Connection Deletion Failed");
        },


        // Save the contents of the editor object 
        saveConnection: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to save the connection
                tndStudios.models.dataConnections.save(
                    app.page.packageId,
                    app.page.editor.toObject(),
                    app.saveSuccess,
                    app.saveFailure);
            }

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
                    app.page.editItem = new tndStudios.models.dataConnections.dataConnection(data.data);
                    app.page.connections.push(app.page.editItem);
                }

                // Notify the user
                tndStudios.utils.ui.notify(1, "Connection Saved ('" + data.data.name + "')");
            };
        },

        // Save was unsuccessful, inform the user
        saveFailure: function () {

            // Notify the user
            tndStudios.utils.ui.notify(0, "Connection Could Not Be Saved");
        },

        // Test the connection of a given connection
        testConnection: function (testItem) {

            // The the api call to test the connection
            tndStudios.models.dataConnections.test(
                app.page.packageId,
                app.page.editor.toObject(),
                app.testSuccess,
                app.testFailure);
        },

        // Test was successful
        testSuccess: function (data) {

            // Notify the user
            tndStudios.utils.ui.notify(1, "Connection Tested Successfully");
        },

        // Test failed
        testFailure: function () {

            // Notify the user
            tndStudios.utils.ui.notify(0, "Connection Test Failed");
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
            tndStudios.models.dataConnections.providers(
                app.page.packageId,
                app.loadProviderTypesSuccess,
                app.loadProviderTypesFailure);
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
            tndStudios.models.dataConnections.list(
                app.page.packageId,
                null,
                app.loadConnectionsSuccess,
                app.loadConnectionsFailure);
        },

        // Load was successful, assign the data
        loadConnectionsSuccess: function (data) {
            if (data.data) {

                app.page.connections = []; // clear the connections array

                // Add the connection objects back in with wrapper for additional functions
                data.data.forEach(function (connection) {
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

// Create the validation rules for the main editor form
var validator = $("#editorForm").validate(
    {
        rules:
        {
            name: { required: true },
            connectionString: { required: true },
            providerType: {
                required: {
                    depends: function (element) {
                        return (element.value != '0');
                    }
                }
            }
        },
        messages:
        {
            name: "Name Of Connection Required",
            connectionString: "Connection String Required",
            providerType: "Please Select A Provider Type"
        }
    });

// Start the load process to initialise the form
app.load();