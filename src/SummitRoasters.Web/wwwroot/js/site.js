// Site-wide JavaScript - Mobile menu, user dropdown, toast notifications

document.addEventListener('DOMContentLoaded', function () {
    initMobileMenu();
    initUserDropdown();
    initToastNotifications();
});

// Mobile Menu
function initMobileMenu() {
    const toggle = document.querySelector('[data-testid="header-mobile-menu-toggle"]');
    const menu = document.querySelector('[data-testid="mobile-menu"]');
    const close = document.querySelector('[data-testid="mobile-menu-close"]');
    const overlay = document.querySelector('[data-testid="mobile-menu-overlay"]');

    if (!toggle || !menu) return;

    toggle.addEventListener('click', function () {
        menu.classList.remove('translate-x-full');
        menu.classList.add('translate-x-0');
        overlay?.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
    });

    function closeMenu() {
        menu.classList.add('translate-x-full');
        menu.classList.remove('translate-x-0');
        overlay?.classList.add('hidden');
        document.body.style.overflow = '';
    }

    close?.addEventListener('click', closeMenu);
    overlay?.addEventListener('click', closeMenu);
}

// User Dropdown
function initUserDropdown() {
    const trigger = document.querySelector('[data-testid="header-user-menu-trigger"]');
    const dropdown = document.querySelector('[data-testid="header-user-menu-dropdown"]');

    if (!trigger || !dropdown) return;

    trigger.addEventListener('click', function (e) {
        e.stopPropagation();
        dropdown.classList.toggle('hidden');
    });

    document.addEventListener('click', function () {
        dropdown.classList.add('hidden');
    });
}

// Toast Notifications
function initToastNotifications() {
    window.showToast = function (message, type) {
        type = type || 'success';
        const container = document.querySelector('[data-testid="toast-container"]');
        if (!container) return;

        const toast = document.createElement('div');
        const bgColor = type === 'success' ? 'bg-green-600' : type === 'error' ? 'bg-red-600' : 'bg-amber-600';
        toast.className = bgColor + ' text-white px-6 py-3 rounded-lg shadow-lg flex items-center gap-3 transform translate-x-full transition-transform duration-300';
        toast.setAttribute('data-testid', 'toast-message');
        toast.textContent = message;

        container.appendChild(toast);

        requestAnimationFrame(function () {
            toast.classList.remove('translate-x-full');
            toast.classList.add('translate-x-0');
        });

        setTimeout(function () {
            toast.classList.add('translate-x-full');
            toast.classList.remove('translate-x-0');
            setTimeout(function () { toast.remove(); }, 300);
        }, 3000);
    };
}

// CSRF token helper
function getCsrfToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
}

// Utility: fetch with JSON and CSRF
function fetchJson(url, options) {
    options = options || {};
    var token = getCsrfToken();
    options.headers = Object.assign({
        'Content-Type': 'application/json',
        'RequestVerificationToken': token
    }, options.headers || {});
    return fetch(url, options);
}
