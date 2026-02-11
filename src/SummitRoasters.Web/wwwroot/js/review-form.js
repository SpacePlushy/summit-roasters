// Review form - interactive star rating, AJAX submit

document.addEventListener('DOMContentLoaded', function () {
    initStarRating();
    initReviewSubmit();
});

function initStarRating() {
    var stars = document.querySelectorAll('[data-testid^="review-star-"]');
    var ratingInput = document.querySelector('[data-testid="review-rating-input"]');

    if (!stars.length || !ratingInput) return;

    stars.forEach(function (star) {
        star.addEventListener('click', function () {
            var rating = parseInt(star.getAttribute('data-rating'));
            ratingInput.value = rating;

            stars.forEach(function (s) {
                var starRating = parseInt(s.getAttribute('data-rating'));
                if (starRating <= rating) {
                    s.classList.add('text-amber-400');
                    s.classList.remove('text-gray-300');
                } else {
                    s.classList.remove('text-amber-400');
                    s.classList.add('text-gray-300');
                }
            });
        });

        star.addEventListener('mouseenter', function () {
            var hoverRating = parseInt(star.getAttribute('data-rating'));
            stars.forEach(function (s) {
                var starRating = parseInt(s.getAttribute('data-rating'));
                if (starRating <= hoverRating) {
                    s.classList.add('text-amber-300');
                } else {
                    s.classList.remove('text-amber-300');
                }
            });
        });
    });

    var container = stars[0]?.parentElement;
    if (container) {
        container.addEventListener('mouseleave', function () {
            var currentRating = parseInt(ratingInput.value) || 0;
            stars.forEach(function (s) {
                var starRating = parseInt(s.getAttribute('data-rating'));
                s.classList.remove('text-amber-300');
                if (starRating <= currentRating) {
                    s.classList.add('text-amber-400');
                    s.classList.remove('text-gray-300');
                }
            });
        });
    }
}

function initReviewSubmit() {
    var form = document.querySelector('[data-testid="review-form"]');
    if (!form) return;

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        var productId = form.querySelector('[name="productId"]')?.value;
        var rating = form.querySelector('[data-testid="review-rating-input"]')?.value;
        var title = form.querySelector('[data-testid="review-title-input"]')?.value;
        var body = form.querySelector('[data-testid="review-body-input"]')?.value;

        if (!rating || rating === '0') {
            if (window.showToast) window.showToast('Please select a rating', 'error');
            return;
        }

        fetchJson('/api/reviews', {
            method: 'POST',
            body: JSON.stringify({
                productId: parseInt(productId),
                rating: parseInt(rating),
                title: title,
                body: body
            })
        })
        .then(function (r) {
            if (r.ok) return r.json();
            return r.json().then(function (err) { throw err; });
        })
        .then(function () {
            if (window.showToast) window.showToast('Review submitted!');
            setTimeout(function () { location.reload(); }, 1000);
        })
        .catch(function (err) {
            var msg = (err && err.message) || 'Failed to submit review';
            if (window.showToast) window.showToast(msg, 'error');
        });
    });
}
