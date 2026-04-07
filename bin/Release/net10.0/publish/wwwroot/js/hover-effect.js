document.addEventListener("DOMContentLoaded", function () {

    const books = document.querySelectorAll(".book");

    books.forEach(book => {
        const cover = book.querySelector(".cover");
        const title = book.querySelector("h2").innerText;

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
