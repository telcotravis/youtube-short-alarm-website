// YT Shorts Alarm website interactions
document.addEventListener('DOMContentLoaded', function () {
    // Close mobile nav on link click
    var navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    var navCollapse = document.querySelector('.navbar-collapse');
    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            if (navCollapse.classList.contains('show')) {
                new bootstrap.Collapse(navCollapse, { toggle: true });
            }
        });
    });

    // Navbar background opacity on scroll
    var navbar = document.querySelector('.navbar');
    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) {
            navbar.style.backgroundColor = 'rgba(26, 18, 13, 0.98)';
        } else {
            navbar.style.backgroundColor = 'rgba(26, 18, 13, 0.95)';
        }
    });
});
