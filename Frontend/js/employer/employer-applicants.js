let rankedApplicants = [];
let applicantJobId = 0;

document.addEventListener("DOMContentLoaded", function () {
    if (!checkEmployerAuthentication()) return;
    applicantJobId = getQueryNumber("jobId");
    if (!applicantJobId) {
        document.getElementById("applicantsContainer").innerHTML = '<div class="empty">Invalid vacancy.</div>';
        return;
    }
    loadRankedApplicants();
});

async function loadRankedApplicants() {
    try {
        const jobResponse = await employerFetch(`/api/Job/${applicantJobId}`);
        if (jobResponse.ok) {
            const job = await jobResponse.json();
            document.getElementById("jobName").textContent = `${job.jobTitle} applicants ordered by match score.`;
        }

        const response = await employerFetch(`/api/Applicant/job/${applicantJobId}`);
        if (!response.ok) throw new Error(await readApiError(response, "Unable to load applicants."));
        const basicApplicants = await response.json();

        rankedApplicants = await Promise.all(basicApplicants.map(async function (applicant) {
            const detailResponse = await employerFetch(`/api/Applicant/${applicant.applicationId}`);
            return detailResponse.ok ? await detailResponse.json() : applicant;
        }));
        rankedApplicants.sort((a, b) => b.matchScore - a.matchScore);
        displayRankedApplicants();
    } catch (error) {
        document.getElementById("applicantsContainer").innerHTML = `<div class="empty">${escapeEmployerHtml(error.message)}</div>`;
        showEmployerNotice(error.message, "error");
    }
}

function displayRankedApplicants() {
    const container = document.getElementById("applicantsContainer");
    if (rankedApplicants.length === 0) {
        container.innerHTML = '<div class="empty">No applications have been received for this vacancy.</div>';
        return;
    }

    const statuses = ["Applied", "Reviewed", "Shortlisted", "Rejected", "Accepted"];
    container.innerHTML = `<table><thead><tr><th>Rank</th><th>Applicant</th><th>Match</th><th>Applied</th><th>Status</th><th>Actions</th></tr></thead><tbody>${rankedApplicants.map(function (applicant, index) {
        const options = statuses.map(status => `<option value="${status}" ${status === applicant.status ? "selected" : ""}>${status}</option>`).join("");
        return `<tr><td><strong>#${index + 1}</strong></td><td><strong>${escapeEmployerHtml(applicant.fullName || `Applicant ${applicant.jobSeekerId}`)}</strong><br><small>${escapeEmployerHtml(applicant.email || "")}</small></td><td><span class="score">${Number(applicant.matchScore).toFixed(2)}%</span></td><td>${formatEmployerDate(applicant.appliedAt)}</td><td><select id="status-${applicant.applicationId}">${options}</select></td><td><div class="actions"><button class="button button-success button-small" onclick="updateApplicantStatus(${applicant.applicationId})">Update</button><button class="button button-warning button-small" onclick="sendContactRequest(${applicant.jobSeekerId})">Contact</button></div></td></tr>`;
    }).join("")}</tbody></table>`;
}

async function updateApplicantStatus(applicationId) {
    const status = document.getElementById(`status-${applicationId}`).value;
    try {
        const response = await employerFetch(`/api/Applicant/${applicationId}/status`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ status: status })
        });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to update status."));
        const applicant = rankedApplicants.find(item => item.applicationId === applicationId);
        if (applicant) applicant.status = status;
        showEmployerNotice("Application status updated. The Job Seeker was notified.");
    } catch (error) {
        showEmployerNotice(error.message, "error");
    }
}

async function sendContactRequest(jobSeekerId) {
    try {
        const response = await employerFetch("/api/ContactRequest", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ jobSeekerId: jobSeekerId })
        });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to send contact request."));
        showEmployerNotice("Contact request sent successfully.");
    } catch (error) {
        showEmployerNotice(error.message, "error");
    }
}
