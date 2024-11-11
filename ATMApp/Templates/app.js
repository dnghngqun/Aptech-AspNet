const apiUrl = "http://localhost:5271/api/transactions";

// Handle Deposit
document.getElementById('depositForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const accountId = document.getElementById('depositAccountId').value;
    const amount = document.getElementById('depositAmount').value;

    const response = await fetch(`${apiUrl}/${accountId}/deposit`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(amount)
    });

    const data = await response.json();
    displayMessage(response.ok ? `Deposit successful! New balance: ${data.balance}` : data);
});

// Handle Withdraw
document.getElementById('withdrawForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const accountId = document.getElementById('withdrawAccountId').value;
    const amount = document.getElementById('withdrawAmount').value;

    const response = await fetch(`${apiUrl}/withdraw?accountId=${accountId}&amount=${amount}`, {
        method: 'POST',
    });

    const data = await response.json();
    displayMessage(response.ok ? `Withdraw successful! New balance: ${data.balance}` : data);
});

// Handle Transfer
document.getElementById('transferForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const fromAccountId = document.getElementById('fromAccountId').value;
    const toAccountId = document.getElementById('toAccountId').value;
    const amount = document.getElementById('transferAmount').value;

    const response = await fetch(`${apiUrl}/transfer?fromAccountId=${fromAccountId}&toAccountId=${toAccountId}&amount=${amount}`, {
        method: 'POST',
    });

    const data = await response.json();
    displayMessage(response.ok ? `Transfer successful! From: ${data.FromAccountBalance}, To: ${data.ToAccountBalance}` : data);
});

// Function to display response messages
function displayMessage(message) {
    const messageDiv = document.getElementById('responseMessage');
    messageDiv.textContent = typeof message === 'string' ? message : JSON.stringify(message);
    messageDiv.classList.remove('d-none');
}
