var appHeader = new Vue({
    el: '#headercontainer',
    data: {
        page: new tndStudios.models.common.header()
    },
    computed: {

        // Is a package selected?
        isPackageSelected() {
            return (this.page.packageId != undefined &&
                this.page.packageId != '00000000-0000-0000-0000-000000000000');
        },

    },
    methods: {

        // Calculate a link from a base url
        calculatedLink: function (link) {
            if (link != null) {
                return this.resolveUrl(link.replace("{packageId}", this.page.packageId));
            }
            else
                return "#";
        },

        // Go to the selected package link based on the url template
        goToSelectedPackage: function () {
            var link = $("#pageUrlPattern").val();
            if (link != undefined)
                window.location = this.resolveUrl("~/package/{packageId}".replace("{packageId}", this.page.selectedPackage.key));
        },

        // Resolve the relative paths of a url
        resolveUrl: function (url) {
            if (url.indexOf("~/") == 0) {
                url = window.location.protocol + "//" + window.location.host + "/" + url.substring(2);
            }
            return url;
        },

        // Start the load process
        load: function () {

            // Get the package Id from the page to initialise 
            // the selected package in the drop down
            this.page.selectedPackage.key = $("#packageId").val();

            // Get the list of available packages from the server
            appHeader.loadPackages();

        },

        // Load the list of available packages
        loadPackages: function () {

            // The the api call to load the provider types
            tndStudios.utils.api.call(
                '/api/package',
                'GET',
                null,
                appHeader.loadPackagesCallback
            );
        },

        // Load callback, assign the data
        loadPackagesCallback: function (success, data) {
            if (success, data.data) {
                appHeader.page.packages = data.data; // Assign the Json package to the packages list
            };
        },

    }
});

// Initialise the header model and load anything that it needs from the APIs
appHeader.load();