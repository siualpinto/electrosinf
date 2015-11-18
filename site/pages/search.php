<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>EletroSinf Produto</title>

    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet">

    <!-- Other Css -->
    <link href="../css/top-bar.css" rel="stylesheet">
    <link href="../css/carrinho.css" rel="stylesheet">

</head>

<body>
    <?php include_once 'header.php';?>
    
    
<?php
    echo '<input type="text" id="type" class="hidden"value="'.$_GET['tipo'].'">';
    echo '<input type="text" id="valor" class="hidden"value="'.$_GET['valor'].'">';?>
    
    <div class="container" id="displayitems">
        

        
    </div>



    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    <script src="../js/search.js"></script>
    <!-- Other JS -->
    <script src="../js/header.js"></script>


</body>

</html>