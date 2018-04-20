$(document).ready(function () {

    //global variables
    var amountOfrecords;

    //Paging Start
   
    //$.ajax({
    //    url: "/Employee/CheckAmountOfRecords",
    //    type: "JSON",
    //    success: function(obj) {
    //        amountOfrecords = obj.amount;

    //        var num = amountOfrecords / 10;

    //        var pages = ceil(num);

    //        alert(pages);
    //    }
    //});

    $("#NextPage").click(function() {

        var page = $("#PageNumber").html();
        page++;

        $.ajax({
            url: "/Employee/LoadTablePage",
            data: { "page": page },
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });


    $("#PreviousPage").click(function () {

        var pages = $("#PageNumber").html();

        if (parseInt(pages) === 1) {
            bootbox.alert("This is the first page");
            return;
        }

        pages--;

        $.ajax({
            url: "/Employee/LoadTablePage",
            data: { "page": pages },
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });
    });

    $("#GoBackFromNullError").click(function() {
        $.ajax({
            url: "/Employee/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
                $("#Loader").hide();
            }
        });
    });

    //Paging End

      
    //Search Box Start
    $("#ResetSearch").click(function () {
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

    });
    //Search Box End


    //loads the edit form
    $(".EditLink").click( function(e) {
        
        $.ajax({
            url: "/Employee/Edit/" + $(this).attr("empId"),
            success: function(view) {
                $("#EmployeeFormModalBody").html(view);
                $("#EmployeeFormModal").modal();
            }
        });

    });


    //Edit Customer
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
            success: function (view) {
                $("#EmployeeFormModal").hide();
                $(".modal-backdrop").remove();
                $("#MainPageContainer").html(view);
            },
            error: function (error, type, errorMessage) {
                var errorobj = JSON.parse(errorMessage);
                $("#EmpNumVal").html(errorobj.ErrorMessage);

            }
        });

    });

    //deletes the employee
    $(".RemoveLink").click(function (e) {

        var id = $(this).attr("empId");

        bootbox.confirm({
            message: "are you sure you want to delete this record",
            buttons: {
                confirm: {
                    label: "Yes",
                    className: "btn-danger"
                },
                cancel: {
                    label: "No",
                    className: "btn-default"
                }
            },
            callback: function (result) {
                if (result !== true) {
                    return;
                }

                $.ajax({
                    url: "/Employee/Remove/" + id,
                    error: function(error, type, errorMessage) {
                        bootbox.alert({
                            message: errorMessage,
                            size: "small"
                        });
                    },
                    success: function (view) {
                        $("#MainPageContainer").html(view);
                    }
                    
                });
            }
        });
    });

    //loads the add screen
    $("#AddEmployeeBtn").click(function() {

        $.ajax({
            url: "/Employee/Add",
            success: function(view) {
                $("#EmployeeFormModalBody").html(view);
                $("#EmployeeFormModal").modal();
            }
        });

    });

    //closes the employee form
    $("#CloseEmployeeForm").click(function() {
        $("#EmployeeFormDiv").delay(1000).slideUp("slow");
    });

});