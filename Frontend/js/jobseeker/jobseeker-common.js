function getJobSeekerToken() { return localStorage.getItem("talentSyncToken") || sessionStorage.getItem("talentSyncToken"); }
function getJobSeekerRole() { return localStorage.getItem("talentSyncRole") || sessionStorage.getItem("talentSyncRole"); }
function clearJobSeekerAuthentication() { ["talentSyncToken","talentSyncRole","talentSyncUser"].forEach(key => { localStorage.removeItem(key); sessionStorage.removeItem(key); }); }
function checkJobSeekerAuthentication() { if (!getJobSeekerToken() || getJobSeekerRole() !== "JobSeeker") { clearJobSeekerAuthentication(); window.location.href = "../Authentication/login.html"; return false; } return true; }
async function jobSeekerFetch(url, options = {}) { const headers = new Headers(options.headers || {}); headers.set("Authorization", "Bearer " + getJobSeekerToken()); const response = await fetch(url, { ...options, headers }); if (response.status === 401 || response.status === 403) { clearJobSeekerAuthentication(); window.location.href = "../Authentication/login.html"; throw new Error("Your Job Seeker session is not authorized."); } return response; }
async function jobSeekerError(response, fallback) { try { const data = await response.json(); return data.message || data.title || fallback; } catch { return fallback; } }
function showJobSeekerNotice(message, type = "success") { const box = document.getElementById("pageNotice"); if (!box) return; box.textContent = message; box.className = "notice show " + type; setTimeout(() => box.classList.remove("show"), 4500); }
function escapeJobSeekerHtml(value) { const element = document.createElement("div"); element.textContent = value ?? ""; return element.innerHTML; }
function formatJobSeekerDate(value) { return value ? new Date(value).toLocaleDateString() : "Present"; }
function queryNumber(name) { return Number(new URLSearchParams(location.search).get(name) || 0); }
function setupJobSeekerShell() { document.getElementById("menuButton")?.addEventListener("click", () => document.querySelector(".sidebar")?.classList.toggle("open")); const page = location.pathname.split("/").pop(); document.querySelectorAll(".nav a").forEach(link => { const target = link.getAttribute("href").split("?")[0]; if (target === page || (page === "job-details.html" && target === "jobs.html")) link.classList.add("active"); }); }
function addJobSeekerNavigation() {
    const navigation = document.querySelector(".nav");
    if (navigation && !navigation.querySelector('a[href="skills.html"]')) {
        const jobsItem = Array.from(navigation.children).find(item => item.querySelector('a[href="jobs.html"]'));
        const skillItem = document.createElement("li");
        skillItem.innerHTML = '<a href="skills.html">✦ Skills</a>';
        const cvItem = document.createElement("li");
        cvItem.innerHTML = '<a href="cv.html">▤ CV / Resume</a>';
        navigation.insertBefore(skillItem, jobsItem);
        navigation.insertBefore(cvItem, jobsItem);
    }
}
function addJobSeekerTopbarActions() {
    const topbar = document.querySelector(".topbar");
    if (!topbar || topbar.querySelector(".topbar-actions")) return;
    const oldName = topbar.querySelector(".employer-name");
    const displayName = oldName && oldName.id === "seekerName" ? oldName.textContent : "My Profile";
    if (oldName) oldName.remove();
    const actions = document.createElement("div");
    actions.className = "topbar-actions";
    actions.innerHTML = `<a class="topbar-profile" href="profile.html" aria-label="Open Job Seeker profile"><span class="topbar-avatar">JS</span><span><strong id="topbarUserName">${escapeJobSeekerHtml(displayName)}</strong><small>Job Seeker</small></span></a><button class="topbar-logout" type="button">Logout</button>`;
    actions.querySelector(".topbar-logout").addEventListener("click", function () { clearJobSeekerAuthentication(); location.href = "../Authentication/login.html"; });
    topbar.appendChild(actions);
}
function initializeJobSeekerShell() {
    removeJobSeekerSidebarLogout();
    addJobSeekerNavigation();
    addJobSeekerTopbarActions();
    setupJobSeekerShell();
}

function removeJobSeekerSidebarLogout() {
    document.querySelectorAll(".sidebar .nav a").forEach(function (link) {
        if (link.textContent.trim().toLowerCase().includes("logout")) {
            const menuItem = link.closest("li");
            if (menuItem) menuItem.remove();
        }
    });
}
document.addEventListener("DOMContentLoaded", initializeJobSeekerShell);
