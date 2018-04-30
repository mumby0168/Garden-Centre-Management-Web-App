class Paging {
    constructor(data, tableId, headers, searchTermMap, rowLambda, btnClass, displyEmptyRows) {
        if (data[0] === undefined || data[0] === null)
            return;

        this.m_Data = data;
        this.m_SearchData = data;
        this.m_Table = document.getElementById(tableId);
        this.m_Headers = headers;
        this.m_RowLambda = rowLambda;
        this.m_BtnClass = btnClass;
        this.m_RowsToDisplay = 10;
        this.m_SearchQuery = "";
        this.m_SearchTermIndex = 0;
        this.m_SearchTerm = Object.getOwnPropertyNames(data[0])[0];
        this.m_DisplayEmptyRows = displyEmptyRows;
        this.m_SearchTermMap = searchTermMap;

        this.CreateTableHeader();
        this.CreateSearchBox();
        this.Page(1);

        this.m_Table.setAttribute("class", "table table-bordered table-hover");





    }

    Search() {
        var SearchResults = new Array();
        for (var i = 0; i < this.m_Data.length; i++) {
            try {
                if (this.m_Data[i][this.m_SearchTerm].toString().toUpperCase().includes(this.m_SearchBox.value.toString().toUpperCase())) {
                    SearchResults.push(this.m_Data[i]);
                }
            }
            catch (errA) {
                console.log(errA);
                this.m_SearchData = undefined;
                return;
            }
        }

        if (SearchResults === undefined || SearchResults.length === 0) {
            this.m_SearchData = undefined;
        }
        else {
            this.m_SearchData = SearchResults;
        }

        this.Page(1);
        this.m_SearchBox.focus();
    }

    ClearSearch() {
        this.m_SearchData = this.m_Data;
        this.m_SearchQuery = "";
        this.m_SearchBox.value = this.m_SearchQuery;
        this.Search();
    }

    //TOOD: Rewrite this function
    Page(page) {

        if (page < 1)
            return;

        if (this.m_SearchData !== undefined) {
            if (this.m_SearchData.length - ((page - 1) * this.m_RowsToDisplay) <= 0)
                return;
        }

        this.m_Table.innerHTML = "";
        this.CreateTableHeader();
        this.CreateSearchBox();

        if (this.m_SearchData !== undefined) {
            if (this.m_SearchData.length - ((page - 1) * this.m_RowsToDisplay) <= 0)
                return;

            this.m_Page = page;

            for (var i = (this.m_Page - 1) * this.m_RowsToDisplay; i < this.m_Page * this.m_RowsToDisplay; i++) {
                if (i < this.m_SearchData.length) {
                    this.m_RowLambda(this.m_SearchData[i], this.m_Table);
                }
                else if (this.m_DisplayEmptyRows === false || this.m_DisplayEmptyRows === undefined) {
                    break;
                }
                else {
                    var tr = document.createElement("tr");
                    var td = document.createElement("td");
                    td.setAttribute("colspan", this.m_Headers.length);
                    tr.appendChild(td);
                    tr.setAttribute("height", this.m_Table.rows.item(0).offsetHeight);
                    this.m_Table.appendChild(tr);
                }
            }
        }

        if (this.m_SearchData === undefined) {
            var tr = document.createElement("tr");

            var td = document.createElement("td");
            td.setAttribute("colspan", this.m_Headers.length);

            var div = document.createElement("div");
            div.setAttribute("style", "vertical-align:middle; text-align:center;");
            div.setAttribute("align", "center");

            var h1 = document.createElement("h1");
            h1.innerHTML = "No Results !";

            div.appendChild(h1);
            td.appendChild(div);
            tr.appendChild(td);

            this.m_Table.appendChild(tr);
        }

        var tr = document.createElement("tr");
        var td = document.createElement("td");
        var div = document.createElement("div");
        div.setAttribute("style", "vertical-align:middle;");
        div.setAttribute("align", "center");

        td.setAttribute("colspan", this.m_Headers.length);

        var btn = document.createElement("button");

        if (this.m_BtnClass !== undefined)
            btn.setAttribute("class", this.m_BtnClass);

        btn.innerHTML = "Prev";
        btn.setAttribute("style", "float:left;");
        btn.addEventListener("click", (e) => {
            this.Page(this.m_Page - 1);
        });

        div.appendChild(btn);

        btn = document.createElement("button");

        if (this.m_BtnClass !== undefined)
            btn.setAttribute("class", this.m_BtnClass);

        btn.innerHTML = "Next";
        btn.addEventListener("click", (e) => {
            this.Page(this.m_Page + 1);
        });
        btn.setAttribute("style", "float:right;");

        div.appendChild(btn);

        var sel = document.createElement("select");


        sel.setAttribute("style", "float:right; margin-right: 5px;")

        sel.setAttribute("class", "form-control");

        sel.addEventListener("change", (e) => {
            this.m_RowsToDisplay = parseInt(e.target.value);
            e.target.value = e.target.value;
            this.Page(this.m_Page);
        });

        var opt = document.createElement("option");
        opt.setAttribute("value", "5");
        opt.innerHTML = "5";

        sel.appendChild(opt);

        opt = document.createElement("option");
        opt.setAttribute("value", "10");
        opt.innerHTML = "10";

        sel.appendChild(opt);

        opt = document.createElement("option");
        opt.setAttribute("value", "15");
        opt.innerHTML = "15";

        sel.appendChild(opt);

        opt = document.createElement("option");
        opt.setAttribute("value", "20");
        opt.innerHTML = "20";

        sel.appendChild(opt);

        opt = document.createElement("option");
        opt.setAttribute("value", "25");
        opt.innerHTML = "25";

        opt = document.createElement("option");
        opt.setAttribute("value", "50");
        opt.innerHTML = "50";

        sel.appendChild(opt);
        sel.value = this.m_RowsToDisplay;

        var numPages = 1;
        if (this.m_SearchData !== undefined) {
            var numPages = parseInt(this.m_SearchData.length / this.m_RowsToDisplay);
            if (this.m_SearchData.length % this.m_RowsToDisplay !== 0)
                numPages += 1;
        }

        div.appendChild(sel);

        var p = document.createElement("p");
        p.innerHTML = "Page " + this.m_Page + " of " + numPages;

        div.appendChild(p);
        td.appendChild(div);
        tr.appendChild(td);
        this.m_Table.appendChild(tr);
    }

    CreateTableHeader() {
        this.m_TableHead = document.createElement("thead");
        var tr = document.createElement("tr");
        this.m_TableHead.appendChild(tr);
        for (var h = 0; h < this.m_Headers.length; h++) {
            var td = document.createElement("td");
            td.innerHTML = this.m_Headers[h];
            tr.appendChild(td);
        }

        this.m_Table.appendChild(this.m_TableHead);
    }

    CreateSearchBox() {
        var tr = document.createElement("tr");

        var td = document.createElement("td");
        td.setAttribute("colspan", this.m_Headers.length);

        var div = document.createElement("div");

        var sel = document.createElement("select");

        sel.setAttribute("class", "form-control");
        sel.setAttribute("style", "float:right;");
        sel.addEventListener("change", (e) => {
            this.m_SearchTerm = Object.getOwnPropertyNames(this.m_Data[0])[parseInt(e.target.value)];
            this.m_SearchTermIndex = e.target.value;
            this.Search();
        });

        for (var [k, v] of this.m_SearchTermMap) {
            var opt = document.createElement("option");
            opt.setAttribute("value", v.toString());
            opt.innerHTML = k;

            sel.appendChild(opt);
        }

        div.appendChild(sel);
        sel.value = this.m_SearchTermIndex;

        var clearBtn = document.createElement("button");

        if (this.m_BtnClass !== undefined)
            clearBtn.setAttribute("class", this.m_BtnClass);

        clearBtn.setAttribute("style", "float:right;");

        clearBtn.innerHTML = "Clear";

        clearBtn.addEventListener("click", (e) => {
            this.ClearSearch();
        });

        div.appendChild(clearBtn);

        this.m_SearchBox = document.createElement("input");

        this.m_SearchBox.setAttribute("class", "form-control");

        this.m_SearchBox.setAttribute("type", "text");
        this.m_SearchBox.setAttribute("placeholder", "Search...");
        this.m_SearchBox.setAttribute("style", "float:right;");
        this.m_SearchBox.value = this.m_SearchQuery;
        this.m_SearchBox.addEventListener("input", (e) => {
            if (e.target.value === "" || e.target.value === null)
                this.ClearSearch();

            this.m_SearchQuery = e.target.value;
            this.Search();
        });

        div.appendChild(this.m_SearchBox);

        td.appendChild(div);
        tr.appendChild(td);
        this.m_TableHead.insertBefore(tr, this.m_TableHead.childNodes[0]);
    }
}