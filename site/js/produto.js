$(document).ready(function() {
    console.log("ready!");
    var id=$('#artigo_id').val();
    var produto = $.ajax({
        type: "GET",
        url:"http://localhost:49234/api/artigos/" + id,
        dataType: "json",
        data: {},
        async: false
    }).done(function(data) {
                    console.log(data);
                    $('#product-title').append('<h3>'+data['DescArtigo']+'</h3>'); 
                    $('#price').append('<strong>Preço: '+data['Preco']+'€ </strong>');
                    $('#brand').append('<strong>Marca: '+data['Marca']+'</strong>');
                    $('#model').append('<strong>Modelo: '+data['Modelo']+'</strong>');
                    if(data['Stock'] > 1){
                        $('#availability').append('<strong>Disponibilidade: '+data['Stock']+' unidades </strong>');
                    } else if(data['Stock'] == 1){
                        $('#availability').append('<strong>Disponibilidade: '+data['Stock']+' unidade </strong>');
                    } else {
                        $('#availability').append('<strong>Disponibilidade: Indisponível </strong>');
                    }
        
                    for(var i=0; i < data['Especificacoes'].length; ++i){
                        if(i<5){
                        $('#specifications').append(
                            '<tr><td><strong>'+data['Especificacoes'][i]['CDU_Nome']+'</strong></td><td>'+data['Especificacoes'][i]["CDU_Valor"]+ '</td></tr>')
                        } else {
                        $('#specifications').append(
                            '<tr class="product-spec-hidden"><td><strong>'+data['Especificacoes'][i]['CDU_Nome']+'</strong></td><td>'+data['Especificacoes'][i]["CDU_Valor"]+ '</td></tr>')
                        }
                    }
                    
                    $(".product-spec-toggle").click(function(){
                        var $toggler = $("#product-spec-text");
                        var text = $($toggler).text();
                        $($toggler).text(text == "Lista completa " ? "Comprimir lista" : "Lista completa ");
                        $($toggler).next("span").toggleClass("glyphicon-plus");
                        $($toggler).next("span").toggleClass("glyphicon-minus");
                        $(".product-spec-hidden").toggle();
                    });
        
                for(var i=0; i < data['Relacionados'].length; ++i){
                        if(i<4){
                        $('#related-products').append(
                            '<div class="col-md-3 product-related-mini"><div><a href="produto.php?id='+data['Relacionados'][i]['CodArtigo']+'"><img src="../img/ml1.png" class="img-responsive" alt="Maquina Lavar AEG"></div><div class="product-related-title"><small><strong>'+data['Relacionados'][i]['DescArtigo']+'</strong></small></div></a><div class="product-related-price">'+data['Relacionados'][i]['Preco']+'€</div></div>');
                        }
                }
            })
            .fail(function(){
                alert( "Recarrege a pagina" );
            });
    
});

