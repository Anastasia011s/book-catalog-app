document.addEventListener("DOMContentLoaded", function () {
    const coverImg = document.querySelector(".cover img");


    const modal = document.createElement("div");
    modal.className = "modal-overlay";
    modal.innerHTML = `
        <div class="modal-content">
            <img src="${coverImg.src}" alt="Обложка книги" class="modal-image">
            <button class="close-modal">×</button>
        </div>
    `;
    document.body.appendChild(modal);


    coverImg.addEventListener("click", () => {
        modal.classList.add("visible");
    });


    modal.querySelector(".close-modal").addEventListener("click", () => {
        modal.classList.remove("visible");
    });


    modal.addEventListener("click", (e) => {
        if (e.target === modal) modal.classList.remove("visible");
    });
});
