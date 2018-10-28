var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataConnections =
    {
        // Edit Page Model
        page: function () {

            this.connections = [];
            this.connection = new tndStudios.models.dataConnections.dataConnection();
            this.providerTypes = [];
        },

        // Data Connection Model
        dataConnection: function () {

            this.id = '';
            this.name = '';
            this.description = '';
            this.providerType = 0;
            this.connectionString = '';
            this.definitions = [];

        },
    };