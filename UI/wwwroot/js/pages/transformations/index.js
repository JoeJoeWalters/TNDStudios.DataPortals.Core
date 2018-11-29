var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.transformations.page()
    },
    computed: {

        // Searchable list of the data definitions
        filteredTransformations() {
            return this.page.transformations.filter(function (item) {
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
        newTransformation: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the transformation editor from the selected object
            app.page.editor.fromObject(editItem);
            app.page.editItem = editItem; // Reference to the origional item being edited
        },

        // Delete the current transformation
        deleteTransformation: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real transformation just in case
            // someone clicked the delete before it was saved
            if (idString != "") {

                // Call the delete function
                tndStudios.models.transformations.delete(
                    app.page.packageId,
                    idString,
                    app.deleteCallback);
            }
            else
                tndStudios.utils.ui.notify(0, 'Cannot Delete A Transformation That Is Not Saved Yet');

        },

        // Delete callback
        deleteCallback: function (success, data) {

            if (success) {

                // Clear the editor (as it was showing the transformation when it was deleted)
                app.page.editor.clear();

                // Remove the item from the transformation list
                app.page.transformations = app.page.transformations.filter(
                    function (transformation) {
                        return transformation.id !== app.page.editItem.id;
                    });

                app.page.editItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Transformation Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Transformation Deletion Failed");
        },

        // Save the contents of the editor object 
        saveTransformation: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to save the transformations
                tndStudios.models.transformations.save(
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
                        app.page.editItem = new tndStudios.models.transformations.transformation(data.data);
                        app.page.transformations.push(app.page.editItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Transformation Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Transformation Could Not Be Saved");
        },

        // Start the load process
        load: function () {

            // Start loading the transformations list
            app.loadTransformations();
        },

        // load transformations from the server
        loadTransformations: function () {

            // Start the api call to load the transformations
            tndStudios.models.transformations.list(
                app.page.packageId,
                null,
                app.loadTransformationsCallback);
        },

        // Load callback, assign the data
        loadTransformationsCallback: function (success, data) {
            if (success && data.data) {
                app.page.transformations = []; // clear the transformations array

                // Add the transformation objects back in with wrapper for additional functions
                data.data.forEach(function (transformation) {
                    app.page.transformations.push(new tndStudios.models.common.commonObject(transformation)); // Assign the Json package to the transformation
                });
            };
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
            name: "Name Of Transformations Required"
        }
    });

// Start the load process to initialise the form
app.load();