document.addEventListener('DOMContentLoaded', function () {
    // Kiểm tra xem element có tồn tại không trước khi khởi tạo
    if (document.querySelector('.top-ranked-slider-v2')) {
        var swiper = new Swiper('.top-ranked-slider-v2', {
            slidesPerView: 'auto',
            spaceBetween: 24,
            loop: false,
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
            breakpoints: {
                320: { spaceBetween: 16 },
                768: { spaceBetween: 20 },
                1024: { spaceBetween: 24 }
            }
        });
    }
});