document.addEventListener("DOMContentLoaded", function () {
    const addButton = document.getElementById("addReviewBtn");
    const nameInput = document.getElementById("reviewerName");
    const textInput = document.getElementById("reviewText");
    const reviewSection = document.querySelector(".reviews");
    const reviewStatus = document.getElementById("reviewStatus");

    addButton.addEventListener("click", async () => {
        const name = nameInput.value.trim();
        const text = textInput.value.trim();

        if (name === "" || text === "") {
            alert("Пожалуйста, заполните все поля!");
            return;
        }

        addButton.disabled = true;
        reviewStatus.textContent = "Отправка отзыва...";
        reviewStatus.style.display = "block";

        try {
            const response = await mockApi.addReview("warpeace", { name, text });

            const newReview = document.createElement("div");
            newReview.classList.add("review", "new-review");
            newReview.innerHTML = `
                <p><strong>${response.review.name}</strong></p>
                <p class="rating">⭐️⭐️⭐️⭐️⭐️</p>
                <p>${response.review.text}</p>
            `;

            const addReviewBlock = document.querySelector(".add-review");
            reviewSection.insertBefore(newReview, addReviewBlock);

            nameInput.value = "";
            textInput.value = "";

            reviewStatus.textContent = "✅ Отзыв добавлен (AJAX)";
            setTimeout(() => reviewStatus.style.display = "none", 1500);
        } catch (e) {
            console.error(e);
            reviewStatus.textContent = "❌ Ошибка отправки";
        } finally {
            addButton.disabled = false;
        }
    });
});