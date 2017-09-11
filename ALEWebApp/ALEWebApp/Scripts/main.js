$(document).ready(function () {
    $(".propositionExample").click(function () {
        var proposition = $(this).text().replace(/\s/g, '');
        $("#tbxInputProposition").val(proposition);
    });

    $("#btnRandomProp").click(function () {
        alert("Not yet implemented");
        //$.ajax({
        //    type: "POST",
        //    url: "Parsing/GenerateRandomProposition",
        //    contentType: "application/json; charset=utf-8",
        //    success: function(data) {
        //        alert("Not yet implemented");
        //    }
        //});
    });
});

