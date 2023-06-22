// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(document).ready(function () {
//    $('#expanded-menu-button').click(function () {
//        $('.expanded-menu-button').toggleClass('close');
//    });
//});

$(document).ready(function () {
    var menuOpen = false;
    $('#expanded-menu-button').click(function () {
        if (menuOpen) {
            $('.hamburger-menu').css('right', '-400px');
        } else {
            $('.hamburger-menu').css('right', '0');
        }
        menuOpen = !menuOpen;
    });
});

$(document).ready(function () {
    $('.carousel-item:first').addClass('active'); // Set the first item as active initially

    // Automatically switch to the next item in the carousel every 5 seconds
    setInterval(function () {
        var $activeItem = $('.carousel-item.active');
        var $nextItem = $activeItem.next('.carousel-item');

        if ($nextItem.length === 0) {
            $nextItem = $('.carousel-item:first');
        }

        $activeItem.removeClass('active');
        $nextItem.addClass('active');
    }, 5000);
});