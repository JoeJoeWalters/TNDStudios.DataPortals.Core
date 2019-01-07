var tndStudios = tndStudios || {};
tndStudios.models = tndStudios.models || {};
tndStudios.models.packages =
    {
        // Edit Page Model
        page: function () {

            // The properties of the object            
            this.editor = new tndStudios.models.packages.package(null); // The editor object
            this.editItem = null; // Reference to the item that is being edited for saving changes back to it
            this.searchCriteria = ""; // The filter for the packages list

            // Attached items (connections, api definitions etc.)
            this.apiDefinitions = [];
            this.connections = [];
            this.dataDefinitions = [];
            this.transformations = [];
            this.credentialsStore = [];
            this.packages = [];

            // Diagram Drawing etc.
            var diagramValue = 'st=>start: Start:>Start[blank]\n';
            diagramValue += 'e=>end:>End\n';
            diagramValue += 'op1=>operation: My Operation\n';
            diagramValue += 'sub1=>subroutine: My Subroutine\n';
            diagramValue += 'cond=>condition: Yes\n';
            diagramValue += 'or No?:>Go Somewhere\n';
            diagramValue += 'io=>inputoutput: catch something...\n';
            diagramValue += 'para=>parallel: parallel tasks\n';

            diagramValue += 'st->op1->cond\n';
            diagramValue += 'cond(yes)->io->e\n';
            diagramValue += 'cond(no)->para\n';
            diagramValue += 'para(path1, bottom)->sub1(right)->op1\n';
            diagramValue += 'para(path2, top)->op1';

            this.diagramLogic = diagramValue;

            this.packageId = $("#packageId").val(); // Get the package Id from the field on the page
        },

        // Package Model
        package: function (data) {

            // The properties of the object
            this.id = null;
            this.name = '';
            this.description = '';

            // Copy the content of this package from another package
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

            // Clear this package object (i.e. make it ready for editing)
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

        // The the api call to save the package
        save: function (saveObject, callback) {
            tndStudios.utils.api.call(
                '/api/package',
                'POST',
                saveObject,
                callback);
        },

        // The api call to list the packages
        list: function (filter, callback) {

            // The the api call to load the provider types
            tndStudios.utils.api.call(
                '/api/package',
                'GET',
                null,
                callback
            );

        },

        // Get the full version of the package
        get: function (packageId, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId,
                'GET',
                null,
                callback);
        },

        // The api call to delete a package
        delete: function (packageId, callback) {
            tndStudios.utils.api.call(
                '/api/package/' + packageId,
                'DELETE',
                null,
                callback);
        }
    };