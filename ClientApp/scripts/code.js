let jwtToken = null;

// Login form submit
document.getElementById('loginForm').addEventListener('submit', function(e) {
    e.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    login(username, password);
});

// Login functie
function login(username, password) {
    fetch('https://localhost:7192/api/login', { // Pas URL aan naar jouw API
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Login mislukt');
            }
            return response.json();
        })
        .then(data => {
            jwtToken = data.token; // veronderstel dat API { "token": "..." } terugstuurt
            alert('Login succesvol!');
            document.getElementById('getEmployeesBtn').disabled = false;
        })
        .catch(error => {
            console.error('Fout bij login:', error);
            alert('Login mislukt. Controleer je gegevens.');
        });
}

// Employees ophalen
document.getElementById('getEmployeesBtn').addEventListener('click', function() {
    getEmployees();
});

function getEmployees() {
    fetch('https://localhost:5001/api/employees', { // Pas URL aan
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + jwtToken
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Kan employees niet ophalen');
            }
            return response.json();
        })
        .then(data => {
            const list = document.getElementById('employeeList');
            list.innerHTML = '';
            data.forEach(emp => {
                const li = document.createElement('li');
                li.textContent = `${emp.firstName} ${emp.lastName} (${emp.email})`;
                list.appendChild(li);
            });
        })
        .catch(error => {
            console.error('Fout bij ophalen employees:', error);
            alert('Kan employees niet ophalen');
        });
}

const setup = () => {
    $('#btnSender').on('click', () => {
        let email = $('#email').val();
        let password = $('#password').val();
        let account = {"email": email, "password": password};
        login(JSON.stringify(account));
    });
}

const login = (jsonData) => {
    const url = 'https://localhost:5001/api/employees'
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: jsonData})
        .then(response => {
            if (!response.ok) {
                console.log('Gegevens succesvol verzonden.');
            }
        })
}