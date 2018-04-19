$(document).ready(function () {

    //global variables
    var amountOfrecords;

    //Paging Start
   
    //$.ajax({
    //    url: "/Customers/CheckAmountOfRecords",
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
            url: "/Customers/LoadTablePage",
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
            url: "/Customers/LoadTablePage",
            data: { "page": pages },
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });
    });

    $("#GoBackFromNullError").click(function() {
        $.ajax({
            url: "/Customers/Index",
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
            url: "/Customers/Index/",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#SearchBtn").click(function () {

        var inputBox = $("#SearchBox");
        var filter = inputBox.val();

        $.ajax({
            url: "Customers/Search/",
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
            url: "/Customers/Edit/" + $(this).attr("empId"),
            success: function(view) {
                $("#CustomersFormModalBody").html(view);
                $("#CustomersFormModal").modal();
            }
        });

    });


    //Edit Customer
    $("#CustomersForm").submit(function(e) {
        e.preventDefault();

        if ($("#CustomersNumber").val().toString().length !== 6) {
            //bootbox.alert("The customers Id must be 6 digits");
            $("#EmpNumVal").html("The customers Id must be 6 digits");
            return;
        }

        var form = $("#CustomersForm").serialize();

        $.ajax({
            url: "/Customers/Save",
            data: form,
            datatype: "JSON",
            type: "POST",
            success: function (view) {
                $("#CustomersFormModal").hide();
                $(".modal-backdrop").remove();
                $("#MainPageContainer").html(view);
            }
        });

    });

    //deletes the Customer
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
                    url: "/Customers/Remove/" + id,
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
    $("#AddCustomersBtn").click(function() {

        $.ajax({
            url: "/Customers/Add",
            success: function(view) {
                $("#CustomersFormModalBody").html(view);
                $("#CustomersFormModal").modal();
            }
        });

    });

    //closes the customers form
    $("#CloseCustomersForm").click(function() {
        $("#CustomersFormDiv").delay(1000).slideUp("slow");
    });

});