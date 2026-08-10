document.addEventListener("DOMContentLoaded", function () {
    if (checkEmployerAuthentication()) loadEmployerDashboard();
});

async function loadEmployerDashboard() {
    const employerId = getEmployerId();
    try {
        const employerResponse = await employerFetch(`/api/Employer/${employerId}`);
        if (!employerResponse.ok) throw new Error(await readApiError(employerResponse, "Unable to load Employer account."));
        const employer = await employerResponse.json();
        document.getElementById("employerName").textContent = employer.fullName;

        const companyResponse = await employerFetch(`/api/Company/employer/${employerId}`);
        if (companyResponse.status === 404) {
            document.getElementById("companyStatus").textContent = "Required";
            document.getElementById("dashboardMessage").innerHTML = 'Create your company profile before posting vacancies. <a href="company.html">Create company</a>';
            return;
        }
        if (!companyResponse.ok) throw new Error(await readApiError(companyResponse, "Unable to load company."));

        const company = await companyResponse.json();
        document.getElementById("companyStatus").textContent = "Complete";
        document.getElementById("dashboardMessage").textContent = `${company.companyName} · ${company.location}`;

        const jobsResponse = await employerFetch(`/api/Job/company/${company.companyId}`);
        if (!jobsResponse.ok) throw new Error(await readApiError(jobsResponse, "Unable to load vacancies."));
        const jobs = await jobsResponse.json();
        document.getElementById("jobCount").textContent = jobs.length;
        document.getElementById("activeJobCount").textContent = jobs.filter(job => job.isActive).length;
    } catch (error) {
        showEmployerNotice(error.message, "error");
        document.getElementById("dashboardMessage").textContent = "Dashboard data could not be loaded.";
    }
}
