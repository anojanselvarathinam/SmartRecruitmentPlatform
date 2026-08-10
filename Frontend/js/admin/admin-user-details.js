/* =========================================
   USER DETAILS JS
   ========================================= */

let currentUser = null;


document.addEventListener("DOMContentLoaded", function () {

    checkAdminAuthentication();

    loadUserDetails();

    setupAccountButtons();

});


/* =========================================
   LOAD USER
   ========================================= */

function loadUserDetails() {

    const urlParams =
        new URLSearchParams(
            window.location.search
        );


    const userId =
        parseInt(
            urlParams.get("id")
        );


    const users = [

        {
            id: 1,
            name: "John Silva",
            email: "john@example.com",
            role: "Job Seeker",
            status: "Active"
        },

        {
            id: 2,
            name: "ABC Technologies",
            email: "hr@abc.com",
            role: "Employer",
            status: "Active"
        },

        {
            id: 3,
            name: "Kamal Perera",
            email: "kamal@example.com",
            role: "Job Seeker",
            status: "Blocked"
        },

        {
            id: 4,
            name: "XYZ Solutions",
            email: "hr@xyz.com",
            role: "Employer",
            status: "Active"
        }

    ];


    currentUser =
        users.find(
            user => user.id === userId
        );


    if (!currentUser) {

        showAdminAlert(
            "User not found.",
            "error"
        );

        return;
    }


    displayUserDetails();

}


/* =========================================
   DISPLAY USER
   ========================================= */

function displayUserDetails() {

    document.getElementById("userName")
        .textContent =
        currentUser.name;


    document.getElementById("userEmail")
        .textContent =
        currentUser.email;


    document.getElementById("detailName")
        .textContent =
        currentUser.name;


    document.getElementById("detailEmail")
        .textContent =
        currentUser.email;


    document.getElementById("detailRole")
        .textContent =
        currentUser.role;


    document.getElementById("detailStatus")
        .textContent =
        currentUser.status;


    document.getElementById("userAvatar")
        .textContent =
        currentUser.name
            .charAt(0)
            .toUpperCase();

}


/* =========================================
   BUTTONS
   ========================================= */

function setupAccountButtons() {

    document.getElementById(
        "activateButton"
    ).addEventListener(
        "click",
        activateAccount
    );


    document.getElementById(
        "blockButton"
    ).addEventListener(
        "click",
        blockAccount
    );

}


/* =========================================
   ACTIVATE
   ========================================= */

function activateAccount() {

    if (!currentUser) {
        return;
    }


    currentUser.status = "Active";

    displayUserDetails();


    showAdminAlert(
        "User account activated successfully.",
        "success"
    );

}


/* =========================================
   BLOCK
   ========================================= */

function blockAccount() {

    if (!currentUser) {
        return;
    }


    const confirmed =
        confirm(
            "Are you sure you want to block this account?"
        );


    if (!confirmed) {
        return;
    }


    currentUser.status = "Blocked";

    displayUserDetails();


    showAdminAlert(
        "User account blocked successfully.",
        "success"
    );

}


/* =========================================
   BACK
   ========================================= */

function goBackToUsers() {

    window.location.href =
        "users.html";

}