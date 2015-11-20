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
    <link href="../css/produto.css" rel="stylesheet">
    
</head>
    

<body>
   <?php include_once 'header.php';?>
<?php echo '<input type="text" id="artigo_id" class="hidden"value="'.$_GET['id'].'">';?>
    <div class="container">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script src="../js/produto.js"></script>
        <div class="col-md-6">
            <div id="img_artigo" class="col-md-8 col-md-offset-2">
            </div>

            <div class="col-md-12">
                <div class="col-md-2 product-mini">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Maquina Lavar AEG">
                </div>
                <div class="col-md-2 product-mini">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Maquina Lavar AEG">
                </div>
                <div class="col-md-2 product-mini">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Maquina Lavar AEG">
                </div>
                <div class="col-md-2 product-mini">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Maquina Lavar AEG">
                </div>
                <div class="col-md-2 product-mini">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Maquina Lavar AEG">
                </div>
            </div>

        </div>

        <div class="col-md-6">
            <div id="product-title" class="product-title">
            
            </div>

            <div class="product-info-container">
                <div id="price" class="product-info-item">
                </div>
                <div id="model" class="product-info-item"> 
                </div>
                <div id="brand" class="product-info-item">
                </div>
                <div id="availability" class="product-info-item">
                </div>

                <div class="product-info-item">
                    <button id="adicionar_carrinho" onclick="add_carrinho()" type="button" class="btn btn-success">Adicionar ao carrinho</button>
                </div>

            </div>


        </div>

        <div class="col-md-8 product-specs">
            <div>
                <h4>Mais Informações </h4>
            </div>

            <div>
                <p>
                </p>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <table class="table">

                        <tbody id="specifications">
                            <tr class="product-spec-toggle">
                                <td class="col-md-3"><strong>Especificações</strong></td>
                                <td class="text-right"><a id="product-spec-text">Lista completa </a><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></td>
                            </tr>
                            
                        </tbody>

                    </table>


                </div>

            </div>
        </div>

        <div class="col-md-8 product-related">
            <div>
                <h4>Relacionados </h4>
            </div>
            <div id="related-products" class="col-md-12 product-related-info">

            </div>
        </div>

    </div>



    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    
    <!-- Other JS -->
    <script src="../js/header.js"></script>


</body>

</html>