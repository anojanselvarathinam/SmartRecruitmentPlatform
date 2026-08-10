/* =====================================================
   TALENTSYNC REGISTER
   Connected to ASP.NET Core AuthController
===================================================== */


/*
    Your backend launchSettings.json:

    HTTPS:
    https://localhost:7136

    HTTP:
    http://localhost:5090

    Therefore API:
    https://localhost:7136/api/Auth/register
*/

const API_BASE_URL = "/api";


/* =====================================================
   ELEMENTS
===================================================== */

const registerForm =
    document.getElementById("registerForm");

const fullNameInput =
    document.getElementById("fullName");

const emailInput =
    document.getElementById("email");

const passwordInput =
    document.getElementById("password");

const confirmPasswordInput =
    document.getElementById("confirmPassword");

const termsInput =
    document.getElementById("terms");

const registerButton =
    document.getElementById("registerButton");

const registerButtonText =
    document.getElementById("registerButtonText");

const registerLoader =
    document.getElementById("registerLoader");

const registerMessage =
    document.getElementById("registerMessage");


/* =====================================================
   PASSWORD TOGGLE
===================================================== */

const togglePassword =
    document.getElementById("togglePassword");

const toggleConfirmPassword =
    document.getElementById(
        "toggleConfirmPassword"
    );


if (togglePassword) {

    togglePassword.addEventListener(
        "click",
        function () {

            if (
                passwordInput.type === "password"
            ) {

                passwordInput.type = "text";

            } else {

                passwordInput.type = "password";

            }

        }
    );
}


if (toggleConfirmPassword) {

    toggleConfirmPassword.addEventListener(
        "click",
        function () {

            if (
                confirmPasswordInput.type === "password"
            ) {

                confirmPasswordInput.type = "text";

            } else {

                confirmPasswordInput.type = "password";

            }

        }
    );
}


/* =====================================================
   MESSAGE
===================================================== */

function showRegisterMessage(
    message,
    type
) {

    registerMessage.textContent =
        message;

    registerMessage.className =
        `login-message ${type}`;
}


function clearRegisterMessage() {

    registerMessage.textContent = "";

    registerMessage.className =
        "login-message";
}


/* =====================================================
   LOADING
===================================================== */

function setRegisterLoading(
    loading
) {

    registerButton.disabled =
        loading;


    if (loading) {

        registerButtonText
            .classList
            .add("hidden");

        registerLoader
            .classList
            .remove("hidden");

    } else {

        registerButtonText
            .classList
            .remove("hidden");

        registerLoader
            .classList
            .add("hidden");

    }

}


/* =====================================================
   GET SELECTED ROLE
===================================================== */

function getSelectedRole() {

    const selectedRole =
        document.querySelector(
            'input[name="role"]:checked'
        );

    return selectedRole
        ? selectedRole.value
        : null;
}


/* =====================================================
   EMAIL VALIDATION
===================================================== */

function isValidEmail(email) {

    const emailPattern =
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    return emailPattern.test(email);
}


/* =====================================================
   REGISTER API
===================================================== */

async function registerUser(
    fullName,
    email,
    password,
    role
) {

    const response =
        await fetch(
            `${API_BASE_URL}/Auth/register`,
            {

                method: "POST",

                headers: {
                    "Content-Type":
                        "application/json"
                },

                body: JSON.stringify({

                    /*
                       These property names are
                       EXACTLY from your backend
                       RegisterDto.cs
                    */

                    FullName:
                        fullName,

                    Email:
                        email,

                    Password:
                        password,

                    Role:
                        role

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

            "Registration failed. Please try again."

        );

    }


    return data;
}


/* =====================================================
   FORM SUBMIT
===================================================== */

if (registerForm) {

    registerForm.addEventListener(
        "submit",
        async function (event) {

            event.preventDefault();


            clearRegisterMessage();


            /* -----------------------------------------
               Get values
            ----------------------------------------- */

            const fullName =
                fullNameInput.value.trim();

            const email =
                emailInput.value.trim();

            const password =
                passwordInput.value;

            const confirmPassword =
                confirmPasswordInput.value;

            const role =
                getSelectedRole();


            /* -----------------------------------------
               Full Name validation
            ----------------------------------------- */

            if (!fullName) {

                showRegisterMessage(
                    "Please enter your full name.",
                    "error"
                );

                fullNameInput.focus();

                return;
            }


            /* -----------------------------------------
               Email validation
            ----------------------------------------- */

            if (!email) {

                showRegisterMessage(
                    "Please enter your email.",
                    "error"
                );

                emailInput.focus();

                return;
            }


            if (!isValidEmail(email)) {

                showRegisterMessage(
                    "Please enter a valid email address.",
                    "error"
                );

                emailInput.focus();

                return;
            }


            /* -----------------------------------------
               Password validation
            ----------------------------------------- */

            if (!password) {

                showRegisterMessage(
                    "Please create a password.",
                    "error"
                );

                passwordInput.focus();

                return;
            }


            if (password.length < 6) {

                showRegisterMessage(
                    "Password must contain at least 6 characters.",
                    "error"
                );

                passwordInput.focus();

                return;
            }


            /* -----------------------------------------
               Confirm password
            ----------------------------------------- */

            if (
                password !==
                confirmPassword
            ) {

                showRegisterMessage(
                    "Passwords do not match.",
                    "error"
                );

                confirmPasswordInput.focus();

                return;
            }


            /* -----------------------------------------
               Role validation
            ----------------------------------------- */

            if (!role) {

                showRegisterMessage(
                    "Please select Job Seeker or Employer.",
                    "error"
                );

                return;
            }


            /* -----------------------------------------
               Terms
            ----------------------------------------- */

            if (!termsInput.checked) {

                showRegisterMessage(
                    "Please agree to the Terms & Conditions and Privacy Policy.",
                    "error"
                );

                return;
            }


            /* -----------------------------------------
               Start loading
            ----------------------------------------- */

            setRegisterLoading(true);


            try {


                /* -------------------------------------
                   Call backend
                ------------------------------------- */

                const result =
                    await registerUser(
                        fullName,
                        email,
                        password,
                        role
                    );


                console.log(
                    "Register response:",
                    result
                );


                /* -------------------------------------
                   Success
                ------------------------------------- */

                showRegisterMessage(
                    "Account created successfully! Redirecting to login...",
                    "success"
                );


                /*
                   Backend only creates the account.
                   It does NOT automatically return JWT.

                   Therefore redirect to Login.
                */

                setTimeout(
                    function () {

                        window.location.href =
                            "login.html";

                    },
                    1200
                );


            } catch (error) {


                console.error(
                    "Registration error:",
                    error
                );


                showRegisterMessage(
                    error.message ||
                    "Unable to create your account.",
                    "error"
                );


            } finally {

                setRegisterLoading(false);

            }

        }
    );

}