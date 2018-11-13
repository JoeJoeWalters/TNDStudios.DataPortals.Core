// https://modeling-languages.com/javascript-drawing-libraries-diagrams/
// http://bootstrap-notify.remabledesigns.com/

var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.ui =
    {
        // Notify the user of alerts / messages
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
        },

        // Inform the user that there is some progress going on
        progress: function (on) {
            
            var progressSection = $("#progressBar");

            // Switching on or off?
            if (on) {
                progressSection.removeClass("invisible");
                progressSection.removeClass("d-none");
                progressSection.addClass("visible");
                progressSection.addClass("d-inline");
            }
            else {
                progressSection.addClass("invisible");
                progressSection.addClass("d-none");
                progressSection.removeClass("visible");
                progressSection.removeClass("d-inline");
            }

        }

    };
