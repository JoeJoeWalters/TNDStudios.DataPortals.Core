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
            app.page.editor.fromObject(editItem);
            app.page.editItem = editItem; // Reference to the origional item being edited
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
                    return dataDefinition.id !== app.page.editItem.id;
                });

            app.page.editItem = null; // No longer attached to an editing object

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
                if (app.page.editItem != null) {

                    // Update the existing item
                    app.page.editItem.fromObject(data.data);
                }
                else {

                    // Add the new item to the list
                    app.page.editItem = new tndStudios.models.dataDefinitions.dataItemDefinition(data.data);
                    app.page.dataDefinitions.push(app.page.editItem);
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
            var newItemProperty = new tndStudios.models.dataDefinitions.dataItemProperty();
            newItemProperty.name = 'Property ' + randomNumber.toString();

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

            $('#exampleModal').modal('show');
        }
    }
});

// Create the validation rules for the main editor form
$("#editorForm").validate(
    {
        rules:
        {
            name: { required: true }
        },
        messages:
        {
            name: "Name Of Data Definition Required"
        }
    });

// Start the load process to initialise the form
app.load();