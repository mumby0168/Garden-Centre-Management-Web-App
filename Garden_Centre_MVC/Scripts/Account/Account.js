$(document).ready(function () {

    $("#ForogotPasswordDiv").hidden = true;

    //loads the modal with a form from a partial view
    $("#ForgottonPasswordLink").click(function() {

        $.ajax({
            url: "/Account/ForgotPassword",
            success: function(content) {
                $("#ForogotPasswordDiv").html(content);
            }

        });

        $("#ForogotPasswordDiv").delay(1000).slideDown("slow");

    });


   

    //sends the email when the submit button is clicked
    $("#forgottenPasswordForm").submit(function (e) {

        //stop the page from refreshing

        $.ajax({
            url: "/Account/SendRecoveryEmail",
            data: $("#forgottenPasswordForm").serialize(),
            datatype: JSON,
            success: function (data) {
                $("#ForogotPasswordDiv").delay(1000).slideUp("slow");
                bootbox.alert("The email with your username and password has been sent");
            }
        });

        //stop the page from refreshing
        e.preventDefault();
    });

    //closes the form 
    $("#CloseForgotPasswordFormBtn").click(function() {

        $("#ForogotPasswordDiv").slideUp("Slow");

    });

});

