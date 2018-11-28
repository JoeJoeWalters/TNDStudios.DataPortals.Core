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
    }

