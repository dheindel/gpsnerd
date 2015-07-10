// Header animations
function animateTruck1() {
    $("#truck1").animate({ 'left': '-100px' }, 50000, 'linear', function () {
        $(this).css('left', '3000px');
        animateTruck1();
    });
}
function animateTruck2() {
    $("#truck2").animate({ 'left': '3000px' }, 55000, 'linear', function () {
        $(this).css('left', '-100px');
        animateTruck2();
    });
}

animateTruck1();
animateTruck2();