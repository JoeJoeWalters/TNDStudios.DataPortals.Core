var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.credentials =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.credentialsStore = []; // The list of credentials for this package
            this.searchCriteria = ""; // The filter for the credentials list

            this.editor = new tndStudios.models.credentials.credentials(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Credentials Model
        credentials: function (data) {

            // General Properties
            this.id = null;
            this.name = '';
            this.description = '';

            // Item properties
            this.properties = [];

            // Copy the content of the credentials from another credentials object
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;

                // Copy the item properties in from the source
                var current = this; // Store the context of "this"
                if (fromObject.properties) {
                    fromObject.properties.forEach(function (credential) {
                        current.properties.push(new tndStudios.models.credentials.credential(credential));
                    });
                }
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var toObjectProperties = [];
                if (this.properties) {
                    this.properties.forEach(function (credential) {
                        toObjectProperties.push(credential.toObject());
                    });
                };

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    properties: toObjectProperties
                };

                return result;
            }

            // Clear this credentials object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
                this.properties = [];
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Load the list of available credentials
        list: function (packageId, filter, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/credentials',
                'GET',
                null,
                callback);
        },

        // Get the full version of the credentials
        get: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/credentials/' + id,
                'GET',
                null,
                callback);
        },

        // The the api call to save the credentials
        delete: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/credentials/' + id,
                'DELETE',
                null,
                callback
            );
        },

        // The the api call to save the credentials
        save: function (packageId, saveObject, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/credentials',
                'POST',
                saveObject,
                callback
            );
        },

        // Individual Credential Model
        credential: function (data) {

            // Model Properties
            this.id = null;
            this.name = 'New Credential';
            this.description = 'Credential Description';
            this.value = '';
            this.encrypted = false;

            // Clear the property values
            this.clear = function () {

                this.id = null;
                this.name = 'New Credential';
                this.description = 'Credential Description';
                this.value = '';
                this.encrypted = false;
            };

            // Copy the content of this credential from another credential
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.value = fromObject.value;
                this.encrypted = fromObject.encrypted;
            };

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Name: this.name,
                    Description: this.description,
                    Value: this.value,
                    Encrypted: this.encrypted
                };

                return result;
            };

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            };
        },

        // Set of credentials to another object model
        credentialsLink: function (data) {

            // Model Properties
            this.credentials = new tndStudios.models.common.keyValuePair();
            this.canCreate = false;
            this.canRead = false;
            this.canUpdate = false;
            this.canDelete = false;
            this.filter = '';

            // Clear the property values
            this.clear = function () {

                this.credentials = new tndStudios.models.common.keyValuePair();
                this.canCreate = false;
                this.canRead = false;
                this.canUpdate = false;
                this.canDelete = false;
                this.filter = '';
            };

            // Copy the content of this credential from another credential
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.credentials = fromObject.credentials;
                this.canCreate = fromObject.canCreate;
                this.canRead = fromObject.canRead;
                this.canUpdate = fromObject.canUpdate;
                this.canDelete = fromObject.canDate;
                this.filter = fromObject.filter;
            };

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Credentials: this.credentials,
                    CanCreate: this.canCreate,
                    CanRead: this.canRead,
                    CanUpdate: this.canUpdate,
                    CanDelete: this.canDate,
                    Filter: this.filter
                };

                return result;
            };

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            };
        }
    };