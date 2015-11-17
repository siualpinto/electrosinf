$(document).ready(function() {
    console.log( "ready!" );
    
    function search(texto){
        
        //fazer redirect para pagina search com argumento texto
    }
   function display(){
       //inject code id="displayshit"
       // <button type="button" class="btn btn-default">Todas os Produtos</button>
       
   } 
    alert("oooo");
     var res=$.ajax({url: "http://localhost:49234/api/TDU_TipoArtigo/1", 
                     
             type: "get",
             dataType: "jsonp"
        }).done(function(data){alert(data)});
    alert(JSON.stringify(res));
    alert('aa');
});