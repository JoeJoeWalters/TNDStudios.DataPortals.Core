var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataConnections =
    {
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