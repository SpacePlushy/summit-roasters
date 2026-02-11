// Admin product form - flavor notes tag input, slug generation

document.addEventListener('DOMContentLoaded', function () {
    initSlugGeneration();
    initFlavorNotes();
});

function initSlugGeneration() {
    var nameInput = document.querySelector('[data-testid="admin-product-form-name"]');
    var slugInput = document.querySelector('[data-testid="admin-product-form-slug"]');

    if (!nameInput || !slugInput) return;

    nameInput.addEventListener('input', function () {
        if (!slugInput.getAttribute('data-manual')) {
            slugInput.value = nameInput.value
                .toLowerCase()
                .replace(/[^a-z0-9\s-]/g, '')
                .replace(/\s+/g, '-')
                .replace(/-+/g, '-')
                .replace(/^-|-$/g, '');
        }
    });

    slugInput.addEventListener('input', function () {
        slugInput.setAttribute('data-manual', 'true');
    });
}

function initFlavorNotes() {
    var input = document.querySelector('[data-testid="admin-product-form-flavor-input"]');
    var container = document.querySelector('[data-testid="admin-product-form-flavor-tags"]');
    var hiddenInput = document.querySelector('[data-testid="admin-product-form-flavor-notes"]');

    if (!input || !container || !hiddenInput) return;

    var tags = hiddenInput.value ? hiddenInput.value.split(',').map(function (t) { return t.trim(); }).filter(Boolean) : [];
    renderTags();

    input.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' || e.key === ',') {
            e.preventDefault();
            var value = input.value.trim().replace(/,/g, '');
            if (value && tags.indexOf(value) === -1) {
                tags.push(value);
                renderTags();
                updateHidden();
            }
            input.value = '';
        }
    });

    function renderTags() {
        container.textContent = '';
        tags.forEach(function (tag, i) {
            var span = document.createElement('span');
            span.className = 'inline-flex items-center gap-1 px-2 py-1 bg-amber-100 text-amber-800 rounded-full text-sm';

            var text = document.createElement('span');
            text.textContent = tag;
            span.appendChild(text);

            var removeBtn = document.createElement('button');
            removeBtn.type = 'button';
            removeBtn.className = 'text-amber-600 hover:text-amber-800 font-bold';
            removeBtn.textContent = '\u00d7';
            removeBtn.addEventListener('click', function () {
                tags.splice(i, 1);
                renderTags();
                updateHidden();
            });
            span.appendChild(removeBtn);

            container.appendChild(span);
        });
    }

    function updateHidden() {
        hiddenInput.value = tags.join(', ');
    }
}
