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
            this.editorItem = null; // Reference to the item that is being edited for saving changes back to it
            this.editorValues = new tndStudios.models.dataDefinitions.dataItemValues(); // The editor values for when it is being analysed

            this.propertyEditor = new tndStudios.models.dataDefinitions.dataItemProperty(null); // The editor for the current property
            this.propertyEditorItem = null; // Reference to the item that is being edited

            this.providerTypes = []; // The list of available provider types
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
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Id: this.id,
                    Name: this.name,
                    Description: this.description
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

        // Data Item Property Model
        dataItemProperty: function (data) {

            // Model Properties
            this.dataType = 'System.String';
            this.propertyType = 0;
            this.key = false;
            this.name = 'New Column';
            this.description = 'Column Description';
            this.path = '';
            this.ordinalPosition = -1;
            this.pattern = '';
            this.quoted = false;
            this.calculation = '';

            // Clear the property values
            this.clear = function () {

                this.dataType = 'System.String';
                this.propertyType = 0;
                this.key = false;
                this.name = 'New Column';
                this.description = 'Column Description';
                this.path = '';
                this.ordinalPosition = -1;
                this.pattern = '';
                this.quoted = false;
                this.calculation = '';
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
            };

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    DataType: fromObject.dataType,
                    PropertyType: fromObject.propertyType,
                    Key: fromObject.key,
                    Name: fromObject.name,
                    Description: fromObject.description,
                    Path: fromObject.path,
                    OrdinalPosition: fromObject.ordinalPosition,
                    Pattern: fromObject.pattern,
                    Quoted: fromObject.quoted,
                    Calculation: fromObject.calculation
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