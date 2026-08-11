document.addEventListener("DOMContentLoaded", function () {
    if (!checkAdminAuthentication()) return;
    const admin = getAdminAccountInformation();
    document.getElementById("adminProfileAvatar").textContent = admin.initial;
    document.getElementById("adminProfileName").textContent = admin.name;
    document.getElementById("adminAccountName").textContent = admin.name;
    document.getElementById("adminAccountEmail").textContent = admin.email;
    document.getElementById("adminAccountRole").textContent = admin.role;
});
