// Product detail page - weight/grind selectors, quantity stepper, tabs

document.addEventListener('DOMContentLoaded', function () {
    initWeightSelector();
    initQuantityStepper();
    initTabs();
});

function initWeightSelector() {
    var buttons = document.querySelectorAll('[data-testid^="weight-option-"]');
    var priceDisplay = document.querySelector('[data-testid="product-price"]');
    var basePrice = priceDisplay ? parseFloat(priceDisplay.getAttribute('data-base-price')) : 0;

    buttons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            buttons.forEach(function (b) {
                b.classList.remove('ring-2', 'ring-amber-600', 'bg-amber-50');
                b.classList.add('border-gray-300');
            });
            btn.classList.add('ring-2', 'ring-amber-600', 'bg-amber-50');
            btn.classList.remove('border-gray-300');

            var adjustment = parseFloat(btn.getAttribute('data-price-adjustment')) || 0;
            var newPrice = basePrice + adjustment;
            if (priceDisplay) {
                priceDisplay.textContent = '$' + newPrice.toFixed(2);
            }
        });
    });
}

function initQuantityStepper() {
    var input = document.querySelector('[data-testid="quantity-input"]');
    var decBtn = document.querySelector('[data-testid="quantity-decrease"]');
    var incBtn = document.querySelector('[data-testid="quantity-increase"]');

    if (!input) return;

    if (decBtn) {
        decBtn.addEventListener('click', function () {
            var val = parseInt(input.value) || 1;
            if (val > 1) input.value = val - 1;
        });
    }

    if (incBtn) {
        incBtn.addEventListener('click', function () {
            var val = parseInt(input.value) || 1;
            if (val < 99) input.value = val + 1;
        });
    }
}

function initTabs() {
    var tabButtons = document.querySelectorAll('[data-testid^="tab-button-"]');
    var tabPanels = document.querySelectorAll('[data-testid^="tab-panel-"]');

    tabButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var target = btn.getAttribute('data-tab');

            tabButtons.forEach(function (b) {
                b.classList.remove('border-amber-600', 'text-amber-700');
                b.classList.add('border-transparent', 'text-gray-500');
            });
            btn.classList.add('border-amber-600', 'text-amber-700');
            btn.classList.remove('border-transparent', 'text-gray-500');

            tabPanels.forEach(function (panel) {
                panel.classList.add('hidden');
            });
            var targetPanel = document.querySelector('[data-testid="tab-panel-' + target + '"]');
            if (targetPanel) targetPanel.classList.remove('hidden');
        });
    });
}
