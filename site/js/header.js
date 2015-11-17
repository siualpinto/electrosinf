$(document).ready(function() {
    console.log( "ready!" );
    
    function search(texto){
        
        //fazer redirect para pagina search com argumento texto
    }
   function display(){
       //inject code id="displayshit"
       // <button type="button" class="btn btn-default">Todas os Produtos</button>
       
   } 
    var res=$.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/TDU_Categoria",
        data: {},
        async: false
    }).done(function(data) {    
        for(var i = 0; i < data.length; ++i){
           $('#displayshit').append('<div class="dropdown inline"><button type="button" class="btn btn-default dropdown-toggle" id="Categoria_'+data[i]['CDU_IdCategoria']+'"data-toggle="dropdown">'+data[i]['CDU_Categoria']+'<span class="caret"></span></button><ul class="dropdown-menu" id="dropdown_categoria_'+data[i]['CDU_IdCategoria']+'" role="menu" aria-labelledby="Categoria_'+data[i]['CDU_IdCategoria']+'"></ul></div>');
           //cenascenascenas
           //<li role="presentation"><a role="menuitem" tabindex="-1" href="#">HTML</a></li>
           $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/TDU_Categoria/"+data[i]['CDU_IdCategoria'],
        data: {},
        async: false
    }).done(function(dat) {
                for(var a = 0; a < dat.length; ++a){
                    $('#dropdown_categoria_'+data[i]['CDU_IdCategoria']).append('<li role="presentation"><a role="menuitem" tabindex="-1" href="#">'+dat[a]['CDU_TipoArtigo']+'</a></li>');
                }
           })
    .fail(function(){
    alert( "Recarrege a pagina" );
  }); 
             
        
        }
    })
    .fail(function(){
    alert( "Recarrege a pagina" );
  });
    console.log(res);
});