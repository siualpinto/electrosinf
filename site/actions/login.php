<?php
	if(isset($_POST['token'])){
		if($_POST['token'] === "aytidughasdpusagdusagdisadytadulhbweo8li721y837y2fyus"){
			if(isset($_POST['_cliente'])){ 
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