let currentCompany = null;

document.addEventListener("DOMContentLoaded", function () {
    if (!checkEmployerAuthentication()) return;
    loadCompany();
    document.getElementById("companyForm").addEventListener("submit", saveCompany);
});

async function loadCompany() {
    try {
        const response = await employerFetch(`/api/Company/employer/${getEmployerId()}`);
        if (response.status === 404) return;
        if (!response.ok) throw new Error(await readApiError(response, "Unable to load company."));

        currentCompany = await response.json();
        document.getElementById("companyName").value = currentCompany.companyName;
        document.getElementById("industry").value = currentCompany.industry;
        document.getElementById("location").value = currentCompany.location;
        document.getElementById("website").value = currentCompany.website || "";
        document.getElementById("description").value = currentCompany.description || "";
        document.getElementById("companySubmit").textContent = "Update Company";
    } catch (error) {
        showEmployerNotice(error.message, "error");
    }
}

async function saveCompany(event) {
    event.preventDefault();
    const button = document.getElementById("companySubmit");
    const payload = {
        companyName: document.getElementById("companyName").value.trim(),
        industry: document.getElementById("industry").value.trim(),
        location: document.getElementById("location").value.trim(),
        website: document.getElementById("website").value.trim(),
        description: document.getElementById("description").value.trim()
    };

    if (!payload.companyName || !payload.industry || !payload.location) {
        showEmployerNotice("Company name, industry and location are required.", "error");
        return;
    }

    button.disabled = true;
    button.textContent = "Saving...";
    try {
        const url = currentCompany ? `/api/Company/${currentCompany.companyId}` : "/api/Company";
        const response = await employerFetch(url, {
            method: currentCompany ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });
        if (!response.ok) throw new Error(await readApiError(response, "Unable to save company."));
        currentCompany = await response.json();
        showEmployerNotice("Company information saved successfully.");
        button.textContent = "Update Company";
    } catch (error) {
        showEmployerNotice(error.message, "error");
    } finally {
        button.disabled = false;
        if (!currentCompany) button.textContent = "Save Company";
    }
}
