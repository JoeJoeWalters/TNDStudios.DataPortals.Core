var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.common =
    {
        // Key Value Pair definition
        keyValuePair: function () {

            this.key = "";
            this.value = "";
        },

        // Page Header Model
        header: function () {

            // List of available packages
            this.packages = [];

            // The currently selected package (for the drop down)
            this.selectedPackage = new tndStudios.models.common.keyValuePair();

            // The actual package for the page itself
            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page

        },

        // Common Object Model (Base for all saveable items)
        commonObject: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';
            
            // Copy the content of this object from another object
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

            // Clear this object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            };
        },

    }

