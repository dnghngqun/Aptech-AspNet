$(document).ready(function () {
    // --- Comic Book CRUD ---
    // 1. Hiển thị danh sách các comic books
    function loadComicBooks() {
        $.ajax({
            url: "http://localhost:5247/api/comicbooks",
            type: "GET",
            success: function (data) {
                let comicBooksTable = $("#comicBooksTable tbody");
                comicBooksTable.empty(); // Xóa nội dung cũ

                data.forEach(function (comicBook, index) {
                    let row = `
                        <tr>
                            <td>${comicBook.id}</td>
                            <td>${comicBook.title}</td>
                            <td>${comicBook.author}</td>
                            <td>${comicBook.pricePerDay}</td>
                            <td>
                                <button class="btn btn-warning btn-sm editComicBook" data-id="${comicBook.id}">Edit</button>
                                <button class="btn btn-danger btn-sm deleteComicBook" data-id="${comicBook.id}">Delete</button>
                            </td>
                        </tr>
                    `;
                    comicBooksTable.append(row);
                });
            },
            error: function () {
                alert("Failed to load comic books.");
            }
        });
    }

    loadComicBooks(); // Load comic books khi trang được tải

    // 2. Thêm mới Comic Book
    $("#submitComicBookForm").click(function () {
        let comicBook = {
            title: $("#comicBookTitle").val(),
            author: $("#comicBookAuthor").val(),
            pricePerDay: $("#comicBookPrice").val()
        };

        $.ajax({
            url: "http://localhost:5247/api/comicbooks",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(comicBook),
            success: function () {
                alert("Comic book created successfully!");
                loadComicBooks(); // Load lại danh sách comic books
                $("#comicBookModal").modal("hide");
                $("#comicBookForm")[0].reset(); // Reset form
            },
            error: function () {
                alert("Failed to create comic book.");
            }
        });
    });

    // 3. Xử lý chỉnh sửa Comic Book
    $(document).on("click", ".editComicBook", function () {
        let comicBookId = $(this).data("id");

        $.ajax({
            url: `http://localhost:5247/api/comicbooks/${comicBookId}`,
            type: "GET",
            success: function (comicBook) {
                $("#comicBookTitle").val(comicBook.title);
                $("#comicBookAuthor").val(comicBook.author);
                $("#comicBookPrice").val(comicBook.pricePerDay);
                $("#submitComicBookForm").text("Update");
                $("#submitComicBookForm").off().click(function () {
                    let updatedComicBook = {
                        id: comicBookId,
                        title: $("#comicBookTitle").val(),
                        author: $("#comicBookAuthor").val(),
                        pricePerDay: $("#comicBookPrice").val()
                    };

                    $.ajax({
                        url: `http://localhost:5247/api/comicbooks/${comicBookId}`,
                        type: "PUT",
                        contentType: "application/json",
                        data: JSON.stringify(updatedComicBook),
                        success: function () {
                            alert("Comic book updated successfully!");
                            loadComicBooks(); // Load lại danh sách comic books
                            $("#comicBookModal").modal("hide");
                            $("#comicBookForm")[0].reset();
                        },
                        error: function () {
                            alert("Failed to update comic book.");
                        }
                    });
                });
                $("#comicBookModal").modal("show");
            },
            error: function () {
                alert("Failed to load comic book details.");
            }
        });
    });

    // 4. Xử lý xóa Comic Book
    $(document).on("click", ".deleteComicBook", function () {
        let comicBookId = $(this).data("id");

        if (confirm("Are you sure you want to delete this comic book?")) {
            $.ajax({
                url: `http://localhost:5247/api/comicbooks/${comicBookId}`,
                type: "DELETE",
                success: function () {
                    alert("Comic book deleted successfully!");
                    loadComicBooks(); // Load lại danh sách comic books
                },
                error: function () {
                    alert("Failed to delete comic book.");
                }
            });
        }
    });

    // --- Customer Registration ---
    // 5. Đăng ký khách hàng mới
    $("#customerRegistrationForm").submit(function (event) {
        event.preventDefault();

        let customer = {
            fullname: $("#customerFullname").val(),
            phoneNumber: $("#customerPhoneNumber").val(),
            registrationDate: new Date().toISOString()
        };

        $.ajax({
            url: "http://localhost:5247/api/customers",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(customer),
            success: function () {
                alert("Customer registered successfully!");
                $("#customerRegistrationForm")[0].reset(); // Reset form
            },
            error: function () {
                alert("Failed to register customer.");
            }
        });
    });

    // --- Renting Books ---
    // 6. Hiển thị danh sách khách hàng và sách
    function loadCustomersAndComicBooks() {
        // Load customers
        $.ajax({
            url: "http://localhost:5247/api/customers",
            type: "GET",
            success: function (customers) {
                let customerSelect = $("#customerSelect");
                customerSelect.empty();
                customerSelect.append('<option value="">Select Customer</option>');

                customers.forEach(function (customer) {
                    customerSelect.append(`<option value="${customer.id}">${customer.fullname}</option>`);
                });
            }
        });

        // Load comic books
        $.ajax({
            url: "http://localhost:5247/api/comicbooks",
            type: "GET",
            success: function (comicBooks) {
                let comicBookSelect = $("#comicBookSelect");
                comicBookSelect.empty();
                comicBookSelect.append('<option value="">Select Comic Book</option>');

                comicBooks.forEach(function (comicBook) {
                    comicBookSelect.append(`<option value="${comicBook.id}">${comicBook.title}</option>`);
                });
            }
        });
    }

    loadCustomersAndComicBooks(); // Load ngay khi trang được tải

    // 7. Xử lý thuê sách
    $("#rentalForm").submit(function (event) {
        event.preventDefault();

        let rental = {
            customerId: $("#customerSelect").val(),
            rentalDate: $("#rentalDate").val(),
            returnDate: $("#returnDate").val()
        };
        console.log(rental); 


        $.ajax({
            url: "http://localhost:5247/api/rentals",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(rental),
            success: function (rentalData) {
                let rentalDetails = {
                    rentalId: rentalData.id,
                    comicBookId: $("#comicBookSelect").val(),
                    quantity: $("#rentalQuantity").val(),
                    pricePerDay: $("#comicBookPrice").val() // Tính toán theo giá sách
                };

                $.ajax({
                    url: "http://localhost:5247/api/rentaldetails",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(rentalDetails),
                    success: function () {
                        alert("Rental created successfully!");
                        $("#rentalForm")[0].reset(); // Reset form
                    },
                    error: function () {
                        alert("Failed to create rental details.");
                    }
                });
            },
            error: function () {
                alert("Failed to create rental.");
            }
        });
    });

    // --- Rental Reports ---
    // 8. Tạo báo cáo cho các lần thuê giữa 2 ngày
    $("#reportForm").submit(function (event) {
        event.preventDefault();

        let startDate = $("#reportStartDate").val();
        let endDate = $("#reportEndDate").val();

        $.ajax({
            url: `http://localhost:5247/api/rentals/reports?startDate=${startDate}&endDate=${endDate}`,
            type: "GET",
            success: function (report) {
                let reportTable = $("#reportTable tbody");
                reportTable.empty();

                report.forEach(function (item, index) {
                    let row = `
                        <tr>
                            <td>${index + 1}</td>
                            <td>${item.title}</td>
                            <td>${item.rentalDate}</td>
                            <td>${item.returnDate}</td>
                            <td>${item.customerName}</td>
                            <td>${item.quantity}</td>
                        </tr>
                    `;
                    reportTable.append(row);
                });
            },
            error: function () {
                alert("Failed to generate report.");
            }
        });
    });
});
