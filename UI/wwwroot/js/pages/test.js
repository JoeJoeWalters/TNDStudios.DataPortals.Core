Vue.component('item-header',
    {
        methods:
        {
            remove: function () {
                this.$props["removeclick"](this);
            }
        },
        props: ['data', 'removeclick'],
        template: '<div v-on:click="remove" v-bind:title="data.description">{{ data.name }}</div>'
    });

var app = new Vue({
    el: '#test',
    data: {
        definition:
        {
        }
    },
    computed: {
        computedExample: function () {
            return "";
        }
    },
    methods: {
        remove: function (toRemove) {
            app.definition.itemProperties.splice(
                app.definition.itemProperties.indexOf(toRemove)
            );
        },
        load: function () {
            tndStudios.utils.api.call(
                'api/test/definition',
                'GET',
                {},
                app.loadSuccess,
                app.loadFailure);
        },
        loadSuccess: function (data) {
            alert(JSON.stringify(data));
        },
        loadFailure: function () {
            alert('Failure')
        }
    }
});