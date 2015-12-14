<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>EletroSinf Carrinho</title>

    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet">

    <!-- Other Css -->
    <link href="../css/top-bar.css" rel="stylesheet">
    <link href="../css/carrinho.css" rel="stylesheet">

</head>

<body>
   <?php include_once 'header.php';?>
    <input type="hidden" id="clienteID" value=<?=$_SESSION['clienteID']?> style= "display: none" >
    <div id="faturas" class="container">
        <div class="shop-label col-md-12">
            <div class="col-md-3">
                <h4>Referência</h4>
            </div>
            <div class="col-md-3">
                <h4>Data da encomenda</h4>
            </div>

            <div class="col-md-2">
                <h4>Estado</h4>
            </div>

            <div class="col-md-4">
                <h4>Artigo [Quantidade]</h4>
            </div>
        </div>
    </div>
   <img src="../img/loader/ajax-loader.gif" id="loading-indicator" style="display:none" />


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    <!-- Other JS -->
    <script src="../js/header.js"></script>
    <script src="../js/encomenda.js"></script>

</body>

</html>