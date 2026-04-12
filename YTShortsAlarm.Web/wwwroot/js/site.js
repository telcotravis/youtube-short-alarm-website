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

    // Navbar background opacity on scroll with border
    var navbar = document.querySelector('.navbar');
    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) {
            navbar.style.backgroundColor = 'rgba(17, 19, 24, 0.98)';
            navbar.style.borderBottom = '1px solid rgba(255, 255, 255, 0.06)';
        } else {
            navbar.style.backgroundColor = 'rgba(17, 19, 24, 0.95)';
            navbar.style.borderBottom = '1px solid transparent';
        }
    });

    // Scroll reveal animation with IntersectionObserver
    var revealElements = document.querySelectorAll('.reveal');
    if (revealElements.length > 0) {
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        revealElements.forEach(function (el) {
            observer.observe(el);
        });
    }

    // Screenshot carousel
    function initCarousel(container) {
        var track = container.querySelector('.carousel-track');
        var slides = track.querySelectorAll('.carousel-slide');
        var prevBtn = container.querySelector('.carousel-btn-prev');
        var nextBtn = container.querySelector('.carousel-btn-next');
        var dotsContainer = container.querySelector('.carousel-dots');
        var currentIndex = 0;

        function getSlidesPerView() {
            return window.innerWidth >= 768 ? 3 : 1;
        }

        function getMaxIndex() {
            return Math.max(0, slides.length - getSlidesPerView());
        }

        function buildDots() {
            dotsContainer.innerHTML = '';
            var maxIdx = getMaxIndex();
            for (var i = 0; i <= maxIdx; i++) {
                var dot = document.createElement('button');
                dot.className = 'carousel-dot' + (i === currentIndex ? ' active' : '');
                dot.setAttribute('aria-label', 'Go to slide ' + (i + 1));
                dot.dataset.index = i;
                dot.addEventListener('click', function () {
                    goTo(parseInt(this.dataset.index));
                });
                dotsContainer.appendChild(dot);
            }
        }

        function updateUI() {
            var pct = currentIndex * (100 / getSlidesPerView());
            track.style.transform = 'translateX(-' + pct + '%)';
            prevBtn.disabled = currentIndex <= 0;
            nextBtn.disabled = currentIndex >= getMaxIndex();
            var dots = dotsContainer.querySelectorAll('.carousel-dot');
            dots.forEach(function (d, i) {
                d.classList.toggle('active', i === currentIndex);
            });
        }

        function goTo(index) {
            currentIndex = Math.max(0, Math.min(index, getMaxIndex()));
            updateUI();
        }

        prevBtn.addEventListener('click', function () { goTo(currentIndex - 1); });
        nextBtn.addEventListener('click', function () { goTo(currentIndex + 1); });

        // Touch swipe
        var touchStartX = 0;
        var touchDeltaX = 0;
        var isSwiping = false;
        track.addEventListener('touchstart', function (e) {
            touchStartX = e.touches[0].clientX;
            touchDeltaX = 0;
            isSwiping = true;
            track.style.transition = 'none';
        }, { passive: true });

        track.addEventListener('touchmove', function (e) {
            if (!isSwiping) return;
            touchDeltaX = e.touches[0].clientX - touchStartX;
            var basePct = currentIndex * (100 / getSlidesPerView());
            var dragPct = (touchDeltaX / track.parentElement.offsetWidth) * 100;
            track.style.transform = 'translateX(' + (-basePct + dragPct) + '%)';
        }, { passive: true });

        track.addEventListener('touchend', function () {
            if (!isSwiping) return;
            isSwiping = false;
            track.style.transition = '';
            if (Math.abs(touchDeltaX) > 50) {
                goTo(currentIndex + (touchDeltaX < 0 ? 1 : -1));
            } else {
                updateUI();
            }
        });

        // Recalculate on resize
        var resizeTimer;
        window.addEventListener('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                if (currentIndex > getMaxIndex()) currentIndex = getMaxIndex();
                buildDots();
                updateUI();
            }, 150);
        });

        buildDots();
        updateUI();
    }

    // Initialize all carousels
    document.querySelectorAll('.screenshot-carousel').forEach(initCarousel);

    // Screenshot theme toggle
    var themeButtons = document.querySelectorAll('.theme-toggle-btn');
    themeButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var theme = this.dataset.theme;
            themeButtons.forEach(function (b) {
                b.classList.remove('active');
                b.setAttribute('aria-pressed', 'false');
            });
            this.classList.add('active');
            this.setAttribute('aria-pressed', 'true');

            var darkCarousel = document.getElementById('screenshots-dark');
            var lightCarousel = document.getElementById('screenshots-light');
            if (theme === 'dark') {
                darkCarousel.classList.remove('d-none');
                lightCarousel.classList.add('d-none');
            } else {
                darkCarousel.classList.add('d-none');
                lightCarousel.classList.remove('d-none');
            }
        });
    });
});
