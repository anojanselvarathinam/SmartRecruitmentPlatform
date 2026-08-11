document.addEventListener("DOMContentLoaded", async function () {
    if (!checkJobSeekerAuthentication()) return;
    try {
        const responses = await Promise.all([
            jobSeekerFetch("/api/jobseeker/profile"),
            jobSeekerFetch("/api/job-matching/applications"),
            jobSeekerFetch("/api/jobseeker/notifications")
        ]);
        if (!responses[0].ok) throw new Error(await jobSeekerError(responses[0], "Unable to load profile."));
        const profile = await responses[0].json();
        const applications = responses[1].ok ? await responses[1].json() : [];
        const notifications = responses[2].ok ? await responses[2].json() : [];
        const seekerName = document.getElementById("topbarUserName") || document.getElementById("seekerName");
        if (seekerName) seekerName.textContent = `${profile.firstName} ${profile.lastName}`.trim();
        document.getElementById("skillCount").textContent = profile.skills.length;
        document.getElementById("applicationCount").textContent = applications.length;
        document.getElementById("notificationCount").textContent = notifications.filter(item => !item.isRead).length;
        const missing = [];
        if (!profile.location) missing.push("location");
        if (!profile.summary) missing.push("summary");
        if (!profile.skills.length) missing.push("skills");
        if (!profile.educations.length) missing.push("education");
        if (!profile.cvDocuments.length) missing.push("CV");
        document.getElementById("readiness").innerHTML = missing.length ? `Complete your ${escapeJobSeekerHtml(missing.join(", "))} to improve matching. <a href="profile.html">Update profile</a>` : "Your profile has the main information needed for job matching.";
    } catch (error) { showJobSeekerNotice(error.message, "error"); document.getElementById("readiness").textContent = "Profile information could not be loaded."; }
});
