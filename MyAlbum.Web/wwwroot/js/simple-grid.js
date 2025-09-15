// wwwroot/js/simple-grid.js
(function (global) {
    // 先確保 global.SimpleGrid 存在
    global.SimpleGrid = global.SimpleGrid || {};
    const MAX_PAGE_BTNS = 7;

    const fmt = {
        date: (s) => {
            if (!s) return "";
            if (/^\/Date\(\d+\)\/$/.test(s)) {
                const ms = parseInt(s.match(/\d+/)[0], 10);
                return new Date(ms).toISOString().substring(0, 10);
            }
            return s.toString().substring(0, 10);
        },
        bool: (b) => (b ? "✅" : "❌")
    };

    function esc(s) {
        return (s ?? "").toString()
            .replace(/&/g, "&amp;").replace(/</g, "&lt;")
            .replace(/>/g, "&gt;").replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    function getAntiForgeryToken(form) {
        const input = form ? form.querySelector('input[name="__RequestVerificationToken"]')
            : document.querySelector('input[name="__RequestVerificationToken"]');
        return input ? input.value : "";
    }

    async function load(grid) {
        const tbody = document.getElementById(grid.options.TableId);
        const pagerEl = document.getElementById(grid.options.PagerId);
        const totalEl = document.getElementById(grid.options.TotalId);
        if (!tbody || !pagerEl || !totalEl) return;

        tbody.innerHTML = `<tr><td colspan="${grid.options.Columns.length}" class="text-center text-muted">載入中…</td></tr>`;

        // 組 payload
        const payload = {
            pageIndex: grid.state.pageIndex,
            pageSize: grid.state.pageSize,
            data: grid.getSearchData()
        };

        const headers = { 'Content-Type': 'application/json' };
        if (grid.token) headers['RequestVerificationToken'] = grid.token;

        const res = await fetch(grid.options.ListUrl, {
            method: 'POST',
            headers, body: JSON.stringify(payload)
        });

        if (!res.ok) {
            tbody.innerHTML = `<tr><td colspan="${grid.options.Columns.length}" class="text-center text-danger">讀取失敗 (${res.status})</td></tr>`;
            pagerEl.innerHTML = ""; totalEl.textContent = "";
            return;
        }

        const json = await res.json(); // { items, total }
        const items = json.items ?? [];
        const total = Number(json.total ?? 0);

        // 渲染 rows
        if (!items.length) {
            tbody.innerHTML = `<tr><td colspan="${grid.options.Columns.length}" class="text-center text-muted">查無資料</td></tr>`;
        } else {
            const rows = items.map(it => {
                const tds = grid.options.Columns.map(c => {
                    const raw = it[c.field] ?? it[c.Field] ?? ""; // 容錯
                    const formatter = (c.format || c.Format) ? ((global.cellFormatters && global.cellFormatters[c.format || c.Format]) || fmt[c.format || c.Format]) : null;
                    const text = formatter ? formatter(raw, it) : esc(raw);
                    return `<td>${text}</td>`;
                }).join("");
                return `<tr>${tds}</tr>`;
            }).join("");
            tbody.innerHTML = rows;
        }

        // 分頁
        renderPager(grid, total);
    }

    function renderPager(grid, total) {
        grid.state.total = total;
        const pagerEl = document.getElementById(grid.options.PagerId);
        const totalEl = document.getElementById(grid.options.TotalId);
        pagerEl.innerHTML = "";

        const pages = Math.max(1, Math.ceil(total / grid.state.pageSize));
        const cur = Math.min(Math.max(1, grid.state.pageIndex), pages);
        grid.state.pageIndex = cur;

        const addBtn = (text, page, disabled = false, active = false) => {
            const btn = document.createElement('button');
            btn.type = "button";
            btn.className = `btn btn-sm ${active ? 'btn-primary' : 'btn-outline-secondary'}`;
            btn.textContent = text;
            btn.disabled = disabled;
            btn.onclick = () => { if (!disabled) { grid.state.pageIndex = page; load(grid); } };
            pagerEl.appendChild(btn);
        };

        // "<"
        addBtn("<", cur - 1, cur === 1);

        // 計算頁碼窗口
        let start = Math.max(1, cur - Math.floor(MAX_PAGE_BTNS / 2));
        let end = start + MAX_PAGE_BTNS - 1;
        if (end > pages) { end = pages; start = Math.max(1, end - MAX_PAGE_BTNS + 1); }

        if (start > 1) {
            addBtn("1", 1, false, cur === 1);
            if (start > 2) pagerEl.appendChild(makeEllipsis());
        }
        for (let p = start; p <= end; p++) addBtn(String(p), p, false, p === cur);
        if (end < pages) {
            if (end < pages - 1) pagerEl.appendChild(makeEllipsis());
            addBtn(String(pages), pages, false, cur === pages);
        }

        // ">"
        addBtn(">", cur + 1, cur === pages);

        // 共 N 筆
        totalEl.textContent = `共 ${total} 筆`;
    }

    function makeEllipsis() {
        const span = document.createElement('span');
        span.className = "btn btn-sm btn-outline-secondary disabled";
        span.textContent = "…";
        return span;
    }

    function init(options) {
        const grid = {
            options,
            state: { pageIndex: 1, pageSize: options.PageSize || 10, total: 0 },
            token: null,
            getSearchData: () => {
                if (!options.SearchFormId) return null;
                const form = document.getElementById(options.SearchFormId);
                if (!form) return null;
                const data = {};
                new FormData(form).forEach((v, k) => {
                    if (k === "__RequestVerificationToken") return;
                    const s = String(v).trim();
                    if (s.length === 0) return;
                    data[k] = s;
                });
                return { ...data };
            }
        };

        if (options.SearchFormId) {
            const form = document.getElementById(options.SearchFormId);
            grid.token = getAntiForgeryToken(form);
            if (form) {
                form.addEventListener('submit', function (e) {
                    e.preventDefault();
                    grid.state.pageIndex = 1;
                    load(grid);
                });
            }
        } else {
            grid.token = getAntiForgeryToken(null);
        }

        load(grid);
        return grid;
    }

    // 在這裡掛上方法（此時 SimpleGrid 一定存在）
    global.SimpleGrid.init = init;

})(window);

