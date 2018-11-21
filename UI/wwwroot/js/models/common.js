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

            // The currently selected package
            this.selectedPackage = new tndStudios.models.common.keyValuePair();

        },
    }

