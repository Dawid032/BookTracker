function filterBooks() {
    const searchValue = document.getElementById('searchBar').value.toLowerCase();
    const table = document.querySelector('table');
    const rows = table.getElementsByTagName('tr');

    for (let i = 1; i < rows.length; i++) { // pomijamy nagłówki tabeli
        const title = rows[i].getElementsByTagName('td')[0].innerText.toLowerCase();
        const author = rows[i].getElementsByTagName('td')[1].innerText.toLowerCase();
        
        if (title.includes(searchValue) || author.includes(searchValue)) {
            rows[i].style.display = '';
        } else {
            rows[i].style.display = 'none';
        }
    }
}

function sortTable(columnIndex) {
    const table = document.querySelector('table');
    const rows = Array.from(table.rows).slice(1); // Pomijamy nagłówek
    const isAscending = table.dataset.sortOrder === 'asc';

    rows.sort((a, b) => {
        const aText = a.cells[columnIndex].innerText;
        const bText = b.cells[columnIndex].innerText;

        return isAscending
            ? aText.localeCompare(bText)
            : bText.localeCompare(aText);
    });

    // Reset sort order for next click
    table.dataset.sortOrder = isAscending ? 'desc' : 'asc';

    // Dodaj posortowane wiersze z powrotem do tabeli
    rows.forEach(row => table.appendChild(row));
}

function confirmAction(action, bookTitle) {
    const confirmMessage = action === 'start' 
        ? `Czy na pewno chcesz rozpocząć czytanie "${bookTitle}"?`
        : `Czy na pewno chcesz oznaczyć "${bookTitle}" jako przeczytaną?`;

    if (confirm(confirmMessage)) {
        // Implementacja akcji, np. wysłanie formularza lub wywołanie odpowiedniego endpointu
        alert(`${action === 'start' ? 'Rozpoczęto czytanie' : 'Oznaczono jako przeczytaną'}: ${bookTitle}`);
    }
}

function confirmDelete(bookTitle, bookId) {
    const confirmMessage = `Czy na pewno chcesz usunąć "${bookTitle}"?`;
    if (confirm(confirmMessage)) {
        // Wywołanie akcji usunięcia
        const form = document.createElement('form');
        form.method = 'post';
        form.action = `/Books?handler=Delete`; // Ustaw odpowiednią akcję

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = bookId; // Id książki do usunięcia

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit(); // Wyślij formularz
    }
}
