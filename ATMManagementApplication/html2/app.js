// Gọi API và xử lý sự kiện

// Đăng nhập
document.getElementById("loginForm")?.addEventListener("submit", async (e) => {
  e.preventDefault();
  const email = document.getElementById("email").value;
  const password = document.getElementById("password").value;

  const response = await fetch("http://localhost:5098/api/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  });

  const data = await response.json();
  if (response.ok) {
    localStorage.setItem("token", data.token);
    localStorage.setItem("customerId", data.customerId);
    localStorage.setItem("email", data.email);
    window.location.href = "transaction.html";
  } else {
    alert(data.message);
  }
});

// Đăng ký
document
  .getElementById("registerForm")
  ?.addEventListener("submit", async (e) => {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const response = await fetch("http://localhost:5098/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name, email, password }),
    });

    const data = await response.json();
    if (response.ok) {
      alert("Đăng ký thành công!");
      window.location.href = "login.html";
    } else {
      alert(data.message);
    }
  });

// Thay đổi mật khẩu
document
  .getElementById("changePasswordForm")
  ?.addEventListener("submit", async (e) => {
    e.preventDefault();
    const email = document.getElementById("email").value;
    const oldPassword = document.getElementById("oldPassword").value;
    const newPassword = document.getElementById("newPassword").value;

    const response = await fetch(
      "http://localhost:5098/api/auth/change-password",
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({ email, oldPassword, newPassword }),
      }
    );

    const data = await response.json();
    alert(data.message);
  });

// Rút tiền
document
  .getElementById("withdrawForm")
  ?.addEventListener("submit", async (e) => {
    e.preventDefault();
    const customerId = localStorage.getItem("customerId");
    const amount = document.getElementById("withdrawAmount").value;
    const otp = document.getElementById("otpWithdraw").value;

    const response = await fetch("http://localhost:5098/api/atm/withdraw", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify({ customerId, amount, otp }),
    });

    const data = await response.json();
    alert(data.message);
  });

document
  .getElementById("transactionHistory")
  ?.addEventListener("click", async () => {
    const token = localStorage.getItem("token");
    const customerId = localStorage.getItem("customerId");
    const response = await fetch(
      `http://localhost:5098/api/atm/transaction/${customerId}`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    // const data = await response.json();
    // if (response.ok) {
    //     console.log(data);
    // //   alert(JSON.stringify(data));
    // } else {
    // //   alert(data.message);
    // console.log(data);
    // }
    const data = await response.json();
    const historyContainer = document.getElementById("transactionHistoryData");
    historyContainer.innerHTML = ""; // Xóa nội dung cũ trước khi in

    if (response.ok) {
      // Nếu phản hồi OK, duyệt qua mảng dữ liệu và in ra
      data.forEach((transaction) => {
        const { transactionType, amount, timestamp, isSuccessful } =
          transaction;
        const successStatus = isSuccessful ? "Successful" : "Failed";
        const formattedTransaction = `Type: ${transactionType}, Amount: ${amount}, Time: ${timestamp}, ${successStatus}`;

        const transactionElement = document.createElement("p");
        transactionElement.textContent = formattedTransaction;
        historyContainer.appendChild(transactionElement);
      });
    } else {
      // Xử lý nếu có lỗi từ API
      console.error(
        "Error:",
        data.message || "Unable to fetch transaction data"
      );
      alert("Failed to load transaction history.");
    }
  });

// Gửi OTP cho giao dịch
document.getElementById("requestOTP")?.addEventListener("click", async () => {
  const customerId = localStorage.getItem("customerId"); // Giả định bạn lưu email trong localStorage sau khi đăng nhập

  if (!customerId) {
    alert("Vui lòng đăng nhập để gửi OTP.");
    return;
  }

  const response = await fetch("http://localhost:5098/api/atm/request-otp", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify({ customerId }),
  });

  const data = await response.json();
  alert(data.message);
});

// Đăng xuất
document.getElementById("logout")?.addEventListener("click", () => {
  localStorage.removeItem("token");
  localStorage.removeItem("customerId");
  localStorage.removeItem("email");
  window.location.href = "login.html";
});

document.getElementById("checkBalance")?.addEventListener("click", async () => {
  console.log("Check balance");
  const customerId = localStorage.getItem("customerId");
  const token = localStorage.getItem("token");
  const response = await fetch(
    `http://localhost:5098/api/atm/balance/${customerId}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  const data = await response.json();
  if (response.ok) {
    alert(`Số dư: ${data.balance}`);
  } else {
    alert(data.message);
  }
});
