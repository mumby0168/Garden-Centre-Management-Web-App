$(document).ready(function() {
    //this handles moving to the comment page
    $("#EmployeeLandingLink").click(function () {

        $("#Loader").show("fast");

        $.ajax({
            url: "Employee/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
                $("#Loader").hide();
            }
        });
    });


    $("#TransactionsLandingLink").click(function() {

        $.ajax({
            url: "Transactions/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });


    });

    $("#ActionLogLandingLink").click(function() {

        $.ajax({
            url: "Log/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#InventoryLandingLink").click(function() {

        $.ajax({
            url: "Inventory/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#CustomerLandingLink").click(function () {

        $.ajax({
            url: "Customer/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });


});