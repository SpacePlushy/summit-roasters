// Newsletter signup - AJAX submit, replace form with success message

document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('[data-testid="footer-newsletter-form"]');
    if (!form) return;

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        var emailInput = form.querySelector('[data-testid="footer-newsletter-email"]');
        var email = emailInput ? emailInput.value.trim() : '';

        if (!email) return;

        fetchJson('/api/newsletter', {
            method: 'POST',
            body: JSON.stringify({ email: email })
        })
        .then(function (r) { return r.json(); })
        .then(function (data) {
            var successMsg = document.createElement('p');
            successMsg.className = 'text-green-400 text-sm font-medium';
            successMsg.setAttribute('data-testid', 'footer-newsletter-success');
            successMsg.textContent = data.message || 'Thanks for subscribing!';
            form.replaceWith(successMsg);
        })
        .catch(function () {
            if (window.showToast) window.showToast('Failed to subscribe', 'error');
        });
    });
});
