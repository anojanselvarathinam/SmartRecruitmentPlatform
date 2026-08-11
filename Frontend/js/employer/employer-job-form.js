let editingJob = null;
let employerCompany = null;

document.addEventListener("DOMContentLoaded", async function () {
    if (!checkEmployerAuthentication()) return;
    document.getElementById("jobForm").addEventListener("submit", saveVacancy);
    await loadCompanyAndJob();
});

async function loadCompanyAndJob() {
    try {
        const companyResponse = await employerFetch(`/api/Company/employer/${getEmployerId()}`);
        if (companyResponse.status === 404) {
            showEmployerNotice("Create your company profile before posting a vacancy.", "error");
            document.getElementById("jobSubmit").disabled = true;
            return;
        }
        if (!companyResponse.ok) throw new Error(await readApiError(companyResponse, "Unable to load company."));
        employerCompany = await companyResponse.json();

        const jobId = getQueryNumber("id");
        if (!jobId) return;
        const jobResponse = await employerFetch(`/api/Job/${jobId}`);
        if (!jobResponse.ok) throw new Error(await readApiError(jobResponse, "Unable to load vacancy."));
        editingJob = await jobResponse.json();
        document.getElementById("formTitle").textContent = "Edit Vacancy";
        document.getElementById("jobTitle").value = editingJob.jobTitle;
        document.getElementById("location").value = editingJob.location;
        document.getElementById("experience").value = editingJob.requiredExperience;
        document.getElementById("education").value = editingJob.education;
        document.getElementById("requiredSkills").value = editingJob.requiredSkills;
        document.getElementById("description").value = editingJob.description;
        document.getElementById("isActive").checked = editingJob.isActive;
        document.getElementById("activeField").style.display = "block";
    } catch (error) {
        showEmployerNotice(error.message, "error");
        document.getElementById("jobSubmit").disabled = true;
    }
}

async function saveVacancy(event) {
    event.preventDefault();
    if (!employerCompany) return;
    const button = document.getElementById("jobSubmit");
    const payload = {
        companyId: employerCompany.companyId,
        jobTitle: document.getElementById("jobTitle").value.trim(),
        location: document.getElementById("location").value.trim(),
        requiredExperience: Number(document.getElementById("experience").value),
        education: document.getElementById("education").value.trim(),
        requiredSkills: document.getElementById("requiredSkills").value.trim(),
        description: document.getElementById("description").value.trim()
    };
    if (!payload.jobTitle || !payload.location || !payload.education || !payload.requiredSkills || !payload.description || payload.requiredExperience < 0) {
        showEmployerNotice("Complete all required vacancy fields.", "error");
        return;
    }
    if (editingJob) payload.isActive = document.getElementById("isActive").checked;

    button.disabled = true;
    button.textContent = "Saving...";
    try {
        const response = await employerFetch(editingJob ? `/api/Job/${editingJob.jobId}` : "/api/Job", {
            method: editingJob ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to save vacancy."));
        showEmployerNotice("Vacancy saved successfully.");
        window.setTimeout(function () { window.location.href = "jobs.html"; }, 700);
    } catch (error) {
        showEmployerNotice(error.message, "error");
        button.disabled = false;
        button.textContent = "Save Vacancy";
    }
}
