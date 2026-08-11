let employerVacancies = [];
let rankedApplicants = [];
let applicantJobId = 0;
let contactedJobSeekerIds = new Set();

const validApplicationStatuses = [
    "Applied",
    "Reviewed",
    "Shortlisted",
    "Rejected",
    "Accepted"
];

document.addEventListener("DOMContentLoaded", function () {
    if (!checkEmployerAuthentication()) return;
    document.getElementById("vacancySelect").addEventListener("change", changeVacancy);
    document.getElementById("closeApplicantDetails").addEventListener("click", closeApplicantDetails);
    loadApplicantsPage();
});

async function loadApplicantsPage() {
    const container = document.getElementById("applicantsContainer");
    try {
        const companyResponse = await employerFetch(`/api/Company/employer/${getEmployerId()}`);
        if (companyResponse.status === 404) {
            showNoVacancies("Create a company profile before managing applicants.");
            return;
        }
        if (!companyResponse.ok) throw new Error(await readApiError(companyResponse, "Unable to load company."));
        const company = await companyResponse.json();

        const jobsResponse = await employerFetch(`/api/Job/company/${company.companyId}`);
        if (!jobsResponse.ok) throw new Error(await readApiError(jobsResponse, "Unable to load vacancies."));
        employerVacancies = await jobsResponse.json();
        displayVacancyOptions();

        if (employerVacancies.length === 0) {
            showNoVacancies("No vacancies available.");
            return;
        }

        await loadExistingContactRequests();
        const requestedJobId = getQueryNumber("jobId");
        const requestedJob = employerVacancies.find(job => job.jobId === requestedJobId);
        if (requestedJob) {
            applicantJobId = requestedJob.jobId;
            document.getElementById("vacancySelect").value = String(applicantJobId);
            await loadRankedApplicants();
        } else {
            container.innerHTML = '<div class="card empty">Select a vacancy above to view applicants.</div>';
        }
    } catch (error) {
        container.innerHTML = `<div class="card empty">${escapeEmployerHtml(error.message)}</div>`;
        showEmployerNotice(error.message, "error");
    }
}

function displayVacancyOptions() {
    const select = document.getElementById("vacancySelect");
    if (employerVacancies.length === 0) {
        select.innerHTML = '<option value="">No vacancies available</option>';
        select.disabled = true;
        return;
    }
    select.disabled = false;
    select.innerHTML = '<option value="">Select a vacancy</option>' + employerVacancies.map(function (job) {
        return `<option value="${job.jobId}">${escapeEmployerHtml(job.jobTitle)} — ${escapeEmployerHtml(job.location)}</option>`;
    }).join("");
}

function showNoVacancies(message) {
    document.getElementById("vacancySelect").innerHTML = '<option value="">No vacancies available</option>';
    document.getElementById("vacancySelect").disabled = true;
    document.getElementById("jobName").textContent = message;
    document.getElementById("applicantsContainer").innerHTML = `<div class="card empty">${escapeEmployerHtml(message)}</div>`;
}

async function loadExistingContactRequests() {
    const response = await employerFetch(`/api/ContactRequest/employer/${getEmployerId()}`);
    if (!response.ok) return;
    const requests = await response.json();
    contactedJobSeekerIds = new Set(requests.map(request => request.jobSeekerId));
}

async function changeVacancy(event) {
    applicantJobId = Number(event.target.value);
    closeApplicantDetails();
    if (!applicantJobId) {
        history.replaceState(null, "", "applicants.html");
        document.getElementById("jobName").textContent = "Select a vacancy to view its ranked applicants.";
        document.getElementById("applicantsContainer").innerHTML = '<div class="card empty">Select a vacancy above to view applicants.</div>';
        return;
    }
    history.replaceState(null, "", `applicants.html?jobId=${applicantJobId}`);
    await loadRankedApplicants();
}

async function loadRankedApplicants() {
    const container = document.getElementById("applicantsContainer");
    const job = employerVacancies.find(item => item.jobId === applicantJobId);
    if (!job) return;
    document.getElementById("jobName").textContent = `Applicants for ${job.jobTitle}, ranked by match score.`;
    container.innerHTML = '<div class="card loading">Loading applicants...</div>';
    try {
        const response = await employerFetch(`/api/Applicant/job/${applicantJobId}`);
        if (!response.ok) throw new Error(await readApiError(response, "Unable to load applicants."));
        const basicApplicants = await response.json();
        rankedApplicants = await Promise.all(basicApplicants.map(async function (applicant) {
            const detailResponse = await employerFetch(`/api/Applicant/${applicant.applicationId}`);
            return detailResponse.ok ? await detailResponse.json() : applicant;
        }));
        rankedApplicants.sort(function (first, second) { return second.matchScore - first.matchScore; });
        displayRankedApplicants();
    } catch (error) {
        container.innerHTML = `<div class="card empty">${escapeEmployerHtml(error.message)}</div>`;
        showEmployerNotice(error.message, "error");
    }
}

function displayRankedApplicants() {
    const container = document.getElementById("applicantsContainer");
    if (rankedApplicants.length === 0) {
        container.innerHTML = '<div class="card empty">No applications have been received for this vacancy yet.</div>';
        return;
    }
    container.innerHTML = rankedApplicants.map(function (applicant, index) {
        const score = Math.min(100, Number(applicant.matchScore));
        const options = validApplicationStatuses.map(status => `<option value="${status}" ${status === applicant.status ? "selected" : ""}>${status}</option>`).join("");
        const contacted = contactedJobSeekerIds.has(applicant.jobSeekerId);
        return `<article class="card applicant-row">
            <div class="applicant-rank">#${index + 1}</div>
            <div class="applicant-identity"><strong>${escapeEmployerHtml(applicant.fullName || `Applicant ${applicant.jobSeekerId}`)}</strong><span>Applied ${formatEmployerDate(applicant.appliedAt)}</span></div>
            <div class="applicant-match"><strong>${score.toFixed(2)}%</strong><span>Match Score</span><div class="score-bar"><span style="width:${score}%"></span></div></div>
            <div class="applicant-status"><span class="badge badge-${applicant.status.toLowerCase()}">${escapeEmployerHtml(applicant.status)}</span><select id="status-${applicant.applicationId}" aria-label="Application status">${options}</select></div>
            <div class="applicant-actions"><button class="button button-secondary button-small" onclick="viewApplicantDetails(${applicant.applicationId})">View Details</button><button class="button button-success button-small" onclick="updateApplicantStatus(${applicant.applicationId}, this)">Update Status</button><button class="button button-warning button-small" onclick="sendContactRequest(${applicant.jobSeekerId}, this)" ${contacted ? "disabled" : ""}>${contacted ? "Request Sent" : "Send Contact Request"}</button></div>
        </article>`;
    }).join("");
}

async function viewApplicantDetails(applicationId) {
    const panel = document.getElementById("applicantDetailsPanel");
    const content = document.getElementById("applicantDetailsContent");
    panel.hidden = false;
    content.innerHTML = '<div class="loading">Loading applicant details...</div>';
    try {
        const response = await employerFetch(`/api/Applicant/${applicationId}`);
        if (!response.ok) throw new Error(await readApiError(response, "Unable to load applicant details."));
        const applicant = await response.json();
        content.innerHTML = `<div class="applicant-detail-grid"><div><span>Applicant</span><strong>${escapeEmployerHtml(applicant.fullName || `Applicant ${applicant.jobSeekerId}`)}</strong></div><div><span>Email</span><strong>${escapeEmployerHtml(applicant.email || "Not available")}</strong></div><div><span>Application ID</span><strong>${applicant.applicationId}</strong></div><div><span>Job ID</span><strong>${applicant.jobId}</strong></div><div><span>Match Score</span><strong>${Number(applicant.matchScore).toFixed(2)}%</strong></div><div><span>Status</span><strong>${escapeEmployerHtml(applicant.status)}</strong></div><div><span>Applied</span><strong>${formatEmployerDate(applicant.appliedAt)}</strong></div></div>`;
        panel.scrollIntoView({ behavior: "smooth", block: "nearest" });
    } catch (error) {
        content.innerHTML = `<div class="empty">${escapeEmployerHtml(error.message)}</div>`;
        showEmployerNotice(error.message, "error");
    }
}

function closeApplicantDetails() {
    document.getElementById("applicantDetailsPanel").hidden = true;
}

async function updateApplicantStatus(applicationId, button) {
    const status = document.getElementById(`status-${applicationId}`).value;
    const originalText = button.textContent;
    button.disabled = true;
    button.textContent = "Updating...";
    try {
        const response = await employerFetch(`/api/Applicant/${applicationId}/status`, { method: "PUT", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ status: status }) });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to update status."));
        const applicant = rankedApplicants.find(item => item.applicationId === applicationId);
        if (applicant) applicant.status = status;
        displayRankedApplicants();
        showEmployerNotice("Application status updated. The Job Seeker was notified.");
    } catch (error) {
        button.disabled = false;
        button.textContent = originalText;
        showEmployerNotice(error.message, "error");
    }
}

async function sendContactRequest(jobSeekerId, button) {
    button.disabled = true;
    button.textContent = "Sending...";
    try {
        const response = await employerFetch("/api/ContactRequest", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ jobSeekerId: jobSeekerId }) });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to send contact request."));
        contactedJobSeekerIds.add(jobSeekerId);
        button.textContent = "Request Sent";
        showEmployerNotice("Contact request sent successfully.");
    } catch (error) {
        button.disabled = false;
        button.textContent = "Send Contact Request";
        showEmployerNotice(error.message, "error");
    }
}
