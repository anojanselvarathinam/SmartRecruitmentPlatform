/* =========================================
   ADMIN COMMON JS
   ========================================= */

document.addEventListener("DOMContentLoaded", function () {

    setupMobileMenu();
    setupLogout();
    setupActiveMenu();

});


/* =========================================
   GET TOKEN
   ========================================= */

function getAdminToken() {

    return localStorage.getItem("token");

}


/* =========================================
   CHECK LOGIN
   ========================================= */

function checkAdminAuthentication() {

    const token = getAdminToken();

    if (!token) {

        window.location.href =
            "../Authentication/login.html";

        return false;
    }

    return true;
}


/* =========================================
   LOGOUT
   ========================================= */

function setupLogout() {

    const logoutButton =
        document.getElementById("adminLogout");

    if (!logoutButton) {
        return;
    }

    logoutButton.addEventListener("click", function () {

        localStorage.removeItem("token");
        localStorage.removeItem("user");

        window.location.href =
            "../Authentication/login.html";

    });

}


/* =========================================
   MOBILE MENU
   ========================================= */

function setupMobileMenu() {

    const menuButton =
        document.getElementById("mobileMenuButton");

    const sidebar =
        document.querySelector(".admin-sidebar");

    if (!menuButton || !sidebar) {
        return;
    }

    menuButton.addEventListener("click", function () {

        sidebar.classList.toggle("mobile-open");

    });

}


/* =========================================
   ACTIVE MENU
   ========================================= */

function setupActiveMenu() {

    const currentPage =
        window.location.pathname.split("/").pop();

    const menuLinks =
        document.querySelectorAll(".admin-menu a");

    menuLinks.forEach(function (link) {

        const linkPage =
            link.getAttribute("href")
                .split("/")
                .pop();

        if (linkPage === currentPage) {

            link.classList.add("active");

        }

    });

}


/* =========================================
   SHOW ALERT
   ========================================= */

function showAdminAlert(message, type = "success") {

    const alertBox =
        document.getElementById("adminAlert");

    if (!alertBox) {
        return;
    }

    alertBox.textContent = message;

    alertBox.className =
        "admin-alert " + type + " show";

    setTimeout(function () {

        alertBox.classList.remove("show");

    }, 3000);

}


/* =========================================
   API ERROR HANDLER
   ========================================= */

function handleAdminApiError(error) {

    console.error("Admin API Error:", error);

    showAdminAlert(
        "Something went wrong. Please try again.",
        "error"
    );

}