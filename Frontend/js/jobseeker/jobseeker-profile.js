let seekerProfile = null;
const firstName = document.getElementById("firstName");
const lastName = document.getElementById("lastName");
const phone = document.getElementById("phone");
const profileLocation = document.getElementById("location");
const summary = document.getElementById("summary");
const skillName = document.getElementById("skillName");
const skillLevel = document.getElementById("skillLevel");
const institution = document.getElementById("institution");
const degree = document.getElementById("degree");
const fieldOfStudy = document.getElementById("fieldOfStudy");
const educationStart = document.getElementById("educationStart");
const educationEnd = document.getElementById("educationEnd");
const experienceCompany = document.getElementById("experienceCompany");
const experienceTitle = document.getElementById("experienceTitle");
const experienceStart = document.getElementById("experienceStart");
const experienceEnd = document.getElementById("experienceEnd");
const experienceDescription = document.getElementById("experienceDescription");
const cvFile = document.getElementById("cvFile");

document.addEventListener("DOMContentLoaded", function () {
    if (!checkJobSeekerAuthentication()) return;
    loadSeekerProfile();
    document.getElementById("profileForm").addEventListener("submit", saveProfile);
    document.getElementById("skillForm").addEventListener("submit", addSkill);
    document.getElementById("educationForm").addEventListener("submit", addEducation);
    document.getElementById("experienceForm").addEventListener("submit", addExperience);
    document.getElementById("cvForm").addEventListener("submit", uploadCv);
});

async function loadSeekerProfile() {
    try {
        const response = await jobSeekerFetch("/api/jobseeker/profile");
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to load profile."));
        seekerProfile = await response.json();
        ["firstName","lastName","phone","location","summary"].forEach(id => document.getElementById(id).value = seekerProfile[id] || "");
        renderProfileLists();
    } catch (error) { showJobSeekerNotice(error.message, "error"); }
}

function renderProfileLists() {
    renderList("skillsList", seekerProfile.skills, item => `<div><strong>${escapeJobSeekerHtml(item.skillName)}</strong><p>${escapeJobSeekerHtml(item.skillLevel || "Level not specified")}</p></div><button class="button button-danger button-small" onclick="deleteProfileItem('skills',${item.id})">Delete</button>`);
    renderList("educationList", seekerProfile.educations, item => `<div><strong>${escapeJobSeekerHtml(item.degree)}</strong><p>${escapeJobSeekerHtml(item.institution)} · ${formatJobSeekerDate(item.startDate)}–${formatJobSeekerDate(item.endDate)}</p></div><button class="button button-danger button-small" onclick="deleteProfileItem('education',${item.id})">Delete</button>`);
    renderList("experienceList", seekerProfile.experiences, item => `<div><strong>${escapeJobSeekerHtml(item.jobTitle)}</strong><p>${escapeJobSeekerHtml(item.companyName)} · ${formatJobSeekerDate(item.startDate)}–${formatJobSeekerDate(item.endDate)}</p></div><button class="button button-danger button-small" onclick="deleteProfileItem('experience',${item.id})">Delete</button>`);
    renderList("cvList", seekerProfile.cvDocuments, item => `<div><strong>${escapeJobSeekerHtml(item.fileName)}</strong><p>${Math.ceil(item.fileSize / 1024)} KB · ${formatJobSeekerDate(item.uploadedAt)}</p></div><button class="button button-danger button-small" onclick="deleteProfileItem('cv',${item.id})">Delete</button>`);
}

function renderList(id, items, template) { document.getElementById(id).innerHTML = items.length ? items.map(item => `<div class="list-item">${template(item)}</div>`).join("") : '<div class="empty">No records added.</div>'; }

async function saveProfile(event) { event.preventDefault(); await sendProfileRequest("/api/jobseeker/profile", "PUT", { firstName: firstName.value.trim(), lastName: lastName.value.trim(), phone: phone.value.trim(), location: profileLocation.value.trim(), summary: summary.value.trim() }, "Profile saved."); }
async function addSkill(event) { event.preventDefault(); const data = await sendProfileRequest("/api/jobseeker/skills", "POST", { skillName: skillName.value.trim(), skillLevel: skillLevel.value || null }, "Skill added."); if (data) { seekerProfile.skills.push(data); event.target.reset(); renderProfileLists(); } }
async function addEducation(event) { event.preventDefault(); const payload = { institution: institution.value.trim(), degree: degree.value.trim(), fieldOfStudy: fieldOfStudy.value.trim() || null, startDate: educationStart.value, endDate: educationEnd.value || null }; if (payload.endDate && payload.endDate < payload.startDate) return showJobSeekerNotice("Education end date cannot be before start date.", "error"); const data = await sendProfileRequest("/api/jobseeker/education", "POST", payload, "Education added."); if (data) { seekerProfile.educations.push(data); event.target.reset(); renderProfileLists(); } }
async function addExperience(event) { event.preventDefault(); const payload = { companyName: experienceCompany.value.trim(), jobTitle: experienceTitle.value.trim(), description: experienceDescription.value.trim() || null, startDate: experienceStart.value, endDate: experienceEnd.value || null }; if (payload.endDate && payload.endDate < payload.startDate) return showJobSeekerNotice("Experience end date cannot be before start date.", "error"); const data = await sendProfileRequest("/api/jobseeker/experience", "POST", payload, "Experience added."); if (data) { seekerProfile.experiences.push(data); event.target.reset(); renderProfileLists(); } }

async function uploadCv(event) { event.preventDefault(); const file = cvFile.files[0]; if (!file) return; const form = new FormData(); form.append("file", file); try { const response = await jobSeekerFetch("/api/jobseeker/cv", { method: "POST", body: form }); if (!response.ok) throw new Error(await jobSeekerError(response, "CV upload failed.")); seekerProfile.cvDocuments.push(await response.json()); event.target.reset(); renderProfileLists(); showJobSeekerNotice("CV uploaded."); } catch (error) { showJobSeekerNotice(error.message, "error"); } }

async function deleteProfileItem(type, id) { if (!confirm("Delete this record?")) return; try { const response = await jobSeekerFetch(`/api/jobseeker/${type}/${id}`, { method: "DELETE" }); if (!response.ok) throw new Error(await jobSeekerError(response, "Delete failed.")); const property = type === "skills" ? "skills" : type === "education" ? "educations" : type === "experience" ? "experiences" : "cvDocuments"; seekerProfile[property] = seekerProfile[property].filter(item => item.id !== id); renderProfileLists(); showJobSeekerNotice("Record deleted."); } catch (error) { showJobSeekerNotice(error.message, "error"); } }

async function sendProfileRequest(url, method, payload, message) { try { const response = await jobSeekerFetch(url, { method, headers: { "Content-Type": "application/json" }, body: JSON.stringify(payload) }); if (!response.ok) throw new Error(await jobSeekerError(response, "Request failed.")); const data = await response.json(); if (url.endsWith("/profile")) seekerProfile = data; showJobSeekerNotice(message); return data; } catch (error) { showJobSeekerNotice(error.message, "error"); return null; } }
