var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.common =
    {
        // System Object Types
        objectTypes: {
            ApiDefinitions: 1,
            DataDefinitions: 2,
            Connections: 3,
            Credentials: 4,
            Transformations: 5,
            Providers: 6
        },

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

        // Calculate a link from a base url
        calculatedLink: function (link, packageId, id) {
            if (link != null) {
                return this.resolveUrl(
                    link.replace("{packageId}", packageId)
                        .replace("{id}", id)
                );
            }
            else
                return "#";
        },

        // Resolve the relative paths of a url
        resolveUrl: function (url) {
            if (url.indexOf("~/") == 0) {
                url = window.location.protocol + "//" + window.location.host + "/" + url.substring(2);
            }
            return url;
        },

        // Tell the page to check to see if an id has been passed in to start loading
        loadAtStart: function (callback) {
            var loadId = $("#loadId").val();
            if (loadId != undefined &&
                loadId != "" &&
                loadId != '00000000-0000-0000-0000-000000000000') {
                callback(loadId); // Call the callback signature
            }
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
