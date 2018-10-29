var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataConnections =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.connections = []; // The list of connections for this package
            this.connection = new tndStudios.models.dataConnections.dataConnection(); // The editor object
            this.providerTypes = []; // The list of available provider types
        },

        // Data Connection Model
        dataConnection: function () {

            // The properties of the object
            this.id = '';
            this.name = '';
            this.description = '';
            this.providerType = 0;
            this.connectionString = '';
            this.definitions = [];

            // Copy the content of this connection from another connection
            // e.g. when editing in a secondary editor object
            this.copyFrom = function (fromObject) {
                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.providerType = fromObject.providerType;
                this.connectionString = fromObject.connectionString;

                this.definitions = [];
            }

            // Clear this connection object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = "";
                this.name = "";
                this.description = "";
                this.providerType = 0;
                this.connectionString = "";

                // Clear the lists
                this.definitions = [];
            }

            // Create a formatted object that can be passed to the server
            this.paramObject = function () {
                
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
        },
    };