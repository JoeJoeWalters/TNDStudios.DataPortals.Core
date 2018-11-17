var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.api =
    {
        // Lookup enums
        lookupTypes: {
            Encoding: 1,
            Culture: 2,
            DataTypes: 3,
            DataPropertyTypes: 4
        },

        // Get the data for a lookup from the API controller
        lookup: function (lookupId, callback) {

            // Make the call to the lookup endpoint
            tndStudios.utils.api.call(
                '/api/system/lookup/' + lookupId,
                'GET',
                null,
                callback,
                tndStudios.utils.api.lookupFailure
            );
        },

        // Method to hit if the lookup codes fail
        lookupFailure: function () {
        },

        // Make an API call with the given parameters
        call: function (url, method, request, successCallBack, failureCallBack) {

            // Start the progress spinner in 1/4 of a second just so it
            // doesn't flash too much if the response is really quick
            tndStudios.utils.ui.progress(true);

            // Make the ajax call
            $.ajax({
                type: method,
                url: url,
                data: JSON.stringify(request),
                crossDomain: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data, status, jqXHR) {

                    // Stop the progress spinner
                    tndStudios.utils.ui.progress(false);

                    // Success?
                    if (data.success)
                        successCallBack(data);
                    else
                        failureCallBack();
                },
                error: function (jgXHR, status) {

                    // Stop the progress spinner
                    tndStudios.utils.ui.progress(false);

                    // Do the failure call
                    failureCallBack();
                }
            });
        }
    };
