let savedSkills = [];
let editingSkillId = 0;

document.addEventListener("DOMContentLoaded", function () {
    if (!checkJobSeekerAuthentication()) return;
    document.getElementById("skillForm").addEventListener("submit", saveSkill);
    document.getElementById("cancelSkillEdit").addEventListener("click", resetSkillForm);
    loadSkills();
});

async function loadSkills() {
    try {
        const response = await jobSeekerFetch("/api/jobseeker/skills");
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to load skills."));
        savedSkills = await response.json();
        displaySkills();
    } catch (error) {
        document.getElementById("skillsList").innerHTML = `<div class="empty">${escapeJobSeekerHtml(error.message)}</div>`;
        showJobSeekerNotice(error.message, "error");
    }
}

function displaySkills() {
    const list = document.getElementById("skillsList");
    document.getElementById("skillTotal").textContent = `${savedSkills.length} skill${savedSkills.length === 1 ? "" : "s"}`;
    if (!savedSkills.length) {
        list.innerHTML = '<div class="empty">No skills saved yet. Add your first skill above.</div>';
        return;
    }
    list.innerHTML = savedSkills.map(function (skill) {
        return `<div class="list-item"><div><strong>${escapeJobSeekerHtml(skill.skillName)}</strong><p>${escapeJobSeekerHtml(skill.skillLevel || "Level not specified")}</p></div><div class="actions"><button class="button button-secondary button-small" onclick="editSkill(${skill.id})">Edit</button><button class="button button-danger button-small" onclick="confirmDeleteSkill(${skill.id})">Delete</button></div></div>`;
    }).join("");
}

async function saveSkill(event) {
    event.preventDefault();
    const payload = { skillName: document.getElementById("skillName").value.trim(), skillLevel: document.getElementById("skillLevel").value || null };
    const url = editingSkillId ? `/api/jobseeker/skills/${editingSkillId}` : "/api/jobseeker/skills";
    const method = editingSkillId ? "PUT" : "POST";
    const button = document.getElementById("skillSubmit");
    button.disabled = true;
    button.textContent = editingSkillId ? "Saving..." : "Adding...";
    try {
        const response = await jobSeekerFetch(url, { method: method, headers: { "Content-Type": "application/json" }, body: JSON.stringify(payload) });
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to save skill."));
        const savedSkill = await response.json();
        if (editingSkillId) savedSkills = savedSkills.map(item => item.id === editingSkillId ? savedSkill : item);
        else savedSkills.push(savedSkill);
        showJobSeekerNotice(editingSkillId ? "Skill updated." : "Skill added.");
        resetSkillForm();
        displaySkills();
    } catch (error) {
        showJobSeekerNotice(error.message, "error");
    } finally {
        button.disabled = false;
        button.textContent = editingSkillId ? "Save Changes" : "Add Skill";
    }
}

function editSkill(skillId) {
    const skill = savedSkills.find(item => item.id === skillId);
    if (!skill) return;
    editingSkillId = skillId;
    document.getElementById("skillName").value = skill.skillName;
    document.getElementById("skillLevel").value = skill.skillLevel || "";
    document.getElementById("skillSubmit").textContent = "Save Changes";
    document.getElementById("cancelSkillEdit").hidden = false;
    document.getElementById("skillName").focus();
}

function resetSkillForm() {
    editingSkillId = 0;
    document.getElementById("skillForm").reset();
    document.getElementById("skillSubmit").textContent = "Add Skill";
    document.getElementById("cancelSkillEdit").hidden = true;
}

function confirmDeleteSkill(skillId) {
    showConfirmationModal({ title: "Delete Skill?", message: "Are you sure you want to delete this skill? This action cannot be undone.", confirmText: "Delete", variant: "danger", onConfirm: function () { return deleteSkill(skillId); } });
}

async function deleteSkill(skillId) {
    try {
        const response = await jobSeekerFetch(`/api/jobseeker/skills/${skillId}`, { method: "DELETE" });
        if (!response.ok) throw new Error(await jobSeekerError(response, "Unable to delete skill."));
        savedSkills = savedSkills.filter(item => item.id !== skillId);
        if (editingSkillId === skillId) resetSkillForm();
        displaySkills();
        showJobSeekerNotice("Skill deleted.");
    } catch (error) { showJobSeekerNotice(error.message, "error"); }
}
