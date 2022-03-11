function pollSignatures() {

    $.ajax(
        {
            type: "GET",
            url: "/Cause/GetSignatures",
            data: { causeId: $("#causeId").val() },
            success: function (msg) {

                var obj = JSON.parse(JSON.stringify(msg));
                if (obj.status) {
                    $("#lblNoSignatures").hide();
                    var arr = $.parseJSON(obj.msg)
                    $("#signatureCount").text("Signatures: " + arr.length);
                    populateSignatures($("#signatureList"), arr);
                }
                setTimeout(pollSignatures, 1000);
            }
        });
}

function populateSignatures(select, data) {
    select.html('');
    var items = [];
    $.each(data, function (id, option) {
        items.push('<li class="list-group-item text-center"><h5>' + option.msg + '</h5></li>');
    });
    select.append(items.join(''));
}

function addSignature() {
    $.ajax(
        {
            type: "POST",
            url: "/Cause/AddSignature",
            data: { causeId: $("#causeId").val(), userId: $("#userId").val(), userName: $("#userName").val() },
            async: true,
            success: function (msg) {
                $("#btnForm").hide();
                var obj = JSON.parse(JSON.stringify(msg));
                if (obj.status) {
                    $("#bannerMessage").text(obj.msg);
                    $("#bannerMessage").addClass("bg-success");
                }
                else {
                    $("#bannerMessage").text(obj.msg);
                    $("#bannerMessage").addClass("bg-danger");
                }
                $("#bannerMessage").show();
                $("#banner").show();
                setTimeout(hideSuccessBanner, 5000);
            }
        });
}

function hideSuccessBanner() {
    $("#banner").hide();
    $("#bannerMessage").hide
    if ($("#bannerMessage").hasClass("bg-success")) {
        $("#bannerMessage").removeClass("bg-success");
    }
    else if ($("#bannerMessage").hasClass("bg-danger")) {
        $("#bannerMessage").removeClass("bg-danger");
    }
}

function checkUser(username) {
    $.ajax(
        {
            type: "GET",
            url: "/Cause/GetSignatures",
            data: { causeId: $("#causeId").val() },
            success: function (msg) {

                var obj = JSON.parse(JSON.stringify(msg));
                if (obj.status) {
                    var arr = $.parseJSON(obj.msg)

                    var state = false;

                    $.each(arr, function (id, option) {
                        if (option.msg == username) {
                            state = true;
                            $("#btnSignUp").hide();
                            return;
                        }
                    });

                    if (!state) {
                        $("#btnSignUp").show();
                    }
                }
                else {
                    if (username != $("#publisher").val()) {
                        $("#btnSignUp").show();
                    }
                    $("#lblNoSignatures").show().text(obj.msg);
                }
            }
        });
}

$(document).ready(function () {
    //$("#banner").hide
    //$("#bannerMessage").hide

    if ($("#userName").val() != "") {
        checkUser($("#userName").val());
    }

    pollSignatures();

    $("#btnForm").submit(function (e) {
        e.preventDefault();
        addSignature();

    });

    // Bind click to Confirm button within popup
    $('#confirm-delete').on('click', '.btn-confirm', function (e) {

        $("#deleteCauseForm").submit();
    });

    $("#ShareFacebook").on("click", function () {
        FB.ui({
            display: 'popup',
            method: 'share',
            href: 'https://developers.facebook.com/docs/',
        }, function (response) { });
    }
    });
});