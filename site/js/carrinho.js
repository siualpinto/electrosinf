function getCarrinho(){
	var clienteID = $("#clienteID").val();
	var carrinho = $.ajax({
		method: "GET",
		url: "http://localhost:49234/api/TDU_Carrinho/"+clienteID
	}).done(function(data) {
		var total = 0;
		var produtos = $("#produtos");
		var podeGerarFatura = true

		$.each(data, function(index,element){
			total+=element.PrecoTotal;
			if(element.Stock < element.CDU_Quantidade){
				podeGerarFatura=false;
				red="border : 3px solid red";
			}else red="";
			
			produtos.append("<div class=\"shop-item col-md-12\">"
					            +"<div class=\"product-title\">"
					                +"<h4>"+element.Nome+"</h4>"
					            +"</div>"
					            +"<div class=\"col-md-12 product-info text-center\">"
					                +"<div class=\"col-md-3 shop-image\">"
					                    +"<img src=\"../img/maquinalavar.png\" class=\"img-responsive\" alt=\"Micro\">" /*FALTA A IMAGEM*/
					             	+"</div>"
					                +"<div class=\"col-md-3\">"
					                    +"<h4>"+element.Stock+" Produtos</h4>" /*FALTA DISPONIBILIDADE NO WS DO CARRINHO*/
					                +"</div>"
					                +"<div class=\"col-md-3\">"
					                    +"<div class=\"col-md-4 col-md-offset-4\">"
					                        +"<input style=\""+red+"\" type=\"number\" class=\"form-control\" placeholder=\"2\" value="+element.CDU_Quantidade+">"
					                    +"</div>"
					                +"</div>"

					                +"<div class=\"col-md-3\">"
					                    +"<h4>"+element.PrecoTotal.toFixed(2)+"</h4>"
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
				window.alert("start fatura");
				var fatura = $.ajax({
					method: "POST",
					url: "http://localhost:49234/api/docvenda/",
					data: {Entidade:clienteID, DocType:"FA"}
				}).done(function(result){
					if(result == "quantidadeErrada"){
						window.alert("Ocorreu um erro");
						window.location.reload();
					}else if (result == "Sucesso"){
						window.alert(result);
						window.location.href = "http://localhost:3000/electrosinf/site/pages/homepage.php";
					}
				});
			});
		}
	});
}

function start(){
	getCarrinho();
}

$(start);