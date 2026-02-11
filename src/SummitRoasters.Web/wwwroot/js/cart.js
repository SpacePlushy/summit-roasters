// Cart JavaScript - AJAX add/update/remove, badge update

document.addEventListener('DOMContentLoaded', function () {
    initAddToCart();
    initCartQuantity();
    initCartRemove();
});

function initAddToCart() {
    document.querySelectorAll('[data-testid="add-to-cart-button"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var productId = btn.getAttribute('data-product-id');
            var quantity = 1;
            var qtyInput = document.querySelector('[data-testid="quantity-input"]');
            if (qtyInput) quantity = parseInt(qtyInput.value) || 1;

            var weightEl = document.querySelector('[data-testid^="weight-option-"].ring-2');
            var weight = weightEl ? weightEl.getAttribute('data-weight') : null;

            var grindEl = document.querySelector('[data-testid="grind-select"]');
            var grind = grindEl ? grindEl.value : null;

            var body = { productId: parseInt(productId), quantity: quantity };
            if (weight) body.weight = weight;
            if (grind) body.grind = grind;

            fetchJson('/api/cart/add', {
                method: 'POST',
                body: JSON.stringify(body)
            })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                updateCartBadge(data.itemCount);
                if (window.showToast) window.showToast('Added to cart!');
            })
            .catch(function () {
                if (window.showToast) window.showToast('Failed to add to cart', 'error');
            });
        });
    });
}

function initCartQuantity() {
    document.querySelectorAll('[data-testid^="cart-item-quantity-"]').forEach(function (input) {
        input.addEventListener('change', function () {
            var productId = input.getAttribute('data-product-id');
            var quantity = parseInt(input.value) || 1;

            fetchJson('/api/cart/update', {
                method: 'PUT',
                body: JSON.stringify({ productId: parseInt(productId), quantity: quantity })
            })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                updateCartBadge(data.itemCount);
                location.reload();
            });
        });
    });
}

function initCartRemove() {
    document.querySelectorAll('[data-testid^="cart-item-remove-"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            var productId = btn.getAttribute('data-product-id');

            fetchJson('/api/cart/remove/' + productId, {
                method: 'DELETE'
            })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                updateCartBadge(data.itemCount);
                location.reload();
            });
        });
    });
}

function updateCartBadge(count) {
    var badge = document.querySelector('[data-testid="header-cart-badge"]');
    if (badge) {
        badge.textContent = count;
        badge.style.display = count > 0 ? 'flex' : 'none';
    }
}
