var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.api =
    {
        call: function (url, method, request, successCallBack, failureCallBack) {

            // Start the progress spinner
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
