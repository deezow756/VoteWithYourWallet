
$(document).ready(function () {
    $("#createCauseFrom").validate({
        rules: {
            name: {
                required: true
            },
            subject: {
                required: true
            },
            description: {
                required: true
            }
        }
    });
});