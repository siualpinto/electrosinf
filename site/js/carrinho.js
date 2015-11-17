function getCarrinho(){
	var carrinho = $.ajax({
		type: "GET",
		url: "http://localhost:49234/api/artigos",
		
		crossDomain: true,
	}).done(function(data) {
		window.alert(data);
		console.log(data);
	});
	console.log("fim");
}

$(getCarrinho);