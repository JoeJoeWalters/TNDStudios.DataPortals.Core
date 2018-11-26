var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataConnections =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.connections = []; // The list of connections for this package
            this.searchCriteria = ""; // The filter for the connections list

            this.editor = new tndStudios.models.dataConnections.dataConnection(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it
            this.providerTypes = []; // The list of available provider types

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Data Connection Model
        dataConnection: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';
            this.providerType = 0;
            this.connectionString = '';

            // Copy the content of this connection from another connection
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.providerType = fromObject.providerType;
                this.connectionString = fromObject.connectionString;
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    ProviderType: this.providerType,
                    ConnectionString: this.connectionString
                };

                return result;
            }

            // Clear this connection object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
                this.providerType = 0;
                this.connectionString = '';
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Load the list of available connections
        list: function (packageId, filter, success, failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection',
                'GET',
                null,
                success,
                failure);
        },

        // Delete an existing connection
        delete: function (packageId, id, success, failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + id,
                'DELETE',
                null,
                success,
                failure
            );
        },

        // The the api call to save the connection
        save: function (packageId, saveObject, success, failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection',
                'POST',
                saveObject,
                success,
                failure
            );
        },

        // Test the connection provided
        test: function (packageId, saveObject, success, failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/test',
                'POST',
                saveObject,
                success,
                failure
            );
        },

        // The the api call to sample the connection with this definition
        sample: function (
            packageId,
            connectionId,
            sampleObject,
            success,
            failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + connectionId + '/sample',
                'POST',
                sampleObject,
                success,
                failure
            );
        },

        // The the api call to save the data definition
        analyse: function (
            packageId,
            connectionId,
            success,
            failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/connection/' + connectionId + '/analyse',
                'GET',
                null,
                success,
                failure
            );
        },

        // The the api call to load the provider types
        providers: function (packageId, success, failure) {

            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/providers',
                'GET',
                null,
                success,
                failure
            );
        },

    };