var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.ui =
    {
        // http://bootstrap-notify.remabledesigns.com/
        notify: function (alertType, value) {

            var alertTranslated = 'success'; // Default alert type

            // Check the incoming alert type
            if (alertType == 0) {
                // Danger
                alertTranslated = 'danger';
            }
            else if (alertType == 2) {
                // Warning, not a disaster
                alertTranslated = 'warning';
            }

            // Do the notify
            $.notify({ message: value }, { type: alertTranslated });
        }
    };
