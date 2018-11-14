﻿var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.transformations =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object
            this.transformations = []; // The list of transformations for this package
            this.searchCriteria = ""; // The filter for the transformations list

            this.editor = new tndStudios.models.transformations.transformation(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it
        },

        // Transformation Model
        transformation: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';

            // Copy the content of this transformations from another transformations
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

            // Clear this transformation object (i.e. make it ready for editing)
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