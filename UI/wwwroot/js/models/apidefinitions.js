var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.apiDefinitions =
    {
        // Data Connection Model
        apiDefinition: function () {

            this.id = '';
            this.name = '';
            this.description = '';
            this.dataDefinition = new tndStudios.models.common.keyValuePair();
            this.dataConnection = new tndStudios.models.common.keyValuePair();

        },
    };