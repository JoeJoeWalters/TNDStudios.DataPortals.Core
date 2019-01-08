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
            app.page.editItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the data definition editor from the selected object
            app.page.editItem = editItem; // Reference to the origional item being edited
            this.loadDataDefinition(editItem.id); // Load the data from the server
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

                // The the api call to delete the definition
                tndStudios.models.dataDefinitions.delete(
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

                // Clear the editor (as it was showing the data definition when it was deleted)
                app.page.editor.clear();

                // Remove the item from the data definitions list
                app.page.dataDefinitions = app.page.dataDefinitions.filter(
                    function (dataDefinition) {
                        return dataDefinition.id !== app.page.editItem.id;
                    });

                app.page.editItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Data Definition Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Data Definition Deletion Failed");
        },

        // Save the contents of the editor object 
        saveDataDefinition: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to delete the definition
                tndStudios.models.dataDefinitions.save(
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
                        app.page.dataDefinitions.push(app.page.editItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Data Definition Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Data Definition Could Not Be Saved");
        },

        // Start the load process
        load: function () {

            // Has an id been specified to load straight away?
            tndStudios.models.common.loadAtStart(this.loadDataDefinition);

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
        loadEncodingLookup: function (success, data) {
            if (success && data.data) {
                app.page.encodingLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadCultureLookup: function (success, data) {
            if (success && data.data) {
                app.page.cultureLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadDataPropertyTypesLookup: function (success, data) {
            if (success && data.data) {
                app.page.dataPropertyTypesLookup = data.data;
            }
        },

        // Load was successful, assign the data
        loadDataTypesLookup: function (success, data) {
            if (success, data.data) {
                app.page.dataTypesLookup = data.data;
            }
        },

        // load data definitions from the server
        loadDataDefinitions: function () {

            // Start the api call to load the data definitions
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

                    // Add the data definition objects back in with wrapper for additional functions
                    data.data.forEach(function (dataDefinition) {
                        app.page.dataDefinitions.push(new tndStudios.models.common.commonObject(dataDefinition)); // Assign the Json package to the data definition
                    });
                };
            }
        },

        // load data definition from the server
        loadDataDefinition: function (id) {

            // Start the api call to load the data definition
            tndStudios.models.dataDefinitions.get(
                app.page.packageId,
                id,
                app.loadDataDefinitionCallback);
        },

        // Load callback, assign the data
        loadDataDefinitionCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.editor.fromObject(data.data);
                }
            }
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
            app.page.propertyeditItem = item;

            // Assign the values of the item to the actual editor
            app.page.propertyEditor.fromObject(item);

            // Kick off the modal box
            $('#exampleModal').modal('show');
        },

        // Save the property that is being edited
        saveProperty: function () {

            // Copy the data back in from the editor
            app.page.propertyeditItem.fromObject(app.page.propertyEditor);

            // Notify the user
            tndStudios.utils.ui.notify(1, "Property Saved");
        },

        // Load a list of connections for this package that can be used
        // to analyse the connections and build the data definition
        loadConnections: function () {

            // Start the api call to load the connections
            tndStudios.models.dataConnections.list(
                app.page.packageId,
                null,
                app.loadConnectionsCallback);
        },

        // Load callback, assign the data
        loadConnectionsCallback: function (success, data) {
            if (success) {
                if (data.data) {

                    app.page.connections = []; // clear the connections array

                    // Add the connection objects back in with wrapper for additional functions
                    data.data.forEach(function (connection) {
                        app.page.connections.push(new tndStudios.models.common.commonObject(connection)); // Assign the Json package to the data definition
                    });
                };
            }
        },

        // Load data from the selected connection using the editor definition
        sampleConnection: function () {

            var connectionId = app.page.selectedConnection;
            if (app.page.selectedConnection != null &&
                app.page.selectedConnection != "") {

                // The the api call to sample the connection with this definition
                tndStudios.models.dataConnections.sample(
                    app.page.packageId,
                    connectionId,
                    app.page.editor.toObject(),
                    app.sampleConnectionCallback);
            }
            else {
                tndStudios.utils.ui.notify(0, "No connection selected to sample");
            }
        },

        // Sample callback, get the data from the result
        sampleConnectionCallback: function (success, data) {

            if (success) {
                if (data.data) {
                    // Assign the data payload to the appropriate object in the editor
                    app.page.editorValues = data.data.values;
                };
            }
            else {
                // Loop the messages
                data.messages.forEach(function (message) {
                    tndStudios.utils.ui.notify(0, message);
                });
            }
        },

        // Analyse the data connection to get the structure and then tell
        // the system to start loading data from that connection
        analyseConnection: function () {

            var connectionId = app.page.selectedConnection;
            if (app.page.selectedConnection != null &&
                app.page.selectedConnection != "") {

                // The the api call to save the data definition
                tndStudios.models.dataConnections.analyse(
                    app.page.packageId,
                    connectionId,
                    app.analyseConnectionCallback);
            }
            else {
                tndStudios.utils.ui.notify(0, "No connection selected to analyse");
            }
        },

        // Analysis callback, get the definition from the result
        analyseConnectionCallback: function (success, data) {

            if (success) {

                // Got some data?
                if (data.data) {

                    var oldObject = app.page.editor.toObject(); // Copy the old object first

                    app.page.editor.fromObject(data.data.definition); // Copy the new object in

                    // Re-assign the id, name and description field which we want to keep
                    app.page.editor.id = oldObject.Id;
                    app.page.editor.name = oldObject.Name;
                    app.page.editor.description = oldObject.Description;
                };
            }
            else
            {
                // Loop the messages
                data.messages.forEach(function (message)
                {
                    tndStudios.utils.ui.notify(0, message);
                });
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