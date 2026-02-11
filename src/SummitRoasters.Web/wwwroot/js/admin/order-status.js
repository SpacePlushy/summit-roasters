// Admin order status - AJAX status update

document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('[data-testid="admin-order-status-form"]');
    if (!form) return;

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        var orderId = form.querySelector('[name="orderId"]')?.value;
        var status = form.querySelector('[data-testid="admin-order-status-select"]')?.value;
        var token = form.querySelector('input[name="__RequestVerificationToken"]')?.value || '';

        if (!orderId || !status) return;

        fetch('/admin/updateorderstatus?id=' + orderId + '&status=' + status, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            }
        })
        .then(function (r) {
            if (r.ok || r.redirected) {
                if (window.showToast) window.showToast('Order status updated!');
                setTimeout(function () { location.reload(); }, 500);
            } else {
                throw new Error('Failed');
            }
        })
        .catch(function () {
            if (window.showToast) window.showToast('Failed to update status', 'error');
        });
    });
});
