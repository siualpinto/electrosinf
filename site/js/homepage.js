$(document).ready(function() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/search",
        data: {},
        async: false
    }).done(function (artigos) {        
        for(var i = 0; i < artigos.length; ++i){  
        $('.top-list').append('<div class="top-item col-md-6"><a href="produto.php?id='+artigos[i]['CodArtigo']+'"><img src="../img/'+
                              artigos[i]['CodArtigo']+'/1.jpg" class="img-responsive" alt="'+
                              artigos[i]['DescArtigo']+'"></a><div class="artigo-dados text-left"><a href="produto.php?id='+
                              artigos[i]['CodArtigo']+'"><div class="descricao">'+
                              artigos[i]['DescArtigo']+'</div></a><div class="preco"><strong>'+
                              artigos[i]['Preco']+' </strong><span class="glyphicon glyphicon-euro" aria-hidden="true"></span></div></div>');
        }       
    });
});
