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
        dataItemProperty: function () {

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
        },

        dataItemValues: function () {

            // Model Properties
            this.lines = [];
        }
    };