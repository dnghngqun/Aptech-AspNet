document.getElementById('change-password-form').addEventListener('submit', function(e) {
    e.preventDefault();
    const email = document.getElementById('change-email').value;
    const oldPassword = document.getElementById('old-password').value;
    const newPassword = document.getElementById('new-password').value;

    fetch('http://localhost:5098/api/auth/change-password', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('jwt') // Thêm JWT token nếu cần
        },
        body: JSON.stringify({ Email: email, OldPassword: oldPassword, NewPassword: newPassword })
    })
    .then(response => response.json())
    .then(data => alert(data.message))
    .catch(error => console.error('Error:', error));
});
