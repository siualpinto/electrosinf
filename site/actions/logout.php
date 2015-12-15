<?
	session_start();
	session_destroy(); 
	header("Location: http://localhost:3000/electrosinf/site/pages/homepage.php");
	die();
?>