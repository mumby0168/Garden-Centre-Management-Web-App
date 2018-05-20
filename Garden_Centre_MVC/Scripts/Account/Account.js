$(document).ready(function () {

    $("#ForogotPasswordDiv").hidden = true;


    
    //loads the modal with a form from a partial view
    $("#ForgottonPasswordLink").click(function(e) {

        e.preventDefault();

        $.ajax({
            url: "/Account/ForgotPassword",
            success: function(content) {
                $("#ForgotPasswordModalBody").html(content);
            }

        });

        $("#forgotPasswordModal").delay(1000).modal();
    });   
});

