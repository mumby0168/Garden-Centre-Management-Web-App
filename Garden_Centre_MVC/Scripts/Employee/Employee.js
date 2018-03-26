$(document).ready(function() {

    //loads the edit form
    $(".EditLink").click( function(e) {

        var empId = $(window.event.target).attr("empId");
        
        $.ajax({
            url: "/Employee/Edit/" + empId,
            success: function(view) {
                $("#EmployeeFormDiv").html(view);
            }
        });

    });


    //edit customer
    $("#EmployeeForm").submit(function(e) {
        e.preventDefault();


        var form = $("#EmployeeForm").serialize();

        $.ajax({
            url: "/Employee/Save",
            data: form,
            datatype: "JSON",
            type: "POST",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $(".RemoveLink").click(function (e) {

        var empId = $(window.event.target).attr("empId");

        $.ajax({
            url: "/Employee/Remove/" + empId,
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#AddEmployeeBtn").click(function() {

        $.ajax({
            url: "/Employee/Add",
            success: function(view) {
                $("#EmployeeFormDiv").html(view);
            }
        });

    });


});