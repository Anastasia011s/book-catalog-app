document.addEventListener("DOMContentLoaded", function () {
    const table = document.querySelector("table");
    const tbody = table.querySelector("tbody");
    const applyBtn = document.querySelector(".filter-btn");
    const showMoreBtn = document.getElementById("showMoreBtn");
    const filterLinks = document.querySelectorAll("aside ul li a");
    const countEl = document.getElementById("booksCount");
    const selectedEl = document.getElementById("selectedFilter");
    const loadingEl = document.getElementById("libraryLoading");

    let selectedStatus = "";
    let currentPage = 1;
    const LIMIT = 5;

    const statusMap = {
        "Прочитанные": { code: "read", label: "Прочитано" },
        "В процессе": { code: "reading", label: "В процессе" },
        "Хочу прочитать": { code: "planned", label: "Хочу прочитать" },
        "Аудиокниги": { code: "audio", label: "Аудиокнига" }
    };

    function setLoading(isLoading) {
        loadingEl.style.display = isLoading ? "inline" : "none";
        applyBtn.disabled = isLoading;
        if (showMoreBtn) showMoreBtn.disabled = isLoading;
    }

    function renderRows(items, append = false) {
        if (!append) tbody.innerHTML = "";

        if (!items.length && !append) {
            tbody.innerHTML = `<tr><td colspan="6" style="padding:20px;">По выбранному фильтру книги не найдены</td></tr>`;
            return;
        }

        items.forEach(book => {
            const tr = document.createElement("tr");
            tr.dataset.id = book.id;
            tr.innerHTML = `
                <td>${book.title}</td>
                <td>${book.author}</td>
                <td>${book.year}</td>
                <td>${book.statusLabel}</td>
                <td>${book.addedAt}</td>
                <td><button class="delete-btn" data-id="${book.id}">Удалить</button></td>
            `;
            tbody.appendChild(tr);
        });
    }

    async function loadLibraryPage(page, append = false) {
        setLoading(true);
        try {
            const data = await mockApi.getLibrary({ status: selectedStatus, page, limit: LIMIT });
            renderRows(data.items, append);
            countEl.textContent = data.total;
            showMoreBtn.style.display = data.hasMore ? "inline-block" : "none";
        } catch (e) {
            alert("Ошибка загрузки библиотеки");
            console.error(e);
        } finally {
            setLoading(false);
        }
    }

    filterLinks.forEach(link => {
        link.addEventListener("click", (e) => {
            e.preventDefault();
            filterLinks.forEach(l => l.classList.remove("active-filter"));
            link.classList.add("active-filter");
            const config = statusMap[link.textContent.trim()];
            selectedStatus = config ? config.code : "";
            selectedEl.textContent = config ? config.label : "Все";
        });
    });

    applyBtn.addEventListener("click", async () => {
        currentPage = 1;
        await loadLibraryPage(currentPage, false);
    });

    showMoreBtn.addEventListener("click", async () => {
        currentPage += 1;
        await loadLibraryPage(currentPage, true);
    });

    tbody.addEventListener("click", async (e) => {
        const btn = e.target.closest(".delete-btn");
        if (!btn) return;
        const id = btn.dataset.id;
        if (!confirm("Удалить книгу из библиотеки?")) return;
        setLoading(true);
        try {
            const result = await mockApi.deleteLibraryItem(id);
            if (result.success) {
                btn.closest("tr")?.remove();
                countEl.textContent = result.total;
            }
        } catch (e) {
            alert("Ошибка удаления");
            console.error(e);
        } finally {
            setLoading(false);
        }
    });

    loadLibraryPage(currentPage, false);
});