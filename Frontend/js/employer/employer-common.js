function getEmployerToken() {
    return localStorage.getItem("talentSyncToken") || sessionStorage.getItem("talentSyncToken");
}

function getEmployerRole() {
    return localStorage.getItem("talentSyncRole") || sessionStorage.getItem("talentSyncRole");
}

function decodeEmployerToken(token) {
    try {
        const value = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
        return JSON.parse(decodeURIComponent(atob(value).split("").map(function (character) {
            return "%" + ("00" + character.charCodeAt(0).toString(16)).slice(-2);
        }).join("")));
    } catch {
        return null;
    }
}

function getEmployerId() {
    const payload = decodeEmployerToken(getEmployerToken() || "");
    return Number(payload?.employerId || 0);
}

function clearEmployerAuthentication() {
    ["talentSyncToken", "talentSyncRole", "talentSyncUser"].forEach(function (key) {
        localStorage.removeItem(key);
        sessionStorage.removeItem(key);
    });
}

function checkEmployerAuthentication() {
    if (!getEmployerToken() || getEmployerRole() !== "Employer" || !getEmployerId()) {
        clearEmployerAuthentication();
        window.location.href = "../Authentication/login.html";
        return false;
    }
    return true;
}

async function employerFetch(url, options = {}) {
    const headers = new Headers(options.headers || {});
    headers.set("Authorization", "Bearer " + getEmployerToken());
    const response = await fetch(url, { ...options, headers: headers });

    if (response.status === 401 || response.status === 403) {
        clearEmployerAuthentication();
        window.location.href = "../Authentication/login.html";
        throw new Error("Your Employer session is not authorized.");
    }
    return response;
}

async function readApiError(response, fallback) {
    try {
        const data = await response.json();
        return data.message || data.title || fallback;
    } catch {
        try { return (await response.text()) || fallback; } catch { return fallback; }
    }
}

function showEmployerNotice(message, type = "success") {
    const notice = document.getElementById("pageNotice");
    if (!notice) return;
    notice.textContent = message;
    notice.className = "notice show " + type;
    window.setTimeout(function () { notice.classList.remove("show"); }, 4500);
}

function setEmployerLoading(elementId, message = "Loading...") {
    const element = document.getElementById(elementId);
    if (element) element.innerHTML = `<div class="loading">${escapeEmployerHtml(message)}</div>`;
}

function escapeEmployerHtml(value) {
    const element = document.createElement("div");
    element.textContent = value ?? "";
    return element.innerHTML;
}

function formatEmployerDate(value) {
    return value ? new Date(value).toLocaleDateString() : "-";
}

function getQueryNumber(name) {
    return Number(new URLSearchParams(window.location.search).get(name) || 0);
}

function setActiveEmployerMenu() {
    const page = window.location.pathname.split("/").pop();
    document.querySelectorAll(".nav a").forEach(function (link) {
        const target = link.getAttribute("href").split("?")[0];
        if (target === page || (page === "job-form.html" && target === "jobs.html") ||
            (page === "applicants.html" && target === "jobs.html")) link.classList.add("active");
    });
}

function setupEmployerShell() {
    document.getElementById("menuButton")?.addEventListener("click", function () {
        document.querySelector(".sidebar")?.classList.toggle("open");
    });
    document.getElementById("employerLogout")?.addEventListener("click", function (event) {
        event.preventDefault();
        clearEmployerAuthentication();
        window.location.href = "../Authentication/login.html";
    });
    setActiveEmployerMenu();
}

document.addEventListener("DOMContentLoaded", setupEmployerShell);
