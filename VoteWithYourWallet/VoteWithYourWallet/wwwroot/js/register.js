
var forceRegister = false;

var usernameValidated = false;
var emailValidated = false;
var passwordValidated = false;
var confirmPasswordValidated = false;

function checkRegister() {

    if (usernameValidated && emailValidated && passwordValidated && confirmPasswordValidated) {
        forceRegister = true;
        $("#register").submit();
    }
}

$(document).ready(function () {
    // on username input changed
    $("#username").keyup(function () {
        if ($("#username").val().length != 0) {
            $.ajax({
                type: "GET",
                url: "/User/CheckUsername",
                data: { username: $("#username").val() },
                async: true,
                success: function (msg) {

                    var obj = JSON.parse(JSON.stringify(msg));
                    if (!obj.status) {
                        usernameValidated = true;
                        $("#errorUsername").text("");
                    }
                    else {
                        usernameValidated = false;
                        $("#errorUsername").text("Username already taken");
                    }
                }
            });
        }
        else {
            usernameValidated = false;
            $("#errorUsername").text("Username is required");
        }
    });

    // on email input changed
    $("#email").keyup(function () {
        if ($("#email").val().length != 0) {
            $("#errorEmail").text("");
            var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            // change email is a vaild email
            if (EmailRegex.test($("#email").val())) {
                // check to see if there is an account with the entered email
                $.ajax({
                    type: "GET",
                    url: "/User/CheckEmail",
                    data: { email: $("#email").val() },
                    async: true,
                    success: function (msg) {

                        var obj = JSON.parse(JSON.stringify(msg));
                        if (obj.status) {
                            emailValidated = false;
                            $("#errorEmail").text("Email address is already in use, ");
                            $("#errorEmail").append("<a class='text-danger' href='/User/Login'>try logging in?<a/>")
                        }
                        else {
                            emailValidated = true;
                            $("#errorEmail").text("");
                        }
                    }
                });
            }
            else {
                emailValidated = false;
            }
        }
        else {
            emailValidated = false;
            $("#errorEmail").text("Email is required");
        }
    });

    $("#email").change(function () {
        if ($("#email").val().length != 0) {
            var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            // change email is a vaild email
            if (!EmailRegex.test($("#email").val())) {
                emailValidated = false;
                $("#errorEmail").text("Please enter a valid email");
            }
        }
        else {
            emailValidated = false;
        }
    });

    $("#password").keyup(function () {
        if ($("#password").val().length != 0) {
            passwordValidated = true;
            $("#errorPassword").text("");
        }
        else {
            passwordValidated = false;
            $("#errorPassword").text("Password is required");
        }
    });

    $("#confirmPassword").keyup(function () {
        if ($("#confirmPassword").val().length != 0) {
            confirmPasswordValidated = true;
            $("#errorConfirmPassword").text("");
        }
        else {
            confirmPasswordValidated = false;
            $("#errorConfirmPassword").text("Confirm Password is required");

        }
    });

    $("#confirmPassword").change(function () {
        if ($("#confirmPassword").val() == $("#password").val()) {
            confirmPasswordValidated = true;
            $("#errorConfirmPassword").text("");
        }
        else {
            confirmPasswordValidated = false;
            $("#errorConfirmPassword").text("Confirm Password does not match Password");
        }
    });

    $("#register").submit(function (e) {
        $("#errors").text("");
        if (!forceRegister) {
            e.preventDefault();
            checkRegister();
        }
    });
});