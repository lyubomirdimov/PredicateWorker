$(document).ready(function () {
    $(".propositionExample").click(function () {
        var proposition = $(this).text().replace(/\s/g, '');
        $("#tbxInputProposition").val(proposition);
    });    
});

