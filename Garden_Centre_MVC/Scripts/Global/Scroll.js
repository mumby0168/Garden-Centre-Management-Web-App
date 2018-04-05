function ScrollToElementById(id) {
    $('html, body').animate({
        scrollTop: $("#" + id).offset().top
    }, 'slow');
}

function ScrollToTop() {
    $('html, body').animate({
        scrollTop: 0 }, 'slow');
}