var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.apiDefinitions =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.apiDefinitions = []; // The list of api definitions for this package
            this.searchCriteria = ""; // The filter for the api definitions list

            this.editor = new tndStudios.models.apiDefinitions.apiDefinition(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it

            this.dataDefinitions = []; // The list of data definitions in this package
            this.dataConnections = []; // The list of data connections in this package
            this.credentialsStore = []; // The list of credentials in this package

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Api Definition Model
        apiDefinition: function (data) {

            // General Properties
            this.id = null;
            this.name = '';
            this.description = '';

            // Item properties
            this.dataConnection = new tndStudios.models.common.keyValuePair();
            this.dataDefinition = new tndStudios.models.common.keyValuePair();
            this.credentialsLinks = [];

            // Copy the content of this data item definition from another data item definition
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.dataConnection = fromObject.dataConnection;
                this.dataDefinition = fromObject.dataDefinition;

                // Copy the credential links in from the source
                this.credentialsLinks = [];
                var current = this; // Store the context of "this"
                if (fromObject.credentialsLinks) {
                    fromObject.credentialsLinks.forEach(function (credentialsLink) {
                        current.credentialsLinks.push(new tndStudios.models.credentials.credentialsLink(credentialsLink));
                    });
                }
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    DataConnection: this.dataConnection,
                    DataDefinition: this.dataDefinition,
                    CredentialsLinks: this.credentialsLinks
                };

                return result;
            }

            // Clear this definition object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
                this.dataConnection = new tndStudios.models.common.keyValuePair();
                this.dataDefinition = new tndStudios.models.common.keyValuePair();
                this.credentialsLinks = [];
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Call the delete endpoint
        delete: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/managedapi/definition/' + id,
                'DELETE',
                null,
                callback);
        },

        // Call the Save endpoint
        save: function (packageId, apiObject, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/managedapi/definition',
                'POST',
                apiObject,
                callback
            );
        },

        // Call the Save endpoint
        list: function (packageId, filter, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/managedapi/definition',
                'GET',
                null,
                callback);
        },

        // Get the full version of the api definition
        get: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/managedapi/definition/' + id,
                'GET',
                null,
                callback);
        },

        // Preview the api output
        preview: function (packageId, name, credentials) {
            window.open('/api/package/' + packageId + '/managedapi/objects/' + name, '_blank');
        }

    };