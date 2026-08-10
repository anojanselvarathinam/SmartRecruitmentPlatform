let allUsers = [];

document.addEventListener("DOMContentLoaded", function () {
    if (!checkAdminAuthentication()) return;
    loadUsers();
    setupSearch();
    setupRoleFilter();
});

async function loadUsers() {
    try {
        const response = await adminApiFetch("/api/admin/users");
        if (!response.ok) throw new Error("Unable to load users.");

        allUsers = await response.json();
        displayUsers(allUsers);
    } catch (error) {
        handleAdminApiError(error);
    }
}

function displayUsers(users) {
    const tableBody = document.getElementById("usersTableBody");
    const noUsersMessage = document.getElementById("noUsersMessage");
    tableBody.innerHTML = "";

    if (users.length === 0) {
        noUsersMessage.style.display = "block";
        return;
    }

    noUsersMessage.style.display = "none";

    users.forEach(function (user) {
        const row = document.createElement("tr");
        const roleClass = user.role === "Employer"
            ? "role-employer"
            : "role-jobseeker";
        const status = user.isActive ? "Active" : "Blocked";
        const statusClass = user.isActive ? "status-active" : "status-blocked";

        row.innerHTML = `
            <td>${escapeHtml(user.name)}</td>
            <td>${escapeHtml(user.email)}</td>
            <td><span class="role-badge ${roleClass}">${escapeHtml(formatRole(user.role))}</span></td>
            <td><span class="status-badge ${statusClass}">${status}</span></td>
            <td><button class="view-user-btn" onclick="viewUser(${user.id})">View</button></td>`;

        tableBody.appendChild(row);
    });
}

function setupSearch() {
    document.getElementById("userSearch").addEventListener("input", applyFilters);
}

function setupRoleFilter() {
    document.getElementById("roleFilter").addEventListener("change", applyFilters);
}

function applyFilters() {
    const searchValue = document.getElementById("userSearch").value.toLowerCase().trim();
    const roleValue = document.getElementById("roleFilter").value;

    const filteredUsers = allUsers.filter(function (user) {
        const matchesSearch = user.name.toLowerCase().includes(searchValue) ||
            user.email.toLowerCase().includes(searchValue);
        const matchesRole = roleValue === "all" || user.role === roleValue;
        return matchesSearch && matchesRole;
    });

    displayUsers(filteredUsers);
}

function formatRole(role) {
    return role === "JobSeeker" ? "Job Seeker" : role;
}

function viewUser(userId) {
    window.location.href = `user-details.html?id=${userId}`;
}

function escapeHtml(value) {
    const div = document.createElement("div");
    div.textContent = value ?? "";
    return div.innerHTML;
}
