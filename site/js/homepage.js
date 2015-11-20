$(document).ready(function() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/search",
        data: {},
        async: false
    }).done(function (artigos) {        
        for(var i = 0; i < artigos.length; ++i){  
        $('.top-list').append('<div class="novidades-item col-md-4"><img src="../img/'+artigos[i]['CodArtigo']+'/1.jpg" class="img-responsive" alt="'+artigos[i]['Descrição']+'"><p>'+artigos[i]['DescArtigo']+'</p></div>');
        }       
    });
});
