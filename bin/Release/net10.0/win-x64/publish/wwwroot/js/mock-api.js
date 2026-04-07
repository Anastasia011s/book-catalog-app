// mock-api.js
// Имитация AJAX API для ЛР3 (без backend). Возвращает Promise + задержку.
// Позже можно заменить вызовы mockApi.* на реальные fetch('/api/...').

window.mockApi = (() => {
  let library = [
    { id: 1, title: 'Война и мир', author: 'Лев Толстой', year: 1869, status: 'read', statusLabel: 'Прочитано', addedAt: '12.05.2025', type: 'book' },
    { id: 2, title: 'Мастер и Маргарита', author: 'Михаил Булгаков', year: 1967, status: 'reading', statusLabel: 'В процессе', addedAt: '14.06.2025', type: 'book' },
    { id: 3, title: '1984', author: 'Джордж Оруэлл', year: 1949, status: 'planned', statusLabel: 'Хочу прочитать', addedAt: '02.07.2025', type: 'book' },
    { id: 4, title: 'Анна Каренина', author: 'Лев Толстой', year: 1878, status: 'read', statusLabel: 'Прочитано', addedAt: '21.08.2025', type: 'book' },
    { id: 5, title: 'Маленький принц', author: 'Антуан де Сент-Экзюпери', year: 1943, status: 'audio', statusLabel: 'Аудиокнига', addedAt: '05.09.2025', type: 'audio' },
    { id: 6, title: 'Идиот', author: 'Ф.М. Достоевский', year: 1869, status: 'planned', statusLabel: 'Хочу прочитать', addedAt: '10.09.2025', type: 'book' },
    { id: 7, title: 'Преступление и наказание', author: 'Ф.М. Достоевский', year: 1866, status: 'read', statusLabel: 'Прочитано', addedAt: '11.09.2025', type: 'book' },
    { id: 8, title: 'Отцы и дети', author: 'И.С. Тургенев', year: 1862, status: 'reading', statusLabel: 'В процессе', addedAt: '13.09.2025', type: 'book' },
    { id: 9, title: 'Гарри Поттер и философский камень', author: 'Дж. К. Роулинг', year: 1997, status: 'planned', statusLabel: 'Хочу прочитать', addedAt: '15.09.2025', type: 'book' }
  ];

  const delay = (result, ms = 350) => new Promise(resolve => setTimeout(() => resolve(structuredClone(result)), ms));

  function filterItems(items, status) {
    if (!status) return items;
    return items.filter(item => item.status === status);
  }

  return {
    async getLibrary({ status = '', page = 1, limit = 5 } = {}) {
      const filtered = filterItems(library, status);
      const total = filtered.length;
      const start = (page - 1) * limit;
      const items = filtered.slice(start, start + limit);
      return delay({ items, total, page, limit, hasMore: start + limit < total });
    },

    async deleteLibraryItem(id) {
      const before = library.length;
      library = library.filter(item => item.id !== Number(id));
      const success = library.length < before;
      return delay({ success, deletedId: Number(id), total: library.length });
    },

    async addReview(bookKey, { name, text }) {
      const review = { id: Date.now(), name, text };
      return delay({ success: true, review });
    }
  };
})();
