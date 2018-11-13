// https://modeling-languages.com/javascript-drawing-libraries-diagrams/
// http://bootstrap-notify.remabledesigns.com/

var tndStudios = tndStudios || {};
tndStudios.utils = tndStudios.utils || {};
tndStudios.utils.ui =
    {
        notificationEvents: null,

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

            // Cancel all existing notification events
            tndStudios.utils.ui.cancelNotifyEvents();

            // Decide which function to call 
            if (on) {
                this.notificationEvents.push(
                    setTimeout(tndStudios.utils.ui.progressOn, 250)); // Show the progress bar on a delay
            }
            else
                tndStudios.utils.ui.progressOff(); // Hide the progress bar
        },

        // Function to set on the progress bar
        progressOn: function () {

            var progressSection = $("#progressBar");

            progressSection.removeClass("invisible");
            progressSection.removeClass("d-none");
            progressSection.addClass("visible");
            progressSection.addClass("d-inline");
        },

        // Function to switch off the progress timer
        progressOff: function () {

            var progressSection = $("#progressBar");

            progressSection.addClass("invisible");
            progressSection.addClass("d-none");
            progressSection.removeClass("visible");
            progressSection.removeClass("d-inline");
        },

        // Cancel all of the notification events
        cancelNotifyEvents: function () {

            // Already initialised the notification events?
            this.notificationEvents = (this.notificationEvents == null) ? [] : this.notificationEvents;

            // Do we have any notification events set up?
            if (this.notificationEvents != null &&
                this.notificationEvents.length != 0) {

                // Loop the event id's and clear the timer for each of those events
                // that has not fired yet
                this.notificationEvents.forEach(function (event)
                {
                    clearTimeout(event); // Clear the timer event
                });

                // Set up a new array for the events
                this.notificationEvents = [];
            }

        }

    };
