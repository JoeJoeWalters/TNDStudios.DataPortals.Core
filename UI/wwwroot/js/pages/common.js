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
            return tndStudios.models.common.calculatedLink(link, this.page.packageId, 'Index');
        },

        // Start the load process
        load: function () {
            
        },

    }
});

// Initialise the header model and load anything that it needs from the APIs
appHeader.load();