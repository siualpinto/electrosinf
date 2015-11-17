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
    <div class="container">

        <div class="shop-label col-md-12 text-center">
            <div class="col-md-3 shop-image">
                <h4>Produto</h4>
            </div>

            <div class="col-md-3">
                <h4>Disponibilidade</h4>
            </div>

            <div class="col-md-3">
                <h4>Quantidade</h4>
            </div>

            <div class="col-md-3">
                <h4>Preço</h4>
            </div>
        </div>

        <div class="shop-item col-md-12">

            <div class="product-title">
                <h4> Máquina Lavar Roupa AEG L 68280 FL </h4>
            </div>

            <div class="col-md-12 product-info text-center">
                <div class="col-md-3 shop-image">
                    <img src="../img/maquinalavar.png" class="img-responsive" alt="Micro">
                </div>

                <div class="col-md-3">
                    <h4>13 Produtos</h4>
                </div>

                <div class="col-md-3">

                    <div class="col-md-4 col-md-offset-4">
                        <input type="number" class="form-control" placeholder="2" value="2">
                    </div>

                </div>

                <div class="col-md-3">
                    <h4>€599,99</h4>
                </div>
            </div>
        </div>

        <div class="shop-item col-md-12">

            <div class="product-title">
                <h4> Máquina Lavar Roupa AEG L 68280 FL </h4>
            </div>

            <div class="col-md-12 product-info text-center">
                <div class="col-md-3 shop-image">
                    <img src="../img/micro.png" class="img-responsive" alt="Micro">
                </div>

                <div class="col-md-3">
                    <h4>13 Produtos</h4>
                </div>

                <div class="col-md-3">
                    <div class="col-md-4 col-md-offset-4">
                        <input type="number" class="form-control" placeholder="2" value="2">
                    </div>
                </div>

                <div class="col-md-3">
                    <h4>600$</h4>
                </div>
            </div>
        </div>

        <div class="shop-item col-md-12">

            <div class="product-title">
                <h4> Máquina Lavar Roupa AEG L 68280 FL </h4>
            </div>

            <div class="col-md-12 product-info text-center">
                <div class="col-md-3 shop-image">
                    <img src="../img/tv.png" class="img-responsive" alt="Micro">
                </div>

                <div class="col-md-3">
                    <h4>13 Produtos</h4>
                </div>

                <div class="col-md-3">
                    <div class="col-md-4 col-md-offset-4">
                        <input type="number" class="form-control" placeholder="2" value="2">
                    </div>
                </div>

                <div class="col-md-3">
                    <h4>600$</h4>
                </div>
            </div>
        </div>
        <div class="shop-total col-md-12 text-right">
            <h3> TOTAL: € </h3>
        </div>

        <div class="shop-buttons col-md-12">
            <div class="pull-right text-center col-md-3">
                <button type="submit" class="btn btn-success text-center">Checkout</button>
            </div>
        </div>

    </div>



    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    <!-- Other JS -->
    <script src="../js/header.js"></script>
    <script src="../js/carrinho.js"></script>

</body>

</html>