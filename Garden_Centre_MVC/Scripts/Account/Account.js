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

    //sends the email when the submit button is clicked
    //$("#forgottenPasswordForm").submit(function (e) {

    //    e.preventDefault();

    //    if ($("#EmployeeId").val().toString().length !== 6) {
    //        bootbox.alert("The employee Id must be 6 digits");
    //        return;
    //    }
            


    //    var form = $("#forgottenPasswordForm").serialize();

    //    $.ajax({
    //        url: "/Account/SendRecoveryEmail",
    //        data: form,
    //        datatype: JSON,
    //        success: function (data) {
    //            $("#MainPageContainer").html(data);
    //        }
    //    });
    //});
});

