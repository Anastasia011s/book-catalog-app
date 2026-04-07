document.addEventListener("DOMContentLoaded", function () {

    const books = document.querySelectorAll(".book");

    if (!books.length) return;

    books.forEach(book => {

        // защита от повторного создания tooltip
        if (book.querySelector(".book-tooltip")) return;

        const titleEl = book.querySelector("h2");
        if (!titleEl) return;

        const title = titleEl.innerText.trim();

        const tooltip = document.createElement("div");
        tooltip.className = "book-tooltip";
        tooltip.textContent = `📖 Подробнее о книге «${title}»`;

        book.appendChild(tooltip);

        book.addEventListener("mouseenter", () => {
            tooltip.classList.add("visible");
        });

        book.addEventListener("mouseleave", () => {
            tooltip.classList.remove("visible");
        });
    });
});