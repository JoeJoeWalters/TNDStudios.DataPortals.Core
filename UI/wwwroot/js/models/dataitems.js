var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.dataItems =
    {
        // Data Item Definition Model
        dataItemDefinition: function () {

            // System items
            this.dirty = false;

            // Model Properties
            this.itemProperties = [];
            this.propertyBag = [];
            this.culture = '';
            this.encodingFormat = '';
            this.connections = [];
        },

        // Data Item Property Model
        dataItemProperty: function () {

            // System Items
            this.removed = false;
            this.dirty = false;

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