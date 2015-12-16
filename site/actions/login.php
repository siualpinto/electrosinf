<?php
	header('Content-Type: application/json');
	if(isset($_POST['token'])){
		if($_POST['token'] === "aytidughasdpusagdusagdisadytadulhbweo8li721y837y2fyus"){
			if(isset($_POST['_cliente'])){ 
				session_set_cookie_params(0); 
				session_start(); 
    			$_SESSION['clienteID']=$_POST['_cliente'];
    			echo json_encode("login");
			}else {
				echo json_encode("no cliente");
			}
		}else {
			echo json_encode("fail token");
		}
	}else {
		echo json_encode("no token");
	}
	echo json_encode("FIM");
?>