let currentUser = null;

document.addEventListener("DOMContentLoaded", function () {
    if (!checkAdminAuthentication()) return;
    loadUserDetails();
    setupAccountButtons();
});

async function loadUserDetails() {
    const userId = Number(new URLSearchParams(window.location.search).get("id"));

    if (!Number.isInteger(userId) || userId <= 0) {
        showAdminAlert("Invalid user ID.", "error");
        return;
    }

    try {
        const response = await adminApiFetch(`/api/admin/users/${userId}`);
        if (response.status === 404) throw new Error("User not found.");
        if (!response.ok) throw new Error("Unable to load user details.");

        currentUser = await response.json();
        displayUserDetails();
    } catch (error) {
        handleAdminApiError(error);
    }
}

function displayUserDetails() {
    const status = currentUser.isActive ? "Active" : "Blocked";
    const role = currentUser.role === "JobSeeker" ? "Job Seeker" : currentUser.role;

    document.getElementById("userName").textContent = currentUser.name;
    document.getElementById("userEmail").textContent = currentUser.email;
    document.getElementById("detailName").textContent = currentUser.name;
    document.getElementById("detailEmail").textContent = currentUser.email;
    document.getElementById("detailRole").textContent = role;
    document.getElementById("detailStatus").textContent = status;
    document.getElementById("userAvatar").textContent = currentUser.name.charAt(0).toUpperCase();
    const detailsCard = document.querySelector(".user-details-card");
    if (detailsCard) {
        detailsCard.classList.remove("user-role-employer", "user-role-jobseeker");
        detailsCard.classList.add(currentUser.role === "Employer" ? "user-role-employer" : "user-role-jobseeker");
    }
    document.getElementById("activateButton").disabled = currentUser.isActive;
    document.getElementById("blockButton").disabled = !currentUser.isActive;
}

function setupAccountButtons() {
    document.getElementById("activateButton").addEventListener("click", function () {
        showConfirmationModal({
            title: "Activate Account?",
            message: "Are you sure you want to activate this account?",
            confirmText: "Activate",
            variant: "success",
            onConfirm: function () { return updateAccountStatus(true); }
        });
    });

    document.getElementById("blockButton").addEventListener("click", function () {
        showConfirmationModal({
            title: "Block Account?",
            message: "Are you sure you want to block this account?",
            confirmText: "Block",
            variant: "danger",
            onConfirm: function () { return updateAccountStatus(false); }
        });
    });
}

async function updateAccountStatus(isActive) {
    if (!currentUser) return;

    try {
        const response = await adminApiFetch(`/api/admin/users/${currentUser.id}/status`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ isActive: isActive })
        });

        if (!response.ok) throw new Error("Unable to update account status.");

        currentUser.isActive = isActive;
        displayUserDetails();
        showAdminAlert(isActive
            ? "User account activated successfully."
            : "User account blocked successfully.", "success");
    } catch (error) {
        handleAdminApiError(error);
    }
}

function goBackToUsers() {
    window.location.href = "users.html";
}
