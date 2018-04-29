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

                var data = null;
                var searchTerms = new Map();

                $.ajax({
                    url: "Log/GetAll",
                    success: function (datas) {
                        data = JSON.parse(datas); 
                        ActionLogTable(data);
                    }
                });



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

    $("#EmployeeNameLink").click(function() {

        var id = document.getElementById("UsersId").value;

        $.ajax({
            url: "Account/GetAccountDetails/" + id,
            success: function(view) {
                $("#MainPageContainer").html(view);
            }
        });

    });


   



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