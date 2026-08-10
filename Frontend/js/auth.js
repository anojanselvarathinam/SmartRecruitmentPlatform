/* =========================================
   TALENTSYNC AUTHENTICATION
========================================= */

const API_BASE_URL = "/api";


/* =========================================
   ELEMENTS
========================================= */

const loginForm = document.getElementById("loginForm");
const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");
const rememberMe = document.getElementById("rememberMe");

const loginButton = document.getElementById("loginButton");
const loginButtonText = document.getElementById("loginButtonText");
const loginLoader = document.getElementById("loginLoader");
const loginMessage = document.getElementById("loginMessage");

const togglePassword =
    document.getElementById("togglePassword");


/* =========================================
   PASSWORD SHOW / HIDE
========================================= */

if (togglePassword) {

    togglePassword.addEventListener("click", function () {

        passwordInput.type =
            passwordInput.type === "password"
                ? "text"
                : "password";

    });

}


/* =========================================
   MESSAGE
========================================= */

function showMessage(message, type) {

    loginMessage.textContent = message;

    loginMessage.className =
        `login-message ${type}`;

}


function clearMessage() {

    loginMessage.textContent = "";

    loginMessage.className =
        "login-message";

}


/* =========================================
   LOADING
========================================= */

function setLoading(isLoading) {

    loginButton.disabled = isLoading;

    if (isLoading) {

        loginButtonText.classList.add("hidden");

        loginLoader.classList.remove("hidden");

    } else {

        loginButtonText.classList.remove("hidden");

        loginLoader.classList.add("hidden");

    }

}


/* =========================================
   DECODE JWT
========================================= */

function decodeJwt(token) {

    try {

        const payload =
            token.split(".")[1];

        const base64 =
            payload
                .replace(/-/g, "+")
                .replace(/_/g, "/");

        const jsonPayload =
            decodeURIComponent(
                atob(base64)
                    .split("")
                    .map(function (char) {

                        return "%" +
                            ("00" +
                                char.charCodeAt(0)
                                    .toString(16))
                                .slice(-2);

                    })
                    .join("")
            );

        return JSON.parse(jsonPayload);

    } catch (error) {

        console.error(
            "JWT decode error:",
            error
        );

        return null;

    }

}


/* =========================================
   GET ROLE FROM JWT
========================================= */

function getRoleFromToken(token) {

    const payload =
        decodeJwt(token);

    if (!payload) {
        return null;
    }


    /*
       ASP.NET Core ClaimTypes.Role
       usually becomes this URI claim.
    */

    const roleClaim =
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";


    return (
        payload[roleClaim] ||
        payload.role ||
        payload.Role ||
        null
    );

}


/* =========================================
   SAVE LOGIN
========================================= */

function saveAuthentication(data) {

    const token =
        data?.token;

    if (!token) {

        throw new Error(
            "Login successful, but JWT token was not received."
        );

    }


    const role =
        getRoleFromToken(token);


    if (!role) {

        throw new Error(
            "Login successful, but user role was not found."
        );

    }


    const storage =
        rememberMe.checked
            ? localStorage
            : sessionStorage;


    storage.setItem(
        "talentSyncToken",
        token
    );


    storage.setItem(
        "talentSyncRole",
        role
    );


    storage.setItem(
        "talentSyncUser",
        JSON.stringify(
            decodeJwt(token)
        )
    );


    return {
        token,
        role
    };

}


/* =========================================
   ROLE REDIRECT
========================================= */

function redirectUser(role) {

    const normalizedRole =
        role.toLowerCase();


    if (
        normalizedRole === "jobseeker"
    ) {

        window.location.href =
            "../JobSeeker/dashboard.html";

        return;

    }


    if (
        normalizedRole === "employer"
    ) {

        window.location.href =
            "../Employer/dashboard.html";

        return;

    }


    if (
        normalizedRole === "admin"
    ) {

        window.location.href =
            "../Admin/dashboard.html";

        return;

    }


    showMessage(
        "Unknown user role.",
        "error"
    );

}


/* =========================================
   LOGIN API
========================================= */

async function loginUser(
    email,
    password
) {

    const response =
        await fetch(
            `${API_BASE_URL}/Auth/login`,
            {

                method: "POST",

                headers: {
                    "Content-Type":
                        "application/json"
                },

                body: JSON.stringify({

                    Email: email,

                    Password: password

                })

            }
        );


    let data = null;


    try {

        data =
            await response.json();

    } catch {

        data = null;

    }


    if (!response.ok) {

        throw new Error(

            data?.message ||
            "Invalid email or password."

        );

    }


    return data;

}


/* =========================================
   LOGIN FORM
========================================= */

if (loginForm) {

    loginForm.addEventListener(
        "submit",
        async function (event) {

            event.preventDefault();

            clearMessage();


            const email =
                emailInput.value.trim();

            const password =
                passwordInput.value;


            if (!email) {

                showMessage(
                    "Please enter your email.",
                    "error"
                );

                emailInput.focus();

                return;

            }


            if (!password) {

                showMessage(
                    "Please enter your password.",
                    "error"
                );

                passwordInput.focus();

                return;

            }


            setLoading(true);


            try {

                const data =
                    await loginUser(
                        email,
                        password
                    );


                const auth =
                    saveAuthentication(data);


                showMessage(
                    "Login successful. Redirecting...",
                    "success"
                );


                setTimeout(
                    function () {

                        redirectUser(
                            auth.role
                        );

                    },
                    700
                );


            } catch (error) {

                console.error(
                    "Login error:",
                    error
                );


                showMessage(
                    error.message ||
                    "Unable to login.",
                    "error"
                );


            } finally {

                setLoading(false);

            }

        }
    );

}