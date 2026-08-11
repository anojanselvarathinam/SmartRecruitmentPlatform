let savedCvs = [];

document.addEventListener("DOMContentLoaded", function () {
    if (!checkJobSeekerAuthentication()) return;
    document.getElementById("cvForm").addEventListener("submit", uploadCvDocument);
    loadCvDocuments();
});

async function loadCvDocuments() {
    try {
        const response = await jobSeekerFetch("/api/jobseeker/cv");
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to load CV documents."));
        savedCvs = await response.json();
        displayCvDocuments();
    } catch (error) {
        document.getElementById("cvList").innerHTML = `<div class="empty">${escapeJobSeekerHtml(error.message)}</div>`;
        showJobSeekerNotice(error.message, "error");
    }
}

function displayCvDocuments() {
    document.getElementById("cvTotal").textContent = `${savedCvs.length} file${savedCvs.length === 1 ? "" : "s"}`;
    const list = document.getElementById("cvList");
    if (!savedCvs.length) { list.innerHTML = '<div class="empty">No CV documents uploaded yet.</div>'; return; }
    list.innerHTML = savedCvs.map(function (cv) {
        return `<div class="list-item cv-card"><div><strong>${escapeJobSeekerHtml(cv.fileName)}</strong><p>${Math.ceil(cv.fileSize / 1024)} KB · Uploaded ${formatJobSeekerDate(cv.uploadedAt)}</p></div><button class="button button-danger button-small" onclick="confirmDeleteCv(${cv.id})">Delete</button></div>`;
    }).join("");
}

async function uploadCvDocument(event) {
    event.preventDefault();
    const input = document.getElementById("cvFile");
    if (!input.files[0]) return;
    const formData = new FormData();
    formData.append("file", input.files[0]);
    const button = document.getElementById("cvSubmit");
    button.disabled = true;
    button.textContent = "Uploading...";
    try {
        const response = await jobSeekerFetch("/api/jobseeker/cv", { method: "POST", body: formData });
        if (!response.ok) throw new Error(await jobSeekerError(response, "CV upload failed."));
        savedCvs.push(await response.json());
        event.target.reset();
        displayCvDocuments();
        showJobSeekerNotice("CV uploaded successfully.");
    } catch (error) { showJobSeekerNotice(error.message, "error"); }
    finally { button.disabled = false; button.textContent = "Upload CV"; }
}

function confirmDeleteCv(cvId) {
    showConfirmationModal({ title: "Delete CV?", message: "Are you sure you want to delete this CV? This action cannot be undone.", confirmText: "Delete", variant: "danger", onConfirm: function () { return deleteCvDocument(cvId); } });
}

async function deleteCvDocument(cvId) {
    try {
        const response = await jobSeekerFetch(`/api/jobseeker/cv/${cvId}`, { method: "DELETE" });
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to delete CV."));
        savedCvs = savedCvs.filter(item => item.id !== cvId);
        displayCvDocuments();
        showJobSeekerNotice("CV deleted.");
    } catch (error) { showJobSeekerNotice(error.message, "error"); }
}
