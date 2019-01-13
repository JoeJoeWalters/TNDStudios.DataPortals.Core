var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataDefinitions =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.dataDefinitions = []; // The list of data definitions for this package
            this.searchCriteria = ""; // The filter for the data definitions list

            this.editor = new tndStudios.models.dataDefinitions.dataItemDefinition(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it
            this.editorValues = new tndStudios.models.dataDefinitions.dataItemValues(); // The editor values for when it is being analysed

            this.propertyEditor = new tndStudios.models.dataDefinitions.dataItemProperty(null); // The editor for the current property
            this.propertyeditItem = null; // Reference to the item that is being edited

            this.providerTypes = []; // The list of available provider types
            this.connections = []; // The list of data connections in this package
            this.selectedConnection = null; // The selected connection item

            this.cultureLookup = []; // Culture lookup codes
            this.encodingLookup = []; // Encoding lookup codes
            this.dataTypesLookup = []; // Data Types lookup codes
            this.dataKeyTypesLookup = []; // Data Key Types lookup codes
            this.dataPropertyTypesLookup = []; // Data Property types lookup codes

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Data Item Definition Model
        dataItemDefinition: function (data) {

            // General Properties
            this.id = null;
            this.name = '';
            this.description = '';

            // Item properties
            this.itemProperties = [];
            this.propertyBag = [];
            this.culture = '';
            this.encodingFormat = '';

            // Copy the content of this data item definition from another data item definition
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.id = fromObject.id;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.culture = fromObject.culture;
                this.encodingFormat = fromObject.encodingFormat;

                // Copy the item properties in from the source
                var current = this; // Store the context of "this"
                if (fromObject.itemProperties) {
                    fromObject.itemProperties.forEach(function (property) {
                        current.itemProperties.push(new tndStudios.models.dataDefinitions.dataItemProperty(property));
                    });
                }
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var toObjectItemProperties = [];
                if (this.itemProperties) {
                    this.itemProperties.forEach(function (property) {
                        toObjectItemProperties.push(property.toObject());
                    });
                };

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description,
                    Culture: this.culture,
                    EncodingFormat: this.encodingFormat,
                    ItemProperties: toObjectItemProperties
                };

                return result;
            }

            // Clear this connection object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
                this.itemProperties = [];
                this.propertyBag = [];
                this.culture = '';
                this.encodingFormat = '';
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Load the list of available connections
        list: function (packageId, filter, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/definition',
                'GET',
                null,
                callback);
        },

        // Get the full version of the definition
        get: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/definition/' + id,
                'GET',
                null,
                callback);
        },

        // The the api call to save the definition
        delete: function (packageId, id, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/definition/' + id,
                'DELETE',
                null,
                callback
            );
        },

        // The the api call to save the data definition
        save: function (packageId, saveObject, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId + '/data/definition',
                'POST',
                saveObject,
                callback
            );
        },

        // Data Item Property Model
        dataItemProperty: function (data) {

            // Model Properties
            this.dataType = 'System.String';
            this.propertyType = 0;
            this.key = 0;
            this.name = 'New Column';
            this.description = 'Column Description';
            this.path = '';
            this.ordinalPosition = -1;
            this.pattern = '';
            this.quoted = false;
            this.calculation = '';
            this.size = 0;
            this.providerGenerated = false;

            // Clear the property values
            this.clear = function () {

                this.dataType = 'System.String';
                this.propertyType = 0;
                this.key = 0;
                this.name = 'New Column';
                this.description = 'Column Description';
                this.path = '';
                this.ordinalPosition = -1;
                this.pattern = '';
                this.quoted = false;
                this.calculation = '';
                this.size = 0;
                this.providerGenerated = false;
            };

            // Copy the content of this data item property from another data item property
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.dataType = fromObject.dataType;
                this.propertyType = fromObject.propertyType;
                this.key = fromObject.key;
                this.name = fromObject.name;
                this.description = fromObject.description;
                this.path = fromObject.path;
                this.ordinalPosition = fromObject.ordinalPosition;
                this.pattern = fromObject.pattern;
                this.quoted = fromObject.quoted;
                this.calculation = fromObject.calculation;
                this.size = fromObject.size;
                this.providerGenerated = fromObject.providerGenerated;
            };

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    DataType: this.dataType,
                    PropertyType: this.propertyType,
                    Key: this.key,
                    Name: this.name,
                    Description: this.description,
                    Path: this.path,
                    OrdinalPosition: this.ordinalPosition,
                    Pattern: this.pattern,
                    Quoted: this.quoted,
                    Calculation: this.calculation,
                    Size: this.size,
                    ProviderGenerated: this.providerGenerated
                };

                return result;
            };

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            };
        },

        dataItemValues: function () {

            // Model Properties
            this.lines = [];
        }
    };