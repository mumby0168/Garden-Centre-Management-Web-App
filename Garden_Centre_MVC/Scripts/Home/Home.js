$(document).ready(function () {

    //this handles moving to the employee page
    $("#EmployeeLandingLink").click(function () {

        //this will call the employee index page and change the view to the view that it shall return.
        $.ajax({
            url: "Employee/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });
    });

    //this handles the click of the transactions link.
    $("#TransactionsLandingLink").click(function() {

        //this will load the index page for the transaction view and set the view to the view it shall return.
        $.ajax({
            url: "Transactions/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });


    });

    //this handles the click of the log link
    $("#ActionLogLandingLink").click(function() {

        // this will load the log page view.
        $.ajax({
            url: "/Log/Index",
            success: function (view) {

                $("#MainPageContainer").html(view);

                var data = null;
                var searchTerms = new Map();

                //this will get all of the data out of the database for all the logs and create the tables.
                $.ajax({
                    url: "Log/GetAll",
                    success: function (datas) {
                        data = JSON.parse(datas); 
                        ActionLogTable(data);
                    }
                });
            },
            //this will be called in case of a error for loading the log page.
            error: function (error, type, errorMessage) {
                alert(errorMessage);
            }

        });

    });

    //this will handle the click of the inventory link.
    $("#InventoryLandingLink").click(function() {

        //this will load the index view for the inventory and set the new view of the web page.
        $.ajax({
            url: "Inventory/Index",
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });

    });

    //this will hanlde the click of the customer page
    $("#CustomerLandingLink").click(function () {

        //this will call the index method of the customer controller and set the new view of the web page.
        $.ajax({
            url: "Customer/Index",
            success: function (view) {
                $("#MainPageContainer").html(view);
            }
        });

    });


   


    // this is called in order to use the framework we have written. it will take the JSON obj create headers for the table
    //and map the properites to each heading. It will also provide a lamda on how to create each column which is used the
    //by the framework. The lamda will be iterated over in the framework.
    function ActionLogTable(JsonObj) {

        var searchTerms = new Map();

        var tableId = "LogTable";


        searchTerms.set("Log Number", 0);
        searchTerms.set("Date of Action", 1);
        searchTerms.set("Username", 2);
        searchTerms.set("Action Type", 3);
        searchTerms.set("Details of Action", 4);


        var header = ["Log Number", "Date of Action", "Username", "Action Type", "Details of Action"];

        var styles = "btn btn-default";

        var paging = new Paging(JsonObj,
            tableId,
            header,
            searchTerms,
            (data, table) => {

                var tr, td;

                tr = document.createElement("tr");
                td = document.createElement("td");      
                td.innerHTML = data.logNumber;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = data.DateofAction;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = data.Username;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = data.ActionType;
                tr.appendChild(td);

                td = document.createElement("td");
                td.innerHTML = data.DetailsOfAction;
                tr.appendChild(td);

                table.appendChild(tr);
            }, styles);

    }



});