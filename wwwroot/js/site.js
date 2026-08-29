// Church of Pantoufle — Sacred Scripts

// Scroll reveal animation
document.addEventListener('DOMContentLoaded', function () {
    const reveals = document.querySelectorAll('.reveal');
    if (reveals.length === 0) return;

    const observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, { threshold: 0.1 });

    reveals.forEach(function (el) { observer.observe(el); });
});

// Donation tier selection
document.addEventListener('DOMContentLoaded', function () {
    var tiers = document.querySelectorAll('.donation-tier');
    var amountInput = document.getElementById('Amount');
    var tierInput = document.getElementById('SelectedTier');

    tiers.forEach(function (tier) {
        tier.addEventListener('click', function () {
            tiers.forEach(function (t) { t.classList.remove('selected'); });
            tier.classList.add('selected');
            if (amountInput) amountInput.value = tier.dataset.amount;
            if (tierInput) tierInput.value = tier.dataset.tier;
        });
    });
});
