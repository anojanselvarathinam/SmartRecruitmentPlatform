document.addEventListener("DOMContentLoaded", function () {
    removeAdminSidebarLogout();
    setupMobileMenu();
    setupActiveMenu();
    setupAdminTopbar();
});

function removeAdminSidebarLogout() {
    document.querySelectorAll(".admin-sidebar a").forEach(function (link) {
        if (link.textContent.trim().toLowerCase().includes("logout")) {
            const menuItem = link.closest("li");
            if (menuItem) menuItem.remove();
        }
    });
}

function getAdminToken() {
    return localStorage.getItem("talentSyncToken") ||
        sessionStorage.getItem("talentSyncToken");
}

function setupAdminTopbar() {
    const profile = document.querySelector(".admin-profile");
    if (!profile || document.querySelector(".admin-topbar-actions")) return;
    const admin = getAdminAccountInformation();
    const profileName = profile.querySelector(".admin-profile-name");
    const profileRole = profile.querySelector(".admin-profile-role");
    const avatar = profile.querySelector(".admin-avatar");
    if (profileName) profileName.textContent = admin.name;
    if (profileRole) profileRole.textContent = admin.role;
    if (avatar) avatar.textContent = admin.initial;
    profile.setAttribute("tabindex", "0");
    profile.setAttribute("role", "button");
    profile.setAttribute("aria-label", "Open Admin profile menu");
    profile.setAttribute("aria-expanded", "false");
    const logout = document.createElement("button");
    logout.className = "admin-topbar-logout";
    logout.type = "button";
    logout.innerHTML = '<span aria-hidden="true">↪</span> Logout';
    logout.addEventListener("click", function (event) { event.stopPropagation(); clearAdminAuthentication(); window.location.href = "../Authentication/login.html"; });
    const actions = document.createElement("div");
    actions.className = "admin-topbar-actions";
    profile.parentNode.insertBefore(actions, profile);
    actions.appendChild(profile);
    actions.appendChild(logout);

    const dropdown = document.createElement("div");
    dropdown.className = "admin-profile-dropdown";
    dropdown.setAttribute("aria-hidden", "true");
    dropdown.innerHTML = `<div class="admin-dropdown-header"><span class="admin-dropdown-avatar">${escapeAdminHtml(admin.initial)}</span><div><strong>${escapeAdminHtml(admin.name)}</strong><span>${escapeAdminHtml(admin.email)}</span><small>Role: ${escapeAdminHtml(admin.role)}</small></div></div><div class="admin-dropdown-actions"><a href="profile.html">My Profile</a><button type="button">Logout</button></div>`;
    actions.appendChild(dropdown);

    function toggleDropdown() {
        const isOpen = dropdown.classList.toggle("show");
        dropdown.setAttribute("aria-hidden", String(!isOpen));
        profile.setAttribute("aria-expanded", String(isOpen));
    }

    profile.addEventListener("click", function (event) { event.stopPropagation(); toggleDropdown(); });
    profile.addEventListener("keydown", function (event) { if (event.key === "Enter" || event.key === " ") { event.preventDefault(); toggleDropdown(); } });
    dropdown.querySelector("button").addEventListener("click", function () { clearAdminAuthentication(); window.location.href = "../Authentication/login.html"; });
    document.addEventListener("click", function (event) { if (!actions.contains(event.target)) closeAdminProfileDropdown(dropdown, profile); });
    document.addEventListener("keydown", function (event) { if (event.key === "Escape") closeAdminProfileDropdown(dropdown, profile); });
}

function closeAdminProfileDropdown(dropdown, profile) {
    dropdown.classList.remove("show");
    dropdown.setAttribute("aria-hidden", "true");
    profile.setAttribute("aria-expanded", "false");
}

function getAdminAccountInformation() {
    const rawUser = localStorage.getItem("talentSyncUser") || sessionStorage.getItem("talentSyncUser");
    let user = {};
    try { user = rawUser ? JSON.parse(rawUser) : {}; } catch { user = {}; }
    const nameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    const emailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    const idClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    const name = user[nameClaim] || user.name || user.unique_name || "Administrator";
    return {
        id: user[idClaim] || user.nameid || "",
        name: name,
        email: user[emailClaim] || user.email || "Email unavailable",
        role: user[roleClaim] || user.role || "Admin",
        initial: name.charAt(0).toUpperCase()
    };
}

function escapeAdminHtml(value) {
    const element = document.createElement("div");
    element.textContent = value ?? "";
    return element.innerHTML;
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
