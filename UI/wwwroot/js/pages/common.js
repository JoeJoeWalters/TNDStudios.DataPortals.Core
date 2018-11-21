var appHeader = new Vue({
    el: '#headercontainer',
    data: {
        page: new tndStudios.models.common.header()
    },
    computed: {

    },
    methods: {

        // Start the load process
        load: function () {

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