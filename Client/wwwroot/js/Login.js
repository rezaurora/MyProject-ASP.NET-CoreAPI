﻿$(document).ready(function () {
    $('#login-form').submit(function (e) {
        e.preventDefault();
        var Account = new Object();
        Account.Email = $('#Email').val();
        Account.Password = $('#Password').val();
        $.ajax({
            type: 'POST',
            url: 'http://localhost:8082/api/Account/Login',
            data: JSON.stringify(Account), //convert json
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var panggiltoken = result.data;
                sessionStorage.setItem("key", panggiltoken);
                debugger;
                $.post("/Home/Login", { email: Account.Email })
                    .done(function () {
                        Swal.fire({
                            icon: 'success',
                            title: result.message,
                            showConfirmButton: false,
                            timer: 1500
                        }).then((successAllert) => {
                            if (successAllert) {
                                location.replace("/Departments/Index");
                            } else {
                                location.replace("/Departments/Index");
                            }
                        });
                    })
                    .fail(function () {
                        alert("Fail!, Gagal Login");
                    })                    
            },
            error: function (errorMessage) {
                Swal.fire('Gagal Login', errorMessage.message, 'error');
            }
        });
    })
});

function logout() {
    sessionStorage.removeItem('key');
    location.replace("/Home/Login");
}
