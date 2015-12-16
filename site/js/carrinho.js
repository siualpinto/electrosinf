function getCarrinho(){
	$('#loading-indicator').show();
	var clienteID = $("#clienteID").val();
	var carrinho = $.ajax({
		method: "GET",
		url: "http://localhost:49234/api/TDU_Carrinho/"+clienteID
	}).done(function(data) {
		var total = 0;
		var produtos = $("#produtos");
		var podeGerarFatura = true
		$('#loading-indicator').hide();
		$.each(data.carrinho, function(index,element){
			total+=element.PrecoTotal;
			if(element.Stock < element.CDU_Quantidade || element.CDU_Quantidade<1){
				podeGerarFatura=false;
				red="border : 3px solid red";
			}else red="";
			console.log(element);
			produtos.append("<div class=\"shop-item col-md-12\">"
								+"<input type=\"hidden\" id=\"\" value=\""+element.CDU_IdArtigo+"\" >"
					            +"<div class=\"col-md-3 product-title\">"
					                +"<h4>"+element.Nome+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-12 product-info text-center\">"
					                +"<div class=\"col-md-3 shop-image\">"
					                    +"<img src=\"../img/"+element.CDU_IdArtigo+"/1.jpg\" class=\"img-responsive\" alt=\"Micro\">" /*FALTA A IMAGEM*/
					             	+"</div>"
					                +"<div class=\"col-md-3\">"
					                    +"<h4>"+element.Stock+" Produtos / "+element.CDU_Armazem+" </h4>" 
					                +"</div>"
					                +"<div class=\"col-md-3\">"
					                    +"<div class=\"col-md-5 col-md-offset-2\">"
					                        +"<input style=\""+red+"\" type=\"number\" min=\"1\" class=\"form-control quantidade\" value="+element.CDU_Quantidade+">"
					                    +"</div>"
					                +"</div>"

					                +"<div class=\"col-md-2\">"
					                    +"<h4>"+element.PrecoTotal.toFixed(2)+"</h4>"
					                +"</div>"
					                +"<div class=\"col-md-1\">"
					                    +"<span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" ></span>"
					                +"</div>"
					            +"</div>"
					        +"</div>");
			
		});

		produtos.append("<div class=\"shop-total col-md-12 text-right\">"
				            +"<h3> TOTAL:"+total.toFixed(2)+" € </h3>"
				        +"</div>");

		if(podeGerarFatura)
			produtos.append("<div class=\"shop-buttons col-md-12\">"
					            +"<div class=\"pull-right text-center col-md-3\">"
					                +"<button id=\"checkout\" type=\"submit\" class=\"btn btn-success text-center\">Checkout</button>"
					            +"</div>"
					        +"</div>");
		else produtos.append("<div class=\"shop-buttons col-md-12\">"
					            +"<div class=\"text-center col-md-3\">"
					                +"<button type=\"submit\" class=\"btn btn-warning text-center\">Não é possível satisfazer as quantidades indicadas as vermelho</button>"
					            +"</div>"
					        +"</div>");
	
		if(podeGerarFatura && total >0){
			$("#checkout").on("click",function gerarFatura(){
				$('#loading-indicator').show();
				var fatura = $.ajax({
					method: "POST",
					url: "http://localhost:49234/api/docvenda/",
					data: {Entidade:clienteID, DocType:"ECL"}
				}).done(function(result){
					$('#loading-indicator').hide();
					if (result == "Sucesso"){
						window.alert(result);
						window.location.href = "http://localhost:3000/electrosinf/site/pages/homepage.php";
					}else {
						window.alert("Ocorreu um erro");
						window.location.reload();
					}
				});
			});
		}

		$(".glyphicon-remove").on("click",function(){
			var id = $(this).parent().parent().parent().children("input").val();
			$.ajax({
				method: "DELETE",
				url: "http://localhost:49234/api/TDU_Carrinho/",
				data: {CDU_IdCliente:clienteID,CDU_IdArtigo:id}
			}).done(function(data){
				window.location.reload();
			});
		});
		$(".quantidade").change(function(){
			var value = this.value;
			var id = $(this).parent().parent().parent().parent().children("input").val();
			$.ajax({
				method: "PUT",
				url: "http://localhost:49234/api/TDU_Carrinho/",
				data: {CDU_IdCliente:clienteID,CDU_IdArtigo:id,CDU_Quantidade:value}
			}).done(function(data){
				window.location.reload();
			});
		});

		var dados= "<h4>Dados para entrega/fatura</h4><div>";
		dados+="<p><strong> Contribuinte: </strong>"+data.NumContribuinte+"</p>";
		dados+="<p><strong> Morada: </strong>"+data.Morada+"</p>";
		dados+="<p><strong> Localidade: </strong>"+data.Localidade+"</p>";
		dados+="<p><strong> Código Postal: </strong>"+data.CodPostal+" "+data.LocalidadeCodPostal+"</p>";
		dados+="<p><strong> Distrito: </strong>"+data.Distrito+"</p>";
		dados+="<p><strong> Contribuinte: </strong>"+data.NumContribuinte+"</p>";
		dados+="<p><strong> Telefone: </strong>"+data.NumTelefone+"</p>";
		dados += "</div>"
		produtos.parent().append(dados);
	});
}
function start(){
	getCarrinho();
}

$(start);