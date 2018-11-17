var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.dataDefinitions.page()
    },
    computed: {

        // Searchable list of the data definitions
        filteredDataDefinitions() {
            return this.page.dataDefinitions.filter(function (item) {
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
        newDataDefinition: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editorItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editorItem) {

            // Copy the data to the data definition editor from the selected object
            app.page.editor.fromObject(editorItem);
            app.page.editorItem = editorItem; // Reference to the origional item being edited
        },

        // Delete the current data definition
        deleteDataDefinition: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real data definition just in case
            // someone clicked the delete before it was saved
            if (idString != "") {

                tndStudios.utils.api.call(
                    '/api/data/definition/' + idString,
                    'DELETE',
                    null,
                    app.deleteSuccess,
                    app.deleteFailure
                );
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete An Item That Is Not Saved Yet');

        },

        // Test was successful
        deleteSuccess: function (data) {

            // Clear the editor (as it was showing the data definition when it was deleted)
            app.page.editor.clear();

            // Remove the item from the data definitions list
            app.page.dataDefinitions = app.page.dataDefinitions.filter(
                function (dataDefinition) {
                    return dataDefinition.id !== app.page.editorItem.id;
                });

            app.page.editorItem = null; // No longer attached to an editing object

            // Notify the user
            tndStudios.utils.ui.notify(1, "Data Definition Deleted Successfully");
        },

        // Test failed
        deleteFailure: function () {

            // Notify the user
            tndStudios.utils.ui.notify(0, "Data Definition Deletion Failed");
        },


        // Save the contents of the editor object 
        saveDataDefinition: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to save the data definition
                tndStudios.utils.api.call(
                    '/api/data/definition',
                    'POST',
                    app.page.editor.toObject(),
                    app.saveSuccess,
                    app.saveFailure
                );
            }

        },

        // Save was successful, assign the appropriate items
        saveSuccess: function (data) {

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
                    app.page.editorItem = new tndStudios.models.dataDefinitions.dataItemDefinition(data.data);
                    app.page.dataDefinitions.push(app.page.editorItem);
                }

                // Notify the user
                tndStudios.utils.ui.notify(1, "Data Definition Saved ('" + data.data.name + "')");
            };
        },

        // Save was unsuccessful, inform the user
        saveFailure: function () {

            // Notify the user
            tndStudios.utils.ui.notify(0, "Data Definition Could Not Be Saved");
        },
        
        // Start the load process
        load: function () {
            
            // Start loading the datae definitions list
            app.loadDataDefinitions();
            
            // Start loading the connections list
            app.loadConnections();

            // Start loading the connections
            app.loadLookups();
        },

        // Load the lookup codes from the server
        loadLookups: function () {

            // Get the lookups
            tndStudios.utils.api.lookup(
                tndStudios.utils.api.lookupTypes.Encoding,
                this.loadEncodingLookup);

            tndStudios.utils.api.lookup(
                tndStudios.utils.api.lookupTypes.Culture,
                this.loadCultureLookup);

            tndStudios.utils.api.lookup(
                tndStudios.utils.api.lookupTypes.DataTypes,
                this.loadDataTypesLookup);

            tndStudios.utils.api.lookup(
                tndStudios.utils.api.lookupTypes.DataPropertyTypes,
                this.loadDataPropertyTypesLookup);
        },

        // Load was successful, assign the data
        loadEncodingLookup: function (data) {
            if (data.data) {
                app.page.encodingLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadCultureLookup: function (data) {
            if (data.data) {
                app.page.cultureLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadDataPropertyTypesLookup: function (data) {
            if (data.data) {
                app.page.dataPropertyTypesLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadDataTypesLookup: function (data) {
            if (data.data) {
                app.page.dataTypesLookup = data.data;
            }
        },

        // load data definitions from the server
        loadDataDefinitions: function () {

            // Start the api call to load the data definitions
            tndStudios.utils.api.call(
                '/api/data/definition',
                'GET',
                null,
                app.loadDataDefinitionsSuccess,
                app.loadDataDefinitionsFailure);
        },

        // Load was successful, assign the data
        loadDataDefinitionsSuccess: function (data) {
            if (data.data) {

                app.page.dataDefinitions = []; // clear the data definitions array

                // Add the data definition objects back in with wrapper for additional functions
                data.data.forEach(function (dataDefinition) {
                    app.page.dataDefinitions.push(new tndStudios.models.dataDefinitions.dataItemDefinition(dataDefinition)); // Assign the Json package to the data definition
                });
            };
        },

        // Load was unsuccessful, inform the user
        loadDataDefinitionsFailure: function () {
        },

        // Add a new property item to the property editor
        addProperty: function () {

            // Generate a random number for the column name
            var randomNumber = Math.floor(Math.random() * 100);

            // Generate the new property
            var newItemProperty = new tndStudios.models.dataDefinitions.dataItemProperty(null);

            // Assign some default values
            newItemProperty.name = 'Property ' + randomNumber.toString();

            // Add the property to the list of properties for this editing item
            app.page.editor.itemProperties.push(newItemProperty);
        },

        // Remove a property from the property editor
        removeProperty: function (item) {
            
            // Remove the item from the property list
            app.page.editor.itemProperties = app.page.editor.itemProperties.filter(
                function (property) {
                    return property !== item;
                });
        },

        // Start editing a property using the property editor modal
        editProperty: function (item) {

            // Set the property that is currently being edited
            app.page.propertyEditorItem = item;

            // Assign the values of the item to the actual editor
            app.page.propertyEditor.fromObject(item);

            // Kick off the modal box
            $('#exampleModal').modal('show');
        },

        // Save the property that is being edited
        saveProperty: function () {
            
            // Notify the user
            tndStudios.utils.ui.notify(1, "Property Saved");
        },

        // Load a list of connections for this package that can be used
        // to analyse the connections and build the data definition
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
                data.data.forEach(function (connection) {
                    app.page.connections.push(new tndStudios.models.dataConnections.dataConnection(connection)); // Assign the Json package to the data definition
                });
            };
        },

        // Load was unsuccessful, inform the user
        loadConnectionsFailure: function () {
            //alert('Failed to retrieve existing connections list')
        },

        // Load data from the selected connection using the editor definition
        sampleConnection: function () {

            var connectionId = app.page.selectedConnection;
            if (app.page.selectedConnection != null &&
                app.page.selectedConnection != "") {

                // The the api call to sample the connection with this definition
                tndStudios.utils.api.call(
                    '/api/data/connection/sample/' + connectionId,
                    'POST',
                    app.page.editor.toObject(),
                    app.sampleConnectionSuccess,
                    app.sampleConnectionFailure
                );
            }
            else {
                tndStudios.utils.ui.notify(0, "No connection selected to sample");
            }
        },

        // Sample was successful, get the data from the result
        sampleConnectionSuccess: function (data) {

            // Got some data?
            if (data.data) {

                // Assign the data payload to the appropriate object in the editor
                app.page.editorValues = data.data.values;
            };
        },

        // Sample was unsuccessful, inform the user
        sampleConnectionFailure: function () {
        },

        // Analyse the data connection to get the structure and then tell
        // the system to start loading data from that connection
        analyseConnection: function () {

            var connectionId = app.page.selectedConnection;
            if (app.page.selectedConnection != null &&
                app.page.selectedConnection != "") {

                // The the api call to save the data definition
                tndStudios.utils.api.call(
                    '/api/data/connection/analyse/' + connectionId,
                    'GET',
                    null,
                    app.analyseConnectionSuccess,
                    app.analyseConnectionFailure
                );
            }
            else {
                tndStudios.utils.ui.notify(0, "No connection selected to analyse");
            }
        },

        // Analysis was successful, get the definition from the result
        analyseConnectionSuccess: function (data) {

            // Got some data?
            if (data.data) {

                var oldObject = app.page.editor.toObject(); // Copy the old object first

                app.page.editor.fromObject(data.data.definition); // Copy the new object in

                // Re-assign the id, name and description field which we want to keep
                app.page.editor.id = oldObject.Id;
                app.page.editor.name = oldObject.Name;
                app.page.editor.description = oldObject.Description;
            };
        },

        // Analysis was unsuccessful, inform the user
        analyseConnectionFailure: function () {
        },
    }
});

// Create the validation rules for the main editor form
var validator = $("#editorForm").validate(
    {
        rules:
        {
            name: { required: true },
            culture: { required: true },
            encodingFormat: { required: true }
        },
        messages:
        {
            name: "Name Of Data Definition Required",
            culture: "The Culture Type Is Required",
            encodingFormat: "The Encoding Format Is Required"
        }
    });

// Start the load process to initialise the form
app.load();