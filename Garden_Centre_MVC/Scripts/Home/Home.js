$(document).ready(function() {



    //this handles moving to the comment page
    $("#EmployeeLandingLink").click(function () {

        $.ajax({
            url: "/Employee/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });
    });


});