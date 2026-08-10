/* =========================================
   ADMIN DASHBOARD JS
   ========================================= */

document.addEventListener("DOMContentLoaded", function () {

    checkAdminAuthentication();

    loadDashboard();

});


/* =========================================
   LOAD DASHBOARD
   ========================================= */

async function loadDashboard() {

    /*
        Temporary demo data.

        Later this can be replaced with:

        fetch("/api/admin/dashboard")
    */

    const dashboardData = {

        totalUsers: 120,

        totalVacancies: 35,

        totalApplications: 245

    };


    document.getElementById("totalUsers")
        .textContent =
        dashboardData.totalUsers;


    document.getElementById("totalVacancies")
        .textContent =
        dashboardData.totalVacancies;


    document.getElementById("totalApplications")
        .textContent =
        dashboardData.totalApplications;

}


/*
   FUTURE BACKEND VERSION

async function loadDashboard() {

    try {

        const response = await fetch(
            "/api/admin/dashboard",
            {
                headers: {
                    "Authorization":
                        "Bearer " + getAdminToken()
                }
            }
        );

        if (!response.ok) {
            throw new Error("Dashboard API failed");
        }

        const data =
            await response.json();

        document.getElementById("totalUsers")
            .textContent = data.totalUsers;

        document.getElementById("totalVacancies")
            .textContent = data.totalVacancies;

        document.getElementById("totalApplications")
            .textContent =
            data.totalApplications;

    }
    catch (error) {

        handleAdminApiError(error);

    }

}

*/