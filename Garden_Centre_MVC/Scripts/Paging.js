class Paging {
    constructor(data, tableId, headers, rowLambda, btnClass) {
        this.m_Data = data;
        this.m_SearchData = data;
        this.m_Table = document.getElementById(tableId);
        this.m_Headers = headers;
        this.m_RowLambda = rowLambda;
        this.m_BtnClass = btnClass;
        this.m_RowsToDisplay = 10;

        this.CreateTableHeader();
    }

    Search(TextBoxId, FieldToSearch) {
        var tb = document.getElementById(TextBoxId);

        this.SearchResults = new Array();

        for (var i = 0; i < this.m_Data.length; i++) {
            if (this.m_Data[i][FieldToSearch].toUpperCase().includes(tb.value.toUpperCase())) {
                this.SearchResults.push(this.m_Data[i]);
            }
        }

        alert(this.SearchResults.length);

        this.m_SearchData = this.SearchResults;

        this.Page(1)

    }

    ClearSearch() {
        this.m_SearchData = this.m_Data;
    }

    Page(page) {
        if (page < 1)
            return;

        if (this.m_SearchData.length - ((page - 1) * this.m_RowsToDisplay) <= 0)
            return;

        this.m_Page = page;

        this.m_Table.innerHTML = "";
        this.CreateTableHeader();

        for (var i = (this.m_Page - 1) * this.m_RowsToDisplay; i < this.m_Page * this.m_RowsToDisplay; i++) {
            if (i < this.m_SearchData.length) {
                this.m_RowLambda(this.m_SearchData[i], this.m_Table);
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

        var numPages = parseInt(this.m_SearchData.length / this.m_RowsToDisplay);
        if (this.m_SearchData.length % this.m_RowsToDisplay !== 0)
            numPages += 1;

        div.appendChild(sel);

        var p = document.createElement("p");
        p.innerHTML = "Page " + this.m_Page + " of " + numPages;

        div.appendChild(p);
        td.appendChild(div);
        tr.appendChild(td);
        this.m_Table.appendChild(tr);
    }

    CreateTableHeader() {
        var th = document.createElement("thead");
        var tr = document.createElement("tr");
        th.appendChild(tr);
        for (var h = 0; h < this.m_Headers.length; h++) {
            var td = document.createElement("td");
            td.innerHTML = this.m_Headers[h];
            tr.appendChild(td);
        }

        this.m_Table.appendChild(th);
    }
}