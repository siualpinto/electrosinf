$(".product-spec-toggle").click(function(){
	var $toggler = $("#product-spec-text");
	var text = $($toggler).text();
    $($toggler).text(text == "Lista completa " ? "Comprimir lista" : "Lista completa ");
	$($toggler).next("span").toggleClass("glyphicon-plus");
	$($toggler).next("span").toggleClass("glyphicon-minus");
	
	$(".product-spec-hidden").toggle();
});

function produto(){
	var produto = $.ajax({
		type: "GET",
		url: "http://localhost:49234/api/artigos/A001",
		async: true,
		crossDomain: true,
	}).done(function(data) {
		window.alert(data);
		console.log(data);
	});
	console.log("fim");
}

window.onpaint= $(produto);