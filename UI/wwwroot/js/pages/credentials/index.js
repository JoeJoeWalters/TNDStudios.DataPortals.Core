var app = new Vue({
    el: '#contentcontainer',
    data: {
        page: new tndStudios.models.credentials.page()
    },
    computed: {

        // Searchable list of the data definitions
        filteredCredentials() {
            return this.page.credentialsStore.filter(function (item) {
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

        // Add a new item to the credentials list
        newCredentials: function () {

            // Clear the editor
            app.page.editor.clear();
            app.page.editItem = null; // No longer attached to an editing object
        },

        // Edit an existing item by assigning it to the editor object
        edit: function (editItem) {

            // Copy the data to the credentials editor from the selected object
            app.page.editItem = editItem; // Reference to the origional item being edited
            this.loadCredentials(editItem.id); // Load the data from the server
        },

        // Delete the current credentials
        deleteCredentials: function () {

            // Get the editor id field
            var idString = "";
            if (app.page.editor && app.page.editor.id) {
                idString = app.page.editor.id;
            }

            // Make sure this is a real credentials just in case
            // someone clicked the delete before it was saved
            if (idString != "") {

                // The the api call to delete the credentials
                tndStudios.models.credentials.delete(
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

                // Clear the editor (as it was showing the credentials when it was deleted)
                app.page.editor.clear();

                // Remove the item from the data definitions list
                app.page.credentialsStore = app.page.credentialsStore.filter(
                    function (credentials) {
                        return credentials.id !== app.page.editItem.id;
                    });

                app.page.editItem = null; // No longer attached to an editing object

                // Notify the user
                tndStudios.utils.ui.notify(1, "Credentials Deleted Successfully");
            }
            else
                tndStudios.utils.ui.notify(0, "Credentials Deletion Failed");
        },

        // Save the contents of the editor object 
        saveCredentials: function () {

            // Is the form valid?
            if ($("#editorForm").valid()) {

                // The the api call to save the credentials
                tndStudios.models.credentials.save(
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
                        app.page.credentialsStore.push(app.page.editItem);
                    }

                    // Notify the user
                    tndStudios.utils.ui.notify(1, "Credentials Saved ('" + data.data.name + "')");
                };
            }
            else
                tndStudios.utils.ui.notify(0, "Credentials Could Not Be Saved");
        },

        // Start the load process
        load: function () {

            // Has an id been specified to load straight away?
            tndStudios.models.common.loadAtStart(this.loadCredentials);

            // Start loading the credentials list
            app.loadCredentialsStore();
        },


        // load credentials list from the server
        loadCredentialsStore: function () {

            // Start the api call to load the credentials list
            tndStudios.models.credentials.list(
                app.page.packageId,
                null,
                app.loadCredentialsStoreCallback);
        },

        // Load callback, assign the data
        loadCredentialsStoreCallback: function (success, data) {
            if (success) {
                if (data.data) {

                    app.page.credentialsStore = []; // clear the credentials array

                    // Add the credentials objects back in with wrapper for additional functions
                    data.data.forEach(function (credentials) {
                        app.page.credentialsStore.push(new tndStudios.models.common.commonObject(credentials)); // Assign the Json package to the credentials list
                    });
                };
            }
        },

        // load credentials from the server
        loadCredentials: function (id) {

            // Start the api call to load the credentials
            tndStudios.models.credentials.get(
                app.page.packageId,
                id,
                app.loadCredentialsCallback);
        },

        // Load callback, assign the data
        loadCredentialsCallback: function (success, data) {
            if (success) {
                if (data.data) {
                    app.page.editor.fromObject(data.data);
                }
            }
        },

        // Add a new credential item to the credentials editor
        addCredential: function () {

            // Generate a random number for the credential name
            var randomNumber = Math.floor(Math.random() * 100);

            // Generate the new credential
            var newCredential = new tndStudios.models.credentials.credential(null);

            // Assign some default values
            newCredential.name = 'Credential ' + randomNumber.toString();

            // Add the property to the list of credentials for this editing item
            app.page.editor.properties.push(newCredential);
        },

        // Remove a credential from the credentials editor
        removeCredential: function (item) {

            // Remove the item from the property list
            app.page.editor.properties = app.page.editor.properties.filter(
                function (credential) {
                    return credential !== item;
                });
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
            name: "Name Of Credentials Required"
        }
    });

// Start the load process to initialise the form
app.load();