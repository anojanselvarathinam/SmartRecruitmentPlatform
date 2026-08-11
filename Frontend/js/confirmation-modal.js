let activeConfirmationModal = null;

function createConfirmationModal() {
    const backdrop = document.createElement("div");
    backdrop.className = "confirmation-backdrop";
    backdrop.setAttribute("aria-hidden", "true");
    backdrop.innerHTML = `
        <div class="confirmation-modal" role="dialog" aria-modal="true" aria-labelledby="confirmationTitle" aria-describedby="confirmationMessage">
            <h2 id="confirmationTitle"></h2>
            <p id="confirmationMessage"></p>
            <div class="confirmation-actions">
                <button type="button" class="confirmation-button confirmation-cancel">Cancel</button>
                <button type="button" class="confirmation-button confirmation-confirm"></button>
            </div>
        </div>`;
    document.body.appendChild(backdrop);
    return backdrop;
}

function closeConfirmationModal() {
    if (!activeConfirmationModal || activeConfirmationModal.isProcessing) return;
    activeConfirmationModal.backdrop.classList.remove("show");
    activeConfirmationModal.backdrop.setAttribute("aria-hidden", "true");
    document.body.classList.remove("confirmation-open");
    if (activeConfirmationModal.previousFocus) activeConfirmationModal.previousFocus.focus();
    activeConfirmationModal = null;
}

function showConfirmationModal(options) {
    if (activeConfirmationModal) return;
    let backdrop = document.querySelector(".confirmation-backdrop");
    if (!backdrop) backdrop = createConfirmationModal();

    const title = backdrop.querySelector("#confirmationTitle");
    const message = backdrop.querySelector("#confirmationMessage");
    const cancelButton = backdrop.querySelector(".confirmation-cancel");
    const confirmButton = backdrop.querySelector(".confirmation-confirm");
    const confirmText = options.confirmText || "Confirm";

    title.textContent = options.title || "Confirm Action";
    message.textContent = options.message || "Are you sure you want to continue?";
    confirmButton.textContent = confirmText;
    confirmButton.className = "confirmation-button confirmation-confirm " +
        (options.variant === "success" ? "confirmation-success" : "confirmation-danger");
    confirmButton.disabled = false;
    cancelButton.disabled = false;
    activeConfirmationModal = { backdrop: backdrop, previousFocus: document.activeElement, isProcessing: false };

    function cancel() { closeConfirmationModal(); }

    async function confirmAction() {
        if (!activeConfirmationModal || activeConfirmationModal.isProcessing) return;
        activeConfirmationModal.isProcessing = true;
        confirmButton.disabled = true;
        cancelButton.disabled = true;
        confirmButton.textContent = confirmText + "...";
        try {
            await options.onConfirm();
        } finally {
            activeConfirmationModal.isProcessing = false;
            closeConfirmationModal();
        }
    }

    backdrop.onclick = function (event) { if (event.target === backdrop) cancel(); };
    cancelButton.onclick = cancel;
    confirmButton.onclick = confirmAction;
    backdrop.classList.add("show");
    backdrop.setAttribute("aria-hidden", "false");
    document.body.classList.add("confirmation-open");
    cancelButton.focus();
}

document.addEventListener("keydown", function (event) {
    if (event.key === "Escape" && activeConfirmationModal) closeConfirmationModal();
});
