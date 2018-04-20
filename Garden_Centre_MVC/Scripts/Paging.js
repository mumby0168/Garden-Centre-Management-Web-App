class Paging {
    constructor(data, tableId, headers, rowLambda, btnClass) {
        this.m_Data = data;
        this.m_Table = document.getElementById(tableId);
        this.m_Headers = headers;
        this.m_RowLambda = rowLambda;
        this.m_BtnClass = btnClass;

        this.CreateTableHeader();
    }

    Page(page) {
        if (page < 1)
            return;

        if (this.m_Data.length - ((page - 1) * 10) <= 0)
            return;

        this.m_Page = page;

        this.m_Table.innerHTML = "";
        this.CreateTableHeader();

        for (var i = (this.m_Page - 1) * 10; i < this.m_Page * 10; i++) {
            if (i < this.m_Data.length) {
                this.m_RowLambda(this.m_Data[i], this.m_Table);
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