var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.propertyBag =
    {
        // Property Bag Item Type Model
        propertyBagItemType: function (data) {

            // The properties of the object
            this.propertyType = 0;
            this.dataType = "String";
            this.defaultValue = null;

            // Copy the content of this property bag type from another property bag type
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.propertyType = fromObject.propertyType;
                this.dataType = fromObject.dataType;
                this.defaultValue = fromObject.defaultValue;
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    PropertyType: this.propertyType,
                    DataType: this.dataType,
                    DefaultValue: this.defaultValue
                };

                return result;
            }

            // Clear this property bag type object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.propertyType = 0;
                this.dataType = "String";
                this.defaultValue = null;
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },

        // Property Bag Item Model
        propertyBagItem: function (data) {

            // The properties of the object
            this.value = null;
            this.itemType = new tndStudios.models.propertyBag.propertyBagItemType();

            // Copy the content of this property bag item from another property bag item
            // e.g. when editing in a secondary editor object
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.value = fromObject.value;
                this.itemType = fromObject.itemType;
            }

            // Create a formatted object that can be passed to the server
            this.toObject = function () {

                var result =
                {
                    Value: this.value,
                    ItemType: this.itemType
                };

                return result;
            }

            // Clear this property bag item object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.value = null;
                this.itemType = new tndStudios.models.propertyBag.propertyBagItemType();
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        }

    }