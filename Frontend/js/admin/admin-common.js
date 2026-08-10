document.addEventListener("DOMContentLoaded", function () {
    setupMobileMenu();
    setupLogout();
    setupActiveMenu();
});

function getAdminToken() {
    return localStorage.getItem("talentSyncToken") ||
        sessionStorage.getItem("talentSyncToken");
}

function getAdminRole() {
    return localStorage.getItem("talentSyncRole") ||
        sessionStorage.getItem("talentSyncRole");
}

function checkAdminAuthentication() {
    const token = getAdminToken();
    const role = getAdminRole();

    if (!token || role !== "Admin") {
        clearAdminAuthentication();
        window.location.href = "../Authentication/login.html";
        return false;
    }

    return true;
}

function clearAdminAuthentication() {
    const keys = ["talentSyncToken", "talentSyncRole", "talentSyncUser"];

    keys.forEach(function (key) {
        localStorage.removeItem(key);
        sessionStorage.removeItem(key);
    });
}

function setupLogout() {
    const logoutButton = document.getElementById("adminLogout");
    if (!logoutButton) return;

    logoutButton.addEventListener("click", function (event) {
        event.preventDefault();
        clearAdminAuthentication();
        window.location.href = "../Authentication/login.html";
    });
}

async function adminApiFetch(url, options = {}) {
    const headers = new Headers(options.headers || {});
    headers.set("Authorization", "Bearer " + getAdminToken());

    const response = await fetch(url, {
        ...options,
        headers: headers
    });

    if (response.status === 401 || response.status === 403) {
        clearAdminAuthentication();
        window.location.href = "../Authentication/login.html";
        throw new Error("Admin session is not authorized.");
    }

    return response;
}

function setupMobileMenu() {
    const menuButton = document.getElementById("mobileMenuButton");
    const sidebar = document.querySelector(".admin-sidebar");
    if (!menuButton || !sidebar) return;

    menuButton.addEventListener("click", function () {
        sidebar.classList.toggle("mobile-open");
    });
}

function setupActiveMenu() {
    const currentPage = window.location.pathname.split("/").pop();
    const menuLinks = document.querySelectorAll(".admin-menu a");

    menuLinks.forEach(function (link) {
        const linkPage = link.getAttribute("href").split("/").pop();
        if (linkPage === currentPage) link.classList.add("active");
    });
}

function showAdminAlert(message, type = "success") {
    const alertBox = document.getElementById("adminAlert");
    if (!alertBox) return;

    alertBox.textContent = message;
    alertBox.className = "admin-alert " + type + " show";
    setTimeout(function () {
        alertBox.classList.remove("show");
    }, 3000);
}

function handleAdminApiError(error) {
    console.error("Admin API Error:", error);
    showAdminAlert(error.message || "Something went wrong. Please try again.", "error");
}
