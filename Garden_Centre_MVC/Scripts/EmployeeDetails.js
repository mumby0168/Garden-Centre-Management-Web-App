$(document).ready(function() {


    $("#PersonalDetailsDown").click(function () {
        $("#PersonalDetailsDiv").slideDown();

    });

    $("#ClosePersonalDetails").click(function() {
        $("#PersonalDetailsDiv").slideUp();
    });

    $("#PersonalDetailsForm").submit(function() {

        $.ajax({
            url: "Account/EditPersonal",
            data: $("#PersonalDetailsForm").serialize(),
            success: function(view) {

            }
    });

    });

});
