$(document).ready(function() {
    //this handles moving to the comment page
    $("#EmployeeLandingLink").click(function () {

        $("#Loader").show("fast");

        $.ajax({
            url: "/Employee/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
                $("#Loader").hide();
            }
        });
    });


});