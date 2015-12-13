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
				artigos+="<li>"+artigo.DescArtigo+"</li>";
			});
			artigos +="</ul>"
			dateLiq = element.DataLiq.split("T")[0];
			if (dateLiq=="0001-01-01")
				dateLiq = element.estado;	
			faturas.append("<div class=\"shop-item col-md-12\">"
								+"<div class=\"col-md-2\">"
					                +"<h4>"+element.id.replace("{","").replace("}","")+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-2\">"
					                +"<h4>"+element.Data.split("T")[0]+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-2\">"
					                +"<h4>"+element.estado+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-2\">"
					                +"<h4>"+dateLiq+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-4\">"
					                +artigos
					            +"</div>"

					        +"</div>");
			
		});


Data: "2015-11-26T00:00:00"
DataLiq: "2015-12-04T00:00:00"
DocType: null
Entidade: null
LinhasDoc: null
NumDoc: 0
Serie: null
TotalMerc: 0
estado: "Pronto"
id: "{AE77033E-9480-11E5-BC47-0800278ABB53}"

	});
}

$(start);