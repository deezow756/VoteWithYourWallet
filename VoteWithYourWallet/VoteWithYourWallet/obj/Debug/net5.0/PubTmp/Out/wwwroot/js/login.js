
var forceLogin = false;

function checkLogin() {

    if ($("#email").val().length != 0) {
        var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (EmailRegex.test($("#email").val())) {
            $.ajax(
                {
                    type: "GET",
                    url: "/User/CheckLogin",
                    data: { email: $("#email").val(), password: $("#password").val() },
                    async: true,
                    success: function (msg) {

                        var obj = JSON.parse(JSON.stringify(msg));
                        if (obj.status) {
                            forceLogin = true;
                            $("#login").submit();
                        }
                        else {
                            $("#errorLogin").text(obj.msg);
                        }
                    }
                });
        }
    }
}


$(document).ready(function () {

    $("#email").keyup(function () {
        if ($("#email").val().length == 0) {
            $("#errorEmail").text("Email is required");
        }
        else {

            $("#errorEmail").text("");
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
        if ($("#password").val().length == 0) {
            $("#errorPassword").text("Password is required");
        }
        else {
            $("#errorPassword").text("");
        }
    });

    $("#login").submit(function (e) {
        if (!forceLogin) {
            e.preventDefault();
            checkLogin();
        }
    });
});