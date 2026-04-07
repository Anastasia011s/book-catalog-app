document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("bookForm");
    if (!form) return;

    form.addEventListener("submit", function (event) {
        document.querySelectorAll(".error-popup").forEach(e => e.remove());

        let hasError = false;

        const title = document.getElementById("title");
        const author = document.getElementById("author");
        const genre = document.getElementById("genre");
        const year = document.getElementById("year");
        const file = document.getElementById("file");

        if (!title.value.trim() || title.value.trim().length < 3) {
            showPopup(title, "Название книги должно содержать минимум 3 символа");
            hasError = true;
        }

        if (!author.value.trim()) {
            showPopup(author, "Введите автора");
            hasError = true;
        }

        if (!genre.value.trim()) {
            showPopup(genre, "Введите жанр");
            hasError = true;
        }

        const yearValue = parseInt(year.value, 10);
        if (isNaN(yearValue) || yearValue < 1500 || yearValue > 2026) {
            showPopup(year, "Год издания должен быть числом от 1500 до 2026");
            hasError = true;
        }

        if (file.files.length === 0) {
            showPopup(file, "Пожалуйста, загрузите обложку книги");
            hasError = true;
        }

        if (hasError) {
            event.preventDefault();
        }
    });

    function showPopup(element, message) {
        const popup = document.createElement("div");
        popup.className = "error-popup";
        popup.textContent = message;

        popup.style.position = "absolute";
        popup.style.background = "#b4004e";
        popup.style.color = "#fff";
        popup.style.fontSize = "13px";
        popup.style.padding = "6px 10px";
        popup.style.borderRadius = "6px";
        popup.style.marginTop = "4px";
        popup.style.zIndex = "1000";
        popup.style.maxWidth = "260px";

        const parent = element.parentElement;
        if (parent) {
            parent.style.position = "relative";
            parent.appendChild(popup);
            popup.style.top = (element.offsetTop + element.offsetHeight + 4) + "px";
            popup.style.left = "0px";
        }
    }
});