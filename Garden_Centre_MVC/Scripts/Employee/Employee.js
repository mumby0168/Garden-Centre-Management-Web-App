$(document).ready(function () {


    //for the search box

    $("#ResetSearch").click(function () {
        alert("Clicked");
        $.ajax({
            url: "/Employee/Index/",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#SearchBtn").click(function () {

        var inputBox = $("#SearchBox");
        var filter = inputBox.val();

        $.ajax({
            url: "Employee/Search/",
            data: {"str": filter},
            success: function(view) {
                $("#MainPageContainer").html(view);
            }

        });

        


        //var table = $("#Table");
        //var i;
        //var tr = $('#employeeTable > tbody > tr');

        //for (i = 0; i < tr.length; i++) {

        //    td = tr[i].getElementsByTagName("td")[0];
        //    if (td) {
        //        var t
        //        if (td.innerText.toUpperCase().indexOf(filter) > -1) {
        //            tr[i].style.display = "";
        //        }
        //        else {
        //            tr[i].style.display = "none";
        //        }

        //    }
        //}

    });


    //loads the edit form
    $(".EditLink").click( function(e) {

        //var empId = $(e.target).attr("empId");
        
        $.ajax({
            url: "/Employee/Edit/" + $(this).attr("empId"),
            success: function(view) {
                $("#EmployeeFormDiv").delay(1000).slideDown("slow").html(view);
            }
        });

    });


    //edit customer
    $("#EmployeeForm").submit(function(e) {
        e.preventDefault();

        if ($("#EmployeeNumber").val().toString().length !== 6) {
            //bootbox.alert("The employee Id must be 6 digits");
            $("#EmpNumVal").html("The employee Id must be 6 digits");
            return;
        }

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

        $.ajax({
            url: "/Employee/Remove/" + $(this).attr("empId"),
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#AddEmployeeBtn").click(function() {

        $.ajax({
            url: "/Employee/Add",
            success: function(view) {
                $("#EmployeeFormDiv").delay(1000).slideDown("slow").html(view);
            }
        });

    });


    $("#CloseEmployeeForm").click(function() {
        $("#EmployeeFormDiv").delay(1000).slideUp("slow");
    });


});