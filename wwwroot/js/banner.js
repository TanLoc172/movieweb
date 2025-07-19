// document.addEventListener('DOMContentLoaded', () => {
//     const carousel = {
//         items: document.querySelectorAll('.banner-item'),
//         prevBtn: document.querySelector('.banner-nav-prev'),
//         nextBtn: document.querySelector('.banner-nav-next'),
//         indicatorDots: document.querySelectorAll('.indicator-dot'),
//         currentSlide: 0,
//         autoPlayDelay: 5000,
//         autoPlayTimer: null,

//         init() {
//             if (!this.items.length) return;
//             this.showSlide(this.currentSlide);
//             this.startAutoPlay();
//             this.bindEvents();
//         },

//         showSlide(index) {
//             this.stopAllTrailers();
//             this.items.forEach((item, i) => {
//                 item.style.display = i === index ? 'block' : 'none';
//                 const poster = item.querySelector('.banner-poster');
//                 const trailer = item.querySelector('.banner-trailer');
//                 if (poster && trailer) {
//                     poster.style.display = i === index ? 'block' : 'none';
//                     trailer.style.display = 'none';
//                 }
//             });
//             this.indicatorDots.forEach((dot, i) => {
//                 dot.classList.toggle('active', i === index);
//             });
//             this.currentSlide = index;
//         },

//         stopAllTrailers() {
//             this.items.forEach(item => {
//                 const video = item.querySelector('video');
//                 if (video) {
//                     video.pause();
//                     video.currentTime = 0;
//                 }
//             });
//         },

//         playTrailer(movieId, trailerPath) {
//             if (!trailerPath) {
//                 console.warn('No trailer path provided');
//                 return;
//             }
//             const item = document.querySelector(`.banner-item[data-movie-id="${movieId}"]`);
//             if (!item) return;

//             const poster = item.querySelector('.banner-poster');
//             const trailerDiv = item.querySelector(`#trailer-${movieId}`);
//             const video = trailerDiv.querySelector(`#video-${movieId}`);

//             if (poster && trailerDiv && video) {
//                 poster.style.display = 'none';
//                 trailerDiv.style.display = 'block';
//                 video.muted = true; // Ensure muted for autoplay
//                 video.play().catch(error => {
//                     console.error('Trailer autoplay failed:', error);
//                     poster.style.display = 'block';
//                     trailerDiv.style.display = 'none';
//                 });
//             }
//         },

//         stopTrailer(movieId) {
//             const item = document.querySelector(`.banner-item[data-movie-id="${movieId}"]`);
//             if (!item) return;

//             const poster = item.querySelector('.banner-poster');
//             const trailerDiv = item.querySelector(`#trailer-${movieId}`);
//             const video = trailerDiv.querySelector(`#video-${movieId}`);

//             if (poster && trailerDiv && video) {
//                 video.pause();
//                 video.currentTime = 0;
//                 trailerDiv.style.display = 'none';
//                 poster.style.display = 'block';
//             }
//         },

//         startAutoPlay() {
//             this.stopAutoPlay();
//             this.autoPlayTimer = setInterval(() => {
//                 const item = this.items[this.currentSlide];
//                 const trailerPath = item.dataset.trailerPath;
//                 const movieId = item.dataset.movieId;

//                 if (trailerPath && item.querySelector('.banner-poster').style.display !== 'none') {
//                     this.playTrailer(movieId, trailerPath);
//                     setTimeout(() => {
//                         this.nextSlide();
//                     }, this.autoPlayDelay + 3000);
//                 } else {
//                     this.nextSlide();
//                 }
//             }, this.autoPlayDelay);
//         },

//         stopAutoPlay() {
//             clearInterval(this.autoPlayTimer);
//         },

//         nextSlide() {
//             this.currentSlide = (this.currentSlide + 1) % this.items.length;
//             this.showSlide(this.currentSlide);
//         },

//         prevSlide() {
//             this.currentSlide = (this.currentSlide - 1 + this.items.length) % this.items.length;
//             this.showSlide(this.currentSlide);
//         },

//         bindEvents() {
//             this.nextBtn.addEventListener('click', () => {
//                 this.stopAutoPlay();
//                 this.nextSlide();
//                 this.startAutoPlay();
//             });

//             this.prevBtn.addEventListener('click', () => {
//                 this.stopAutoPlay();
//                 this.prevSlide();
//                 this.startAutoPlay();
//             });

//             this.indicatorDots.forEach((dot, index) => {
//                 dot.addEventListener('click', () => {
//                     this.stopAutoPlay();
//                     this.showSlide(index);
//                     this.startAutoPlay();
//                 });
//             });

//             document.querySelectorAll('.btn-play').forEach(btn => {
//                 btn.addEventListener('click', () => {
//                     const movieId = btn.dataset.movieId;
//                     const trailerPath = btn.dataset.trailerPath;
//                     this.stopAutoPlay();
//                     this.playTrailer(movieId, trailerPath);
//                 });
//             });

//             document.querySelectorAll('.btn-close-trailer').forEach(btn => {
//                 btn.addEventListener('click', () => {
//                     const movieId = btn.dataset.movieId;
//                     this.stopTrailer(movieId);
//                     this.startAutoPlay();
//                 });
//             });
//         }
//     };

//     carousel.init();
// });

        // Class MovieBanner giữ nguyên như file tĩnh của bạn
        class MovieBanner {
            constructor() {
                this.carousel = document.getElementById('bannerCarousel');
                if (!this.carousel) return; // Thoát nếu không có banner

                this.indicatorsContainer = document.getElementById('indicators');
                this.bannerItems = document.querySelectorAll('.banner-item');
                this.indicators = document.querySelectorAll('.indicator-dot');
                
                this.currentSlide = 0;
                this.isMuted = true;
                this.autoPlayTimer = null;
                this.trailerTimer = null;
                
                this.init();
            }

            init() {
                if (this.bannerItems.length <= 1) {
                    // Nếu chỉ có 1 hoặc không có banner, ẩn các nút điều khiển
                    const navs = document.querySelectorAll('.banner-nav, .banner-indicators');
                    navs.forEach(nav => nav.style.display = 'none');
                }
                this.setupEventListeners();
                this.startAutoPlay();
                this.startTrailerTimer();
            }

            setupEventListeners() {
                document.getElementById('prevBtn')?.addEventListener('click', () => this.previousSlide());
                document.getElementById('nextBtn')?.addEventListener('click', () => this.nextSlide());
                document.getElementById('volumeBtn')?.addEventListener('click', () => this.toggleVolume());
                
                this.indicators.forEach((indicator, index) => {
                    indicator.addEventListener('click', () => this.goToSlide(index));
                });

                this.carousel.addEventListener('mouseenter', () => this.pauseAutoPlay());
                this.carousel.addEventListener('mouseleave', () => this.resumeAutoPlay());
            }

            startTrailerTimer() {
                clearTimeout(this.trailerTimer); // Xóa timer cũ
                this.trailerTimer = setTimeout(() => {
                    this.showTrailer();
                }, 3000); // Hiện trailer sau 3 giây
            }

            showTrailer() {
                const currentItem = this.bannerItems[this.currentSlide];
                if (!currentItem) return;
                
                const trailerDiv = currentItem.querySelector('.banner-trailer');
                const video = trailerDiv.querySelector('video');
                
                if (video && video.querySelector('source')?.getAttribute('src')) {
                    trailerDiv.classList.add('active');
                    video.currentTime = 0;
                    video.play().catch(e => console.log('Autoplay bị trình duyệt chặn:', e));
                }
            }

            hideTrailer() {
                const currentItem = this.bannerItems[this.currentSlide];
                 if (!currentItem) return;

                const trailerDiv = currentItem.querySelector('.banner-trailer');
                const video = trailerDiv.querySelector('video');
                
                if (video) {
                    trailerDiv.classList.remove('active');
                    video.pause();
                }
            }

            goToSlide(index) {
                if (index === this.currentSlide && this.bannerItems.length > 1) return;
                
                this.hideTrailer();
                this.currentSlide = index;
                this.updateCarousel();
                this.updateIndicators();
                this.startTrailerTimer();
            }

            nextSlide() {
                const nextIndex = (this.currentSlide + 1) % this.bannerItems.length;
                this.goToSlide(nextIndex);
            }

            previousSlide() {
                const prevIndex = (this.currentSlide - 1 + this.bannerItems.length) % this.bannerItems.length;
                this.goToSlide(prevIndex);
            }

            updateCarousel() {
                const translateX = -this.currentSlide * 100;
                this.carousel.style.transform = `translateX(${translateX}%)`;
            }

            updateIndicators() {
                this.indicators.forEach((indicator, index) => {
                    indicator.classList.toggle('active', index === this.currentSlide);
                });
            }

            toggleVolume() {
                this.isMuted = !this.isMuted;
                const volumeBtn = document.getElementById('volumeBtn');
                const currentItem = this.bannerItems[this.currentSlide];
                if (!currentItem) return;

                const video = currentItem.querySelector('video');
                
                if (video) {
                    video.muted = this.isMuted;
                }
                
                volumeBtn.innerHTML = this.isMuted ? 
                    '<i class="fas fa-volume-xmark"></i>' : 
                    '<i class="fas fa-volume-high"></i>';
            }

            startAutoPlay() {
                clearInterval(this.autoPlayTimer);
                this.autoPlayTimer = setInterval(() => {
                    this.nextSlide();
                }, 8000); // Tự động chuyển slide sau 8 giây
            }

            pauseAutoPlay() {
                clearInterval(this.autoPlayTimer);
            }

            resumeAutoPlay() {
                this.startAutoPlay();
            }
        }

        // Khởi tạo banner khi DOM đã tải xong
        document.addEventListener('DOMContentLoaded', () => {
            new MovieBanner();
        });
    