﻿$(document).ready(function () {




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
            url: "/Customer/LoadTablePage",
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
            url: "/Customer/LoadTablePage",
            data: { "page": pages },
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });
    });

    $("#GoBackFromNullError").click(function() {
        $.ajax({
            url: "/Customer/Index",
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
            url: "/Customer/Index/",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    $("#SearchBtn").click(function () {

        var inputBox = $("#SearchBox");
        var filter = inputBox.val();

        $.ajax({
            url: "Customer/Search/",
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
            url: "/Customer/Edit/" + $(this).attr("custId"),
            success: function(view) {
                $("#CustomerFormModalBody").html(view);
                $("#CustomerFormModal").modal();
            }
        });

    });


    //Edit Customer
    $("#CustomerForm").submit(function (e) {


        e.preventDefault();

        alert("form submitted");       

        var form = $("#CustomerForm").serialize();

        $.ajax({
            url: "/Customer/Save",
            data: form,
            datatype: "JSON",
            type: "POST",
            success: function (view) {
                $("#CustomerFormModal").hide();
                $(".modal-backdrop").remove();
                $("#MainPageContainer").html(view);
            },
            error: function (error, type, errorMessage) {




                var errorobj = JSON.parse(errorMessage);

                $("#ErrorDiv").html("");

                var div = document.getElementById("ErrorDiv");

                var h4 = document.createElement("h5");

                h4.innerHTML = "Form Issues:";

                div.appendChild(h4);

                var list = document.createElement("ul");

                for (var i = 0; i < errorobj.ErrorMessages.length; i++) {
                    var li = document.createElement("li");

                    li.innerHTML = errorobj.ErrorMessages[i];

                    list.appendChild(li);
                }

                div.setAttribute("style", "color:red");

                div.appendChild(list);


            }
        });

    });

    //deletes the Customer
    $(".RemoveLink").click(function (e) {

        var id = $(this).attr("custId");

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

                alert("Remove Submitted");

                $.ajax({
                    url: "/Customer/Remove/" + id,
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
    $("#AddCustomerBtn").click(function() {

        $.ajax({
            url: "/Customer/Add",
            success: function(view) {
                $("#CustomerFormModalBody").html(view);
                $("#CustomerFormModal").modal();


                //closes the customers form
                $("#CloseCustomerForm").click(function (e) {
                    e.preventDefault();
                    $("#CustomerFormModal").delay(1000).slideUp("slow", () => $("#CustomerFormModal").modal('toggle'));
                        ;
                });
            }
        });

    });

});