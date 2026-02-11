// Search autocomplete - debounced input, keyboard navigation

document.addEventListener('DOMContentLoaded', function () {
    var searchInput = document.querySelector('[data-testid="header-search-input"]');
    if (!searchInput) return;

    var dropdown = document.createElement('div');
    dropdown.className = 'absolute top-full left-0 right-0 bg-white border border-gray-200 rounded-b-lg shadow-lg z-50 hidden';
    dropdown.setAttribute('data-testid', 'search-autocomplete-dropdown');
    searchInput.parentElement.style.position = 'relative';
    searchInput.parentElement.appendChild(dropdown);

    var debounceTimer = null;
    var selectedIndex = -1;

    searchInput.addEventListener('input', function () {
        clearTimeout(debounceTimer);
        var query = searchInput.value.trim();
        if (query.length < 2) {
            dropdown.classList.add('hidden');
            return;
        }
        debounceTimer = setTimeout(function () { performSearch(query); }, 300);
    });

    searchInput.addEventListener('keydown', function (e) {
        var items = dropdown.querySelectorAll('[data-testid^="autocomplete-item-"]');
        if (e.key === 'ArrowDown') {
            e.preventDefault();
            selectedIndex = Math.min(selectedIndex + 1, items.length - 1);
            highlightItem(items);
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            selectedIndex = Math.max(selectedIndex - 1, -1);
            highlightItem(items);
        } else if (e.key === 'Enter' && selectedIndex >= 0 && items[selectedIndex]) {
            e.preventDefault();
            window.location.href = items[selectedIndex].getAttribute('href');
        } else if (e.key === 'Escape') {
            dropdown.classList.add('hidden');
            selectedIndex = -1;
        }
    });

    document.addEventListener('click', function (e) {
        if (!searchInput.parentElement.contains(e.target)) {
            dropdown.classList.add('hidden');
        }
    });

    function performSearch(query) {
        fetch('/api/products/search?q=' + encodeURIComponent(query))
            .then(function (r) { return r.json(); })
            .then(function (results) {
                selectedIndex = -1;
                if (results.length === 0) {
                    dropdown.classList.add('hidden');
                    return;
                }
                dropdown.textContent = '';
                results.forEach(function (item, i) {
                    var link = document.createElement('a');
                    link.href = '/products/' + item.slug;
                    link.className = 'block px-4 py-2 hover:bg-gray-50 flex items-center gap-3';
                    link.setAttribute('data-testid', 'autocomplete-item-' + i);

                    var nameSpan = document.createElement('span');
                    nameSpan.className = 'flex-1 text-sm text-gray-800';
                    nameSpan.textContent = item.name;
                    link.appendChild(nameSpan);

                    var priceSpan = document.createElement('span');
                    priceSpan.className = 'text-sm text-amber-700 font-medium';
                    priceSpan.textContent = '$' + item.price.toFixed(2);
                    link.appendChild(priceSpan);

                    dropdown.appendChild(link);
                });
                dropdown.classList.remove('hidden');
            });
    }

    function highlightItem(items) {
        items.forEach(function (item, i) {
            item.classList.toggle('bg-gray-100', i === selectedIndex);
        });
    }
});
