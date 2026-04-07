document.addEventListener('DOMContentLoaded', () => {
    const block = document.getElementById('bookOfDay');
    if (!block) return;

    const imgEl = document.getElementById('bookOfDayImg');
    const titleEl = document.getElementById('bookOfDayTitle');
    const authorEl = document.getElementById('bookOfDayAuthor');
    const yearEl = document.getElementById('bookOfDayYear');
    const linkEl = document.getElementById('bookOfDayLink');
    const detailsBtnEl = document.getElementById('bookOfDayDetailsBtn');
    const btn = document.getElementById('bookOfDayBtn');
    const statusEl = document.getElementById('bookOfDayStatus');

    let lastId = null;

    function setStatus(text) {
        if (statusEl) statusEl.textContent = text || '';
    }

    function renderBook(book) {
        if (imgEl) {
            imgEl.src = book.cover;
            imgEl.alt = book.title;
        }

        if (titleEl) titleEl.textContent = book.title;
        if (authorEl) authorEl.textContent = `Автор: ${book.author}`;
        if (yearEl) yearEl.textContent = `Год: ${book.year}`;

        if (linkEl) linkEl.href = book.href;
        if (detailsBtnEl) detailsBtnEl.href = book.href;
    }

    async function loadRandom() {
        try {
            setStatus('Загрузка...');
            if (btn) btn.disabled = true;

            const url = lastId
                ? `/Books/GetRandomBook?excludeId=${lastId}`
                : '/Books/GetRandomBook';

            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`Ошибка API: ${response.status}`);
            }

            const book = await response.json();
            lastId = book.id;
            renderBook(book);
            setStatus('');
        } catch (error) {
            console.error(error);
            setStatus('Ошибка загрузки книги');
        } finally {
            if (btn) btn.disabled = false;
        }
    }

    if (btn) {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            loadRandom();
        });
    }

    loadRandom();
});