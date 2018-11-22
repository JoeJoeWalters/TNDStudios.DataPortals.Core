var appHeader = new Vue({
    el: '#headercontainer',
    data: {
        page: new tndStudios.models.common.header()
    },
    computed: {

        // Is a package selected?
        isPackageSelected() {
            return (this.page.selectedPackage.key != '');
        },

    },
    methods: {

        // Calculate a link from a base url
        calculatedLink: function (link) {
            if (link != null) {
                return this.resolveUrl(link.replace("{packageId}", this.page.selectedPackage.key));
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
                '/api/packages',
                'GET',
                null,
                appHeader.loadPackagesSuccess,
                appHeader.loadPackagesFailure
            );
        },

        // Load was successful, assign the data
        loadPackagesSuccess: function (data) {
            if (data.data) {
                appHeader.page.packages = data.data; // Assign the Json package to the packages list
            };
        },

        // Load was unsuccessful, inform the user
        loadPackagesFailure: function () {
            //alert('Failed to retrieve the list of provider types')
        },

    }
});

// Initialise the header model and load anything that it needs from the APIs
appHeader.load();