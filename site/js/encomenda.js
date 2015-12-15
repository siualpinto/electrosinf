function start () {
	$('#loading-indicator').show();
	var clienteID = $("#clienteID").val();
	var carrinho = $.ajax({
		method: "GET",
		url: "http://localhost:49234/api/docvenda/"+clienteID
	}).done(function(data) {
		console.log(data);

		var faturas = $("#faturas");
		
		 $('#loading-indicator').hide();
		$.each(data, function(index,element){
			artigos = "<ul>";
			$.each(element.LinhasDoc, function(k,artigo){
				if(artigo.DescArtigo!="" && artigo.DescArtigo.substring(0,6)!="ECL Nº")
					artigos+="<li>"+artigo.DescArtigo+" ["+artigo.Quantidade+"]</li>";
			});
			artigos +="</ul>"
			dateLiq = element.DataLiq.split("T")[0];
			//if (dateLiq=="0001-01-01")
			//	dateLiq = element.estado;	
			faturas.append("<div class=\"shop-item col-md-12\">"
								+"<div class=\"col-md-3\">"
					                +"<h4>"+element.id.replace("{","").replace("}","")+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-3\">"
					                +"<h4>"+element.Data.split("T")[0]+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-2\">"
					                +"<h4>"+element.estado+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-4\">"
					                +artigos
					            +"</div>"

					        +"</div>");
			
		});
	});
}
$(start);