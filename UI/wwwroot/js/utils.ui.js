// https://modeling-languages.com/javascript-drawing-libraries-diagrams/
// http://bootstrap-notify.remabledesigns.com/

var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.ui =
    {
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
