var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.packages.page()
    },
    computed: {

        // Searchable list of the packages
        filteredPackages() {
            return this.page.packages.filter(function (item) {
                return item.value.toLowerCase().indexOf(app.page.searchCriteria.toLowerCase()) > -1
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
        newPackage: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editorItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editorItem) {

            // Copy the data to the package editor from the selected object
            //app.page.editor.fromObject(editorItem);
            app.page.packageId = editorItem.key; // Set the global key that the common page uses so links can be enabled
            app.page.editorItem = editorItem; // Reference to the origional item being edited
            app.loadPackageComponents(); // Load the dependant items (that are not directly part of the model)
        },

        // Delete the current package
        deletePackage: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real package just in case
            // someone clicked the delete before it was saved
            if (idString != "") {

                tndStudios.utils.api.call(
                    '/api/' + app.page.packageId + '/data/packages/' + idString,
                    'DELETE',
                    null,
                    app.deleteCallback
                );
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Delete callback
        deleteCallback: function (success, data) {

            if (success) {

                // Clear the editor (as it was showing the package when it was deleted)
                app.page.editor.clear();

                // Remove the item from the packages list
                app.page.packages = app.page.packages.filter(
                    function (package) {
                        return package.id !== app.page.editorItem.id;
                    });

                app.page.editorItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Package Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Package Deletion Failed");
        },

        // Save the contents of the editor object 
        savePackage: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to save the package
                tndStudios.models.packages.save(
                    app.page.packageId,
                    app.page.editor.toObject(),
                    app.saveCallback
                );
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
                        app.page.editorItem = new tndStudios.models.common.commonObject(data.data);
                        app.page.packages.push(app.page.editorItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Package Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Package Could Not Be Saved");
        },

        // Start the load process for this package
        loadPackageComponents: function () {

            // Load the API Definitions for this package
            tndStudios.models.apiDefinitions.list(app.page.editorItem.key, null, this.loadApiDefinitionsCallback);

            // Load the Data Connections for this package
            tndStudios.models.dataConnections.list(app.page.editorItem.key, null, this.loadConnectionsCallback);

            // Load the Data Definitions for this package
            tndStudios.models.dataDefinitions.list(app.page.editorItem.key, null, this.loadDataDefinitionsCallback);

            // Load the Transformations for this package
            tndStudios.models.transformations.list(app.page.editorItem.key, null, this.loadTransformationsCallback);

            // Load the Credentials for this package
            tndStudios.models.credentials.list(app.page.editorItem.key, null, this.loadCredentialsStoreCallback);
        },

        // Callback for when the API Definitions are loaded
        loadApiDefinitionsCallback: function (success, data) {
            if (success && data.data) {
                app.page.apiDefinitions = []; // clear the api definitions array
                data.data.forEach(function (apiDefinition) {
                    app.page.apiDefinitions.push(new tndStudios.models.common.commonObject(apiDefinition)); // Assign the Json package to the api definition
                });
            }
        },

        // Callback for when the Connections are loaded
        loadConnectionsCallback: function (success, data) {
            if (success && data.data) {
                app.page.connections = []; // clear the connections array
                data.data.forEach(function (connection) {
                    app.page.connections.push(new tndStudios.models.common.commonObject(connection)); // Assign the Json package to the connection
                });
            }
        },

        // Callback for when the Data Definitions are loaded
        loadDataDefinitionsCallback: function (success, data) {
            if (success && data.data) {
                app.page.dataDefinitions = []; // clear the data definitions array
                data.data.forEach(function (dataDefinition) {
                    app.page.dataDefinitions.push(new tndStudios.models.common.commonObject(dataDefinition)); // Assign the Json package to the data definition
                });
            }
        },

        // Callback for when the Transformations are loaded
        loadTransformationsCallback: function (success, data) {
            if (success && data.data) {
                app.page.transformations = []; // clear the transformations array
                data.data.forEach(function (transformation) {
                    app.page.transformations.push(new tndStudios.models.common.commonObject(transformation)); // Assign the Json package to the transformation
                });
            }
        },

        // Callback for when the Credentials are loaded
        loadCredentialsStoreCallback: function (success, data) {
            if (success && data.data) {
                app.page.credentialsStore = []; // clear the credentials array
                data.data.forEach(function (credentials) {
                    app.page.credentialsStore.push(new tndStudios.models.credentials.credentials(credentials)); // Assign the Json package to the credentials
                });
            }
        },

        // Calculate a link from a base url
        calculatedLink: function (link, id) {
            return tndStudios.models.common.calculatedLink(link, this.page.packageId, id);
        },

        // Load the list of available packages
        loadPackages: function () {
            tndStudios.models.packages.list('', this.loadPackagesCallback);
        },

        // Load callback, assign the data
        loadPackagesCallback: function (success, data) {
            if (success, data.data) {
                app.page.packages = data.data; // Assign the Json package to the packages list
            };
        },
        
        // Load a package at the start if one is specified
        // Can't use the standard one as that searches for the item in the form
        loadAtStart: function () {
            if (app.page.packageId != '00000000-0000-0000-0000-000000000000') {
                app.edit({key: app.page.packageId, value: ''})
            }
        },
    }
});

// Create the validation rules for the main editor form
var validator = $("#editorForm").validate(
    {
        rules:
        {
            name: { required: true }
        },
        messages:
        {
            name: "Name Of The Package Is Required"
        }
    });

// Start the load process to initialise the form
app.loadPackages();

// Check to see if a package has been asked to be loaded from the Url on first load
app.loadAtStart();