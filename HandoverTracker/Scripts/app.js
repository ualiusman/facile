$('.side-nav li:has("ul")').children('ul').hide(); //hide submenu
$('.side-nav li:has("ul")').addClass('hasChildren'); // add class to li ul child
$('.side-nav li:has("ul")').click(function () {
    $(this).toggleClass("active") // add active class to clicked menu item
    $(this).children('ul').slideToggle(); //toggle submenu
});
