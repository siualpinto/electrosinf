function homepage() {
    $.ajax({
        type: "GET",
        dataType: "html",
        url: "http://localhost:49234/api/search",
        data: {},
        async: false
    }).done(function (games) {});
}