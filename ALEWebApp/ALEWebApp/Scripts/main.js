$(document).ready(function () {
    $(".propositionExample").click(function () {
        var proposition = $(this).text().replace(/\s/g, '');
        $("#tbxInputProposition").val(proposition);
    });

 
});

function GenerateRandomVector(url) {
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#tbxInputProposition").val(data);
        }
    });
}
