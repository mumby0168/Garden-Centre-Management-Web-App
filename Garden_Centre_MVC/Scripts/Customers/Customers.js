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
        
    $("#NextPage").click(function() { //Function for getting to the next page.

        var page = $("#PageNumber").html();
        page++; //Add 1 to the page number.

        $.ajax({
            url: "/Customer/LoadTablePage", //Current page.
            data: { "page": page },
            success: function (view) { 
                $("#MainPageContainer").html(view); //Return the new view on the page.


            }
        });

    });


    $("#PreviousPage").click(function () { //Similar as the last function but for accessing the previous page.

        var pages = $("#PageNumber").html();

        if (parseInt(pages) === 1) { //If the page is the first, display the message below.
            bootbox.alert("This is the first page");
            return; //Return the home view.
        }

        pages--;

        $.ajax({
            url: "/Customer/LoadTablePage",
            data: { "page": pages },
            success: function(view) {
                $("#MainPageContainer").html(view); //Return the current view.
            }
        });
    });

    $("#GoBackFromNullError").click(function() { //Back button function.
        $.ajax({
            url: "/Customer/Index", //Defines the page within the container.
            success: function (view) {
                $("#MainPageContainer").html(view);
                $("#Loader").hide(); //Hide the page which the user has gone back from.
            }
        });
    });

    //Paging End

      
    //Search Box Start
    $("#ResetSearch").click(function () { //Reset search function.
        $.ajax({
            url: "/Customer/Index/",
            success: function (view) {
                $("#MainPageContainer").html(view); //Return view.
            }
        });

    });

    $("#SearchBtn").click(function () { //Search function.

        var inputBox = $("#SearchBox");
        var filter = inputBox.val();

        $.ajax({
            url: "Customer/Search/",
            data: {"str": filter}, //Use the input to return names which match.
            success: function(view) {
                $("#MainPageContainer").html(view); //Return the view.
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



                //Return the error message if it does not meet the validadtion.
                var errorobj = JSON.parse(errorMessage);

                $("#ErrorDiv").html(""); //Return the appropriate error message.

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

                div.setAttribute("style", "color:red"); //Sets the styling for the error messages.

                div.appendChild(list);


            }
        });

    });

    //deletes the Customer
    $(".RemoveLink").click(function (e) {

        var id = $(this).attr("custId");

        bootbox.confirm({ //Pop-up confirmation box.
            message: "Are you sure you want to delete this record?", 
            buttons: {
                confirm: {
                    label: "Yes",
                    className: "btn-danger" //Styling on the box.
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