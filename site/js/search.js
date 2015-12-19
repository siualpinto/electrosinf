$(document).ready(function() {
    //console.log("ready!");
    var tipo=$('#type').val();
    var valor=$('#valor').val();
    var url="";
    if(tipo==1){
        url="http://localhost:49234/api/TDU_TipoArtigo/"+valor;
    }
    else{
        url = "http://localhost:49234/api/search?search=\""+valor+"\"";
    }
    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: {},
        async: false
    }).done(function(data) {
                for(var i = 0; i < data.length; ++i){
                    $('#displayitems').append('<a href="produto.php?id='+data[i]['CodArtigo']+'"><div class="shop-item col-md-12"><div class="product-title"><h4>'+data[i]['DescArtigo']+'</h4></div><div class="col-md-12 product-info text-center"><div class="col-md-3 shop-image"> <img src="../img/'+data[i]['CodArtigo']+'/1.jpg" class="img-responsive" alt="'+data[i]['DescArtigo']+'"> </div><div class="col-md-3"><h4>Disponivel: '+data[i]['Stock']+'</h4></div><div class="col-md-3"><h4>Preco: '+data[i]['Preco']+'€</h4></div></div></div></a>');
                    
                }
            })
            .fail(function(){
                alert( "Recarrege a pagina" );
            });
    
});
 
