$(document).ready(function() {
    console.log("ready!");
      $("#login").on("click",function(){
        var userName = $("#email").val();
        var pass = $("#password").val();
        $.ajax({
          type: "PUT",
          dataType: "json",
          url: "http://localhost:49234/api/clientes",
          data: {Email:userName,Password:pass}
        }).done(function (data) {
          if(data.Erro==-1){
            alert("Password errada");
          }else if(data.Erro==0){
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "http://localhost:3000/electrosinf/site/actions/login.php",
                data: {_cliente:data.Descricao,token:"aytidughasdpusagdusagdisadytadulhbweo8li721y837y2fyus"}
            });
              window.location.reload();      
          }
        });
    });
    $.ajax({
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
                    $('#dropdown_categoria_'+data[i]['CDU_IdCategoria']).append('<li role="presentation"><a role="menuitem" tabindex="-1" href="search.php?tipo=1&valor='+dat[a]['CDU_IdTipo']+'">'+dat[a]['CDU_TipoArtigo']+'</a></li>');
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

     $("#search_button").on("click",function() {
         window.location = "search.php?tipo=0&valor="+$('#search').val();
     });
 
});
 