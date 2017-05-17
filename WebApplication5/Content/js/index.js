$('.accordion-tab').click(function() {
    var show = !$(this).next().is(':visible');    
    
    //$(this).siblings().removeClass('minus-icon');
    //$(this).toggleClass('minus-icon', show);
    
    $('.play-listado-plays').slideUp('normal');
    
    if(show)
        $(this).next().slideDown('normal');
});