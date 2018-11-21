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

        },
        
    }
});

// Initialise the header model and load anything that it needs from the APIs
appHeader.load();