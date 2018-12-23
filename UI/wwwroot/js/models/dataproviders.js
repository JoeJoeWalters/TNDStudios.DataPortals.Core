var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataProviders =
    {
        // Data Provider Model
        dataProvider: function (data) {

            // The properties of the object
            this.canRead = false;
            this.canWrite = false;
            this.canAnalyse = false;
            this.canList = false;
            this.propertyBagTypes = [];

            // Copy the content of this data provider from another data provider
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.canRead = fromObject.canRead;
                this.canWrite = fromObject.canWrite;
                this.canAnalyse = fromObject.canAnalyse;
                this.canList = fromObject.canList;
                this.propertyBagTypes = fromObject.propertyBagTypes;
            }

            // Clear this data provider object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.canRead = false;
                this.canWrite = false;
                this.canAnalyse = false;
                this.canList = false;
                this.propertyBagTypes = [];
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // The the api call to load the provider data
        summary: function (providerId, callback) {

            tndStudios.utils.api.call(
                '/api/system/provider/' + providerId + '/summary',
                'GET',
                null,
                callback
            );
        },
    };