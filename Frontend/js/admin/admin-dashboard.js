document.addEventListener("DOMContentLoaded", function () {
    if (checkAdminAuthentication()) loadDashboard();
});

async function loadDashboard() {
    try {
        const response = await adminApiFetch("/api/admin/dashboard");
        if (!response.ok) throw new Error("Unable to load dashboard data.");

        const data = await response.json();
        setDashboardValue("totalUsers", data.totalUsers);
        setDashboardValue("totalEmployers", data.totalEmployers);
        setDashboardValue("totalJobSeekers", data.totalJobSeekers);
        setDashboardValue("totalVacancies", data.totalVacancies);
        setDashboardValue("totalApplications", data.totalApplications);
    } catch (error) {
        handleAdminApiError(error);
    }
}

function setDashboardValue(elementId, value) {
    const element = document.getElementById(elementId);
    if (element) element.textContent = value ?? 0;
}
