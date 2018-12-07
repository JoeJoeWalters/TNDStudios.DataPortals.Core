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

            app.page.editItem = editItem; // Reference to the origional item being edited
            this.loadConnection(editItem.id); // Load the data from the server
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
                    app.deleteCallback);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Delete callback
        deleteCallback: function (success, data) {

            if (success) {

                // Clear the editor (as it was showing the connection when it was deleted)
                app.page.editor.clear();

                // Remove the item from the connections list
                app.page.connections = app.page.connections.filter(
                    function (connection) {
                        return connection.id !== app.page.editItem.id;
                    });

                app.page.editItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Connection Deleted Successfully");
            }
            else
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
                    if (app.page.editItem != null) {

                        // Update the existing item
                        app.page.editItem.fromObject(data.data);
                    }
                    else {

                        // Add the new item to the list
                        app.page.editItem = new tndStudios.models.common.commonObject(data.data);
                        app.page.connections.push(app.page.editItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Connection Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Connection Could Not Be Saved");
        },

        // Test the connection of a given connection
        testConnection: function (testItem) {

            // The the api call to test the connection
            tndStudios.models.dataConnections.test(
                app.page.packageId,
                app.page.editor.toObject(),
                app.testCallback);
        },

        // Test was successful?
        testCallback: function (success, data) {

            if (success)
                tndStudios.utils.ui.notify(1, "Connection Tested Successfully");
            else
                tndStudios.utils.ui.notify(0, "Connection Test Failed");
        },

        // Start the load process
        load: function () {

            // Has an id been specified to load straight away?
            tndStudios.models.common.loadAtStart(this.loadConnection);
            
            // Load the provider types
            app.loadProviderTypes();
            
            // Start loading the credentials list
            app.loadCredentials();

            // Start loading the connections list
            app.loadConnections();
        },

        // The provider was changed (Get the property bag etc.)
        providerChanged: function () {
            tndStudios.models.propertyBag.getDefaults(
                tndStudios.models.common.objectTypes.Providers,
                app.page.editor.providerType,
                this.propertyBagCallback
            );
        },

        // When the property bag is loaded (or not, call this)
        propertyBagCallback: function(success, data) {
            if (success) {
                if (data.data) {

                    // Clear the property bag
                    app.page.editor.propertyBag = [];

                    // Loop the incoming defaults and create new property bag items
                    data.data.forEach(function (propertyBagDefault) {

                        // Create a new property bag item
                        var propertyBagItem = new tndStudios.models.propertyBag.propertyBagItem();

                        // Assign the new values
                        propertyBagItem.itemType = propertyBagDefault;
                        propertyBagItem.value = propertyBagDefault.defaultValue;

                        // Push the item to the list
                        app.page.editor.propertyBag.push(
                            new tndStudios.models.propertyBag.propertyBagItem(propertyBagItem)
                        ); 
                    });
                }
            }
        },

        // Load the available provider types
        loadProviderTypes: function () {

            // The the api call to load the provider types
            tndStudios.models.dataConnections.providers(
                app.page.packageId,
                app.loadProviderTypesCallback);
        },

        // Load was successful, assign the data
        loadProviderTypesCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.providerTypes = data.data; // Assign the Json package to the provider types
                };
            }
        },

        // Load a list of credentials for this package that can be used
        loadCredentials: function () {

            // Load the list of available credentials
            tndStudios.models.credentials.list(
                app.page.packageId,
                null,
                app.loadCredentialsCallback);
        },

        // Load callback, assign the data
        loadCredentialsCallback: function (success, data) {

            if (success) {

                if (data.data) {

                    app.page.credentialsStore = []; // clear the credentials array

                    // Add the credentials objects back in with wrapper for additional functions
                    data.data.forEach(function (credentials) {
                        app.page.credentialsStore.push(new tndStudios.models.credentials.credentials(credentials)); // Assign the Json package to the credentials listing
                    });
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Credentials List Could Not Be Loaded");
        },

        // load connections from the server
        loadConnections: function () {

            // Start the api call to load the connections
            tndStudios.models.dataConnections.list(
                app.page.packageId,
                null,
                app.loadConnectionsCallback);
        },

        // Load was successful, assign the data
        loadConnectionsCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.connections = []; // clear the connections array
                    data.data.forEach(function (connection) {
                        app.page.connections.push(new tndStudios.models.common.commonObject(connection)); // Assign the Json package to the list object
                    });
                }
            }
        },
        
        // load singular connection from the server (as the list is only the headers)
        loadConnection: function (id) {

            // Start the api call to load the connections
            tndStudios.models.dataConnections.get(
                app.page.packageId,
                id,
                app.loadConnectionCallback);
        },

        // Load was successful, assign the data to the editor
        loadConnectionCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.editor.fromObject(data.data);
                }
            }
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