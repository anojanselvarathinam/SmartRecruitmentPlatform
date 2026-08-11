let employerJobs = [];

document.addEventListener("DOMContentLoaded", function () {
    if (checkEmployerAuthentication()) loadEmployerJobs();
});

async function loadEmployerJobs() {
    const container = document.getElementById("jobsContainer");
    try {
        const companyResponse = await employerFetch(`/api/Company/employer/${getEmployerId()}`);
        if (companyResponse.status === 404) {
            container.innerHTML = '<div class="empty">Create a company profile before posting vacancies.<br><br><a class="button button-primary" href="company.html">Create Company</a></div>';
            return;
        }
        if (!companyResponse.ok) throw new Error(await readApiError(companyResponse, "Unable to load company."));
        const company = await companyResponse.json();

        const response = await employerFetch(`/api/Job/company/${company.companyId}`);
        if (!response.ok) throw new Error(await readApiError(response, "Unable to load vacancies."));
        employerJobs = await response.json();
        displayEmployerJobs();
    } catch (error) {
        container.innerHTML = `<div class="empty">${escapeEmployerHtml(error.message)}</div>`;
        showEmployerNotice(error.message, "error");
    }
}

function displayEmployerJobs() {
    const container = document.getElementById("jobsContainer");
    if (employerJobs.length === 0) {
        container.innerHTML = '<div class="empty">No vacancies yet.<br><br><a class="button button-primary" href="job-form.html">Post Vacancy</a></div>';
        return;
    }

    container.innerHTML = `<table><thead><tr><th>Vacancy</th><th>Location</th><th>Experience</th><th>Created</th><th>Status</th><th>Actions</th></tr></thead><tbody>${employerJobs.map(function (job) {
        return `<tr><td><strong>${escapeEmployerHtml(job.jobTitle)}</strong><br><small>${escapeEmployerHtml(job.education)}</small></td><td>${escapeEmployerHtml(job.location)}</td><td>${job.requiredExperience} year${job.requiredExperience === 1 ? "" : "s"}</td><td>${formatEmployerDate(job.createdAt)}</td><td><span class="badge ${job.isActive ? "badge-active" : "badge-closed"}">${job.isActive ? "Active" : "Closed"}</span></td><td><div class="actions"><a class="button button-secondary button-small" href="job-form.html?id=${job.jobId}">Edit</a>${job.isActive ? `<button class="button button-danger button-small" onclick="closeVacancy(${job.jobId})">Close</button>` : ""}<a class="button button-primary button-small" href="applicants.html?jobId=${job.jobId}">View Applicants</a></div></td></tr>`;
    }).join("")}</tbody></table>`;
}

async function closeVacancy(jobId) {
    showConfirmationModal({
        title: "Close Vacancy?",
        message: "This vacancy will no longer appear in active job listings.",
        confirmText: "Close",
        variant: "danger",
        onConfirm: function () { return closeVacancyRequest(jobId); }
    });
}

async function closeVacancyRequest(jobId) {
    try {
        const response = await employerFetch(`/api/Job/${jobId}/close`, { method: "PUT" });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to close vacancy."));
        const job = employerJobs.find(item => item.jobId === jobId);
        if (job) job.isActive = false;
        displayEmployerJobs();
        showEmployerNotice("Vacancy closed successfully.");
    } catch (error) {
        showEmployerNotice(error.message, "error");
    }
}
