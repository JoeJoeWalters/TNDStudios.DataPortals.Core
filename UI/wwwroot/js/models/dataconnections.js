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
        },

        // Data Connection Model
        dataConnection: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';
            this.providerType = 0;
            this.connectionString = '';
            this.definitions = [];

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

                // Copy each definition in
                this.definitions = fromObject.definitions;
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    ProviderType: this.providerType,
                    ConnectionString: this.connectionString,
                    Definitions: this.definitions
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

                // Clear the lists
                this.definitions = [];
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },
    };