$(".product-spec-toggle").click(function(){
	var $toggler = $("#product-spec-text");
	var text = $($toggler).text();
    $($toggler).text(text == "Lista completa " ? "Comprimir lista" : "Lista completa ");
	$($toggler).next("span").toggleClass("glyphicon-plus");
	$($toggler).next("span").toggleClass("glyphicon-minus");
	
	$(".product-spec-hidden").toggle();
});