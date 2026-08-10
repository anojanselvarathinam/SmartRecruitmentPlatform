/* =========================================
   USER MANAGEMENT JS
   ========================================= */

let allUsers = [];


document.addEventListener("DOMContentLoaded", function () {

    checkAdminAuthentication();

    loadUsers();

    setupSearch();

    setupRoleFilter();

});


/* =========================================
   DEMO USERS
   ========================================= */

function loadUsers() {

    /*
        Temporary data.

        Later replace with backend API.
    */

    allUsers = [

        {
            id: 1,
            name: "John Silva",
            email: "john@example.com",
            role: "Job Seeker",
            status: "Active"
        },

        {
            id: 2,
            name: "ABC Technologies",
            email: "hr@abc.com",
            role: "Employer",
            status: "Active"
        },

        {
            id: 3,
            name: "Kamal Perera",
            email: "kamal@example.com",
            role: "Job Seeker",
            status: "Blocked"
        },

        {
            id: 4,
            name: "XYZ Solutions",
            email: "hr@xyz.com",
            role: "Employer",
            status: "Active"
        }

    ];


    displayUsers(allUsers);

}


/* =========================================
   DISPLAY USERS
   ========================================= */

function displayUsers(users) {

    const tableBody =
        document.getElementById("usersTableBody");

    const noUsersMessage =
        document.getElementById("noUsersMessage");


    tableBody.innerHTML = "";


    if (users.length === 0) {

        noUsersMessage.style.display = "block";

        return;
    }


    noUsersMessage.style.display = "none";


    users.forEach(function (user) {

        const row =
            document.createElement("tr");


        const roleClass =
            user.role === "Employer"
                ? "role-employer"
                : "role-jobseeker";


        const statusClass =
            user.status === "Active"
                ? "status-active"
                : "status-blocked";


        row.innerHTML = `

            <td>
                ${escapeHtml(user.name)}
            </td>

            <td>
                ${escapeHtml(user.email)}
            </td>

            <td>
                <span class="role-badge ${roleClass}">
                    ${escapeHtml(user.role)}
                </span>
            </td>

            <td>
                <span class="status-badge ${statusClass}">
                    ${escapeHtml(user.status)}
                </span>
            </td>

            <td>

                <button
                    class="view-user-btn"
                    onclick="viewUser(${user.id})">

                    View

                </button>

            </td>

        `;


        tableBody.appendChild(row);

    });

}


/* =========================================
   SEARCH
   ========================================= */

function setupSearch() {

    const searchInput =
        document.getElementById("userSearch");


    searchInput.addEventListener(
        "input",
        applyFilters
    );

}


/* =========================================
   ROLE FILTER
   ========================================= */

function setupRoleFilter() {

    const roleFilter =
        document.getElementById("roleFilter");


    roleFilter.addEventListener(
        "change",
        applyFilters
    );

}


/* =========================================
   APPLY FILTERS
   ========================================= */

function applyFilters() {

    const searchValue =
        document.getElementById("userSearch")
            .value
            .toLowerCase()
            .trim();


    const roleValue =
        document.getElementById("roleFilter")
            .value;


    const filteredUsers =
        allUsers.filter(function (user) {


            const matchesSearch =
                user.name
                    .toLowerCase()
                    .includes(searchValue)
                ||
                user.email
                    .toLowerCase()
                    .includes(searchValue);


            const matchesRole =
                roleValue === "all"
                ||
                user.role === roleValue;


            return matchesSearch &&
                matchesRole;

        });


    displayUsers(filteredUsers);

}


/* =========================================
   VIEW USER
   ========================================= */

function viewUser(userId) {

    window.location.href =
        `user-details.html?id=${userId}`;

}


/* =========================================
   HTML SECURITY
   ========================================= */

function escapeHtml(value) {

    const div =
        document.createElement("div");

    div.textContent = value;

    return div.innerHTML;

}