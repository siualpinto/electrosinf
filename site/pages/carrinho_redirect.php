<?

	$cliente = $_SESSION['clienteID'];
	$doc = $_POST['DocType'];


	$response = http_post("http://localhost:49234/api/docvenda/", array("Entidade"=>$cliente, "DocType"=>$doc);
	
	return $response;

?>