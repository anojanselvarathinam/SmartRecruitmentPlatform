const API_BASE_URL = "/api";

const loginForm = document.getElementById("loginForm");
const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");
const rememberMe = document.getElementById("rememberMe");
const loginButton = document.getElementById("loginButton");
const loginButtonText = document.getElementById("loginButtonText");
const loginLoader = document.getElementById("loginLoader");
const loginMessage = document.getElementById("loginMessage");
const togglePassword = document.getElementById("togglePassword");

togglePassword?.addEventListener("click", () => {
    passwordInput.type = passwordInput.type === "password" ? "text" : "password";
});

function showMessage(message, type) {
    loginMessage.textContent = message;
    loginMessage.className = `login-message ${type}`;
}

function clearMessage() {
    loginMessage.textContent = "";
    loginMessage.className = "login-message";
}

function setLoading(isLoading) {
    loginButton.disabled = isLoading;
    loginButtonText.classList.toggle("hidden", isLoading);
    loginLoader.classList.toggle("hidden", !isLoading);
}

function decodeJwt(token) {
    try {
        const base64 = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
        const json = decodeURIComponent(atob(base64).split("").map(char =>
            `%${(`00${char.charCodeAt(0).toString(16)}`).slice(-2)}`
        ).join(""));
        return JSON.parse(json);
    } catch (error) {
        console.error("JWT decode error:", error);
        return null;
    }
}

function getRoleFromToken(token) {
    const payload = decodeJwt(token);
    const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    return payload?.[roleClaim] || payload?.role || payload?.Role || null;
}

function saveAuthentication(data) {
    const token = data?.token;
    if (!token) throw new Error("Login successful, but JWT token was not received.");
    const role = getRoleFromToken(token);
    if (!role) throw new Error("Login successful, but user role was not found.");
    const storage = rememberMe.checked ? localStorage : sessionStorage;
    storage.setItem("talentSyncToken", token);
    storage.setItem("talentSyncRole", role);
    storage.setItem("talentSyncUser", JSON.stringify(decodeJwt(token)));
    return { token, role };
}

function redirectUser(role) {
    const routes = {
        jobseeker: "../JobSeeker/dashboard.html",
        employer: "../Employer/dashboard.html",
        admin: "../Admin/dashboard.html"
    };
    const route = routes[role.toLowerCase()];
    if (route) window.location.href = route;
    else showMessage("Unknown user role.", "error");
}

async function loginUser(email, password) {
    const response = await fetch(`${API_BASE_URL}/Auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Email: email, Password: password })
    });
    let data = null;
    try { data = await response.json(); } catch { data = null; }
    if (!response.ok) throw new Error(data?.message || "Invalid email or password.");
    return data;
}

loginForm?.addEventListener("submit", async event => {
    event.preventDefault();
    clearMessage();
    const email = emailInput.value.trim();
    const password = passwordInput.value;
    if (!email) { showMessage("Please enter your email.", "error"); emailInput.focus(); return; }
    if (!password) { showMessage("Please enter your password.", "error"); passwordInput.focus(); return; }
    setLoading(true);
    try {
        const auth = saveAuthentication(await loginUser(email, password));
        showMessage("Login successful. Redirecting...", "success");
        setTimeout(() => redirectUser(auth.role), 700);
    } catch (error) {
        console.error("Login error:", error);
        showMessage(error.message || "Unable to login.", "error");
    } finally {
        setLoading(false);
    }
});
