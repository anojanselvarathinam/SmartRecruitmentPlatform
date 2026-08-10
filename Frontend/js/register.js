const API_BASE_URL = "/api";

const registerForm = document.getElementById("registerForm");
const fullNameInput = document.getElementById("fullName");
const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");
const confirmPasswordInput = document.getElementById("confirmPassword");
const termsInput = document.getElementById("terms");
const registerButton = document.getElementById("registerButton");
const registerButtonText = document.getElementById("registerButtonText");
const registerLoader = document.getElementById("registerLoader");
const registerMessage = document.getElementById("registerMessage");

function addPasswordToggle(toggleId, input) {
    document.getElementById(toggleId)?.addEventListener("click", () => {
        input.type = input.type === "password" ? "text" : "password";
    });
}
addPasswordToggle("togglePassword", passwordInput);
addPasswordToggle("toggleConfirmPassword", confirmPasswordInput);

function showRegisterMessage(message, type) {
    registerMessage.textContent = message;
    registerMessage.className = `login-message ${type}`;
}
function clearRegisterMessage() {
    registerMessage.textContent = "";
    registerMessage.className = "login-message";
}
function setRegisterLoading(loading) {
    registerButton.disabled = loading;
    registerButtonText.classList.toggle("hidden", loading);
    registerLoader.classList.toggle("hidden", !loading);
}
function getSelectedRole() {
    return document.querySelector('input[name="role"]:checked')?.value || null;
}
function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

async function registerUser(fullName, email, password, role) {
    const response = await fetch(`${API_BASE_URL}/Auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ FullName: fullName, Email: email, Password: password, Role: role })
    });
    let data = null;
    try { data = await response.json(); } catch { data = null; }
    if (!response.ok) throw new Error(data?.message || "Registration failed. Please try again.");
    return data;
}

registerForm?.addEventListener("submit", async event => {
    event.preventDefault();
    clearRegisterMessage();
    const fullName = fullNameInput.value.trim();
    const email = emailInput.value.trim();
    const password = passwordInput.value;
    const confirmPassword = confirmPasswordInput.value;
    const role = getSelectedRole();

    if (!fullName) { showRegisterMessage("Please enter your full name.", "error"); fullNameInput.focus(); return; }
    if (!email) { showRegisterMessage("Please enter your email.", "error"); emailInput.focus(); return; }
    if (!isValidEmail(email)) { showRegisterMessage("Please enter a valid email address.", "error"); emailInput.focus(); return; }
    if (!password) { showRegisterMessage("Please create a password.", "error"); passwordInput.focus(); return; }
    if (password.length < 6) { showRegisterMessage("Password must contain at least 6 characters.", "error"); passwordInput.focus(); return; }
    if (password !== confirmPassword) { showRegisterMessage("Passwords do not match.", "error"); confirmPasswordInput.focus(); return; }
    if (!role) { showRegisterMessage("Please select Job Seeker or Employer.", "error"); return; }
    if (!termsInput.checked) { showRegisterMessage("Please agree to the Terms & Conditions and Privacy Policy.", "error"); return; }

    setRegisterLoading(true);
    try {
        await registerUser(fullName, email, password, role);
        showRegisterMessage("Account created successfully! Redirecting to login...", "success");
        setTimeout(() => {
            window.location.href = "login.html";
        }, 1200);
    } catch (error) {
        console.error("Registration error:", error);
        showRegisterMessage(error.message || "Unable to create your account.", "error");
    } finally {
        setRegisterLoading(false);
    }
});
