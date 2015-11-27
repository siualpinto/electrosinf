$(document).ready(function() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/search",
        data: {},
        async: false
    }).done(function (artigos) {

    for(var i = 0; i < artigos[0].length; ++i){  
      active=""
      if (i==0) {active="active"};
      $("#carousel").append("<div class= \"item "+active+" \">"
                    +"<a href=\"produto.php?id="+artigos[1][i]['CodArtigo']+"\">"
                    +"<img src=\"../img/"+artigos[0][i]['CodArtigo']+"/1.jpg\" alt=\""+artigos[1][i]['DescArtigo']+"\" class=\"img-responsive fill\">"
                    +"<div class=\"carousel-caption\">"
                    + artigos[0][i]['DescArtigo']
                    +"</div>"
                    +"</a></div>");
    }

        for(var i = 0; i < artigos[1].length; ++i){  
        $('.top-list').append('<div class="top-item col-md-6"><a href="produto.php?id='+artigos[1][i]['CodArtigo']+'"><img src="../img/'+
                              artigos[1][i]['CodArtigo']+'/1.jpg" class="img-responsive" alt="'+
                              artigos[1][i]['DescArtigo']+'"></a><div class="artigo-dados text-left"><a href="produto.php?id='+
                              artigos[1][i]['CodArtigo']+'"><div class="descricao">'+
                              artigos[1][i]['DescArtigo']+'</div></a><div class="preco"><strong>'+
                              artigos[1][i]['Preco']+' </strong><span class="glyphicon glyphicon-euro" aria-hidden="true"></span></div></div>');
        }       
    });
});
