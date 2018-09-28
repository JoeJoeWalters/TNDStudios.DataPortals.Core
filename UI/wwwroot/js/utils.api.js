var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.api =
    {
        call: function (url, method, request, successCallBack, failureCallBack) {

            $.ajax({
                type: method,
                url: url,
                data: request,
                crossDomain: false,
                dataType: 'json',
                success: function (data, status, jqXHR) {
                    if (data.success)
                        successCallBack(data);
                    else
                        failureCallBack();
                },
                error: function (jgXHR, status) {
                    failureCallBack();
                }
            });
        }
    };
