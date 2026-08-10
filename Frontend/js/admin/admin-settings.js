/* =========================================
   ADMIN SETTINGS JS
   ========================================= */

document.addEventListener("DOMContentLoaded", function () {

    checkAdminAuthentication();

    loadSettings();

    setupSaveButton();

});


/* =========================================
   LOAD SETTINGS
   ========================================= */

function loadSettings() {

    /*
        Temporary settings.

        Later these values can come
        from the backend.
    */

    const settings = {

        registrationEnabled: true,

        jobPostingEnabled: true,

        systemEnabled: true

    };


    document.getElementById(
        "registrationToggle"
    ).checked =
        settings.registrationEnabled;


    document.getElementById(
        "jobPostingToggle"
    ).checked =
        settings.jobPostingEnabled;


    document.getElementById(
        "systemStatusToggle"
    ).checked =
        settings.systemEnabled;

}


/* =========================================
   SAVE BUTTON
   ========================================= */

function setupSaveButton() {

    document.getElementById(
        "saveSettingsButton"
    ).addEventListener(
        "click",
        saveSettings
    );

}


/* =========================================
   SAVE SETTINGS
   ========================================= */

function saveSettings() {

    const settings = {

        registrationEnabled:
            document.getElementById(
                "registrationToggle"
            ).checked,

        jobPostingEnabled:
            document.getElementById(
                "jobPostingToggle"
            ).checked,

        systemEnabled:
            document.getElementById(
                "systemStatusToggle"
            ).checked

    };


    /*
        Temporary console output.

        Later send this object to backend API.
    */

    console.log(
        "Settings:",
        settings
    );


    showAdminAlert(
        "Settings saved successfully.",
        "success"
    );

}