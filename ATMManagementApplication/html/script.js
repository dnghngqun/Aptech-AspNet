let token = '';
let customerId = 0;

async function login() {
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const response = await fetch('http://localhost:5098/api/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email, password })
    });

    const data = await response.json();
    if (response.ok) {
        token = data.token;
        customerId = data.customerId;
        document.getElementById('auth-section').style.display = 'none';
        document.getElementById('user-section').style.display = 'block';
        document.getElementById('customer-info').innerText = `ID: ${customerId}, Email: ${email}`;
    } else {
        alert(data.message);
    }
}

async function getBalance() {
    const response = await fetch(`http://localhost:5098/api/atm/balance/${customerId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const data = await response.json();
    if (response.ok) {
        alert(`Số dư: ${data.balance}`);
    } else {
        alert(data.message);
    }
}

async function getTransactionHistory() {
    const response = await fetch(`http://localhost:5098/api/atm/transaction/${customerId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    const data = await response.json();
    if (response.ok) {
        alert(JSON.stringify(data));
    } else {
        alert(data.message);
    }
}

async function requestOtp() {
    const response = await fetch('http://localhost:5098/api/atm/request-otp', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ customerId })
    });

    const data = await response.json();
    alert(data.message);
}

async function withdraw() {
    const amount = document.getElementById('withdraw-amount').value;
    const otp = document.getElementById('withdraw-otp').value;

    const response = await fetch('http://localhost:5098/api/atm/withdraw', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ customerId, amount, otp })
    });

    const data = await response.json();
    alert(data.message);
}

async function deposit() {
    const amount = document.getElementById('deposit-amount').value;
    const otp = document.getElementById('deposit-otp').value;

    const response = await fetch('http://localhost:5098/api/atm/deposit', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ customerId, amount, otp })
    });

    const data = await response.json();
    alert(data.message);
}

async function transfer() {
    const amount = document.getElementById('transfer-amount').value;
    const receiveId = document.getElementById('receive-id').value;
    const otp = document.getElementById('transfer-otp').value;

    const response = await fetch('http://localhost:5098/api/atm/transfer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ sendId: customerId, receiveId, amount, otp })
    });

    const data = await response.json();
    alert(data.message);
}
