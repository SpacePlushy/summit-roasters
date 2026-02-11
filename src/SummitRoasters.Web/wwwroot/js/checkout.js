// Checkout - multi-step section toggling, address selection, form validation

document.addEventListener('DOMContentLoaded', function () {
    initCheckoutSteps();
    initAddressSelection();
    initDiscountCode();
});

function initCheckoutSteps() {
    var continueToReview = document.querySelector('[data-testid="checkout-continue-review"]');
    var backToShipping = document.querySelector('[data-testid="checkout-back-shipping"]');
    var shippingStep = document.querySelector('[data-testid="checkout-step-shipping"]');
    var reviewStep = document.querySelector('[data-testid="checkout-step-review"]');

    if (continueToReview) {
        continueToReview.addEventListener('click', function (e) {
            e.preventDefault();
            // Validate shipping form
            var form = document.querySelector('[data-testid="checkout-shipping-form"]');
            if (form && !form.checkValidity()) {
                form.reportValidity();
                return;
            }
            if (shippingStep) shippingStep.classList.add('hidden');
            if (reviewStep) reviewStep.classList.remove('hidden');
            window.scrollTo(0, 0);
        });
    }

    if (backToShipping) {
        backToShipping.addEventListener('click', function (e) {
            e.preventDefault();
            if (reviewStep) reviewStep.classList.add('hidden');
            if (shippingStep) shippingStep.classList.remove('hidden');
            window.scrollTo(0, 0);
        });
    }
}

function initAddressSelection() {
    var addressCards = document.querySelectorAll('[data-testid^="saved-address-"]');
    var newAddressForm = document.querySelector('[data-testid="checkout-new-address-form"]');
    var useNewBtn = document.querySelector('[data-testid="checkout-use-new-address"]');

    addressCards.forEach(function (card) {
        card.addEventListener('click', function () {
            addressCards.forEach(function (c) {
                c.classList.remove('ring-2', 'ring-amber-600');
            });
            card.classList.add('ring-2', 'ring-amber-600');
            if (newAddressForm) newAddressForm.classList.add('hidden');

            // Copy saved address values to hidden inputs
            var addressId = card.getAttribute('data-address-id');
            var hiddenInput = document.querySelector('input[name="selectedAddressId"]');
            if (hiddenInput) hiddenInput.value = addressId;
        });
    });

    if (useNewBtn) {
        useNewBtn.addEventListener('click', function (e) {
            e.preventDefault();
            addressCards.forEach(function (c) {
                c.classList.remove('ring-2', 'ring-amber-600');
            });
            if (newAddressForm) newAddressForm.classList.remove('hidden');
            var hiddenInput = document.querySelector('input[name="selectedAddressId"]');
            if (hiddenInput) hiddenInput.value = '';
        });
    }
}

function initDiscountCode() {
    var applyBtn = document.querySelector('[data-testid="checkout-apply-discount"]');
    var discountInput = document.querySelector('[data-testid="checkout-discount-input"]');

    if (applyBtn && discountInput) {
        applyBtn.addEventListener('click', function (e) {
            e.preventDefault();
            var code = discountInput.value.trim();
            if (!code) return;

            // The discount will be applied server-side on form submission
            if (window.showToast) window.showToast('Discount code will be applied at checkout');
        });
    }
}
