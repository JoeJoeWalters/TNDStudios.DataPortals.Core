var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.packages =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.packages = []; // The list of packages
            this.searchCriteria = ""; // The filter for the package list

            this.editor = new tndStudios.models.packages.package(null); // The editor object
            this.editorItem = null; // Reference to the item that is being edited for saving changes back to it

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Package Model
        package: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';

            // Copy the content of this package from another package
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

            // Clear this package object (i.e. make it ready for editing)
            this.clear = function () {

                // Clear the properties
                this.id = null;
                this.name = '';
                this.description = '';
            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },
    };