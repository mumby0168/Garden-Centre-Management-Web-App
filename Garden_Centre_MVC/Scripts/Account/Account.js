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

        e.preventDefault();

        if ($("#EmployeeId").val().toString().length !== 6) {
            bootbox.alert("The employee Id must be 6 digits");
            return;
        }
            


        var form = $("#forgottenPasswordForm").serialize();

        $.ajax({
            url: "/Account/SendRecoveryEmail",
            data: form,
            datatype: JSON,
            success: function (data) {
                $("#ForogotPasswordDiv").delay(1000).slideUp("slow");
                bootbox.alert("The email with your username and password has been sent");
            }
        });

        //stop the page from refreshing
        
    });

    //closes the form 
    $("#CloseForgotPasswordFormBtn").click(function() {

        $("#ForogotPasswordDiv").slideUp("Slow");

    });

});

