<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>EletroSinf Homepage</title>

    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet">

    <!-- Other Css -->
    <link href="../css/top-bar.css" rel="stylesheet">
    <link href="../css/homepage.css" rel="stylesheet">
</head>

<body>

    <?php include_once 'header.php';?>

    <div class="carousel-container text-center">
        <div id="carousel-example-generic" class="carousel slide" data-ride="carousel">
            <!-- Indicators -->
            <ol class="carousel-indicators">
                <li data-target="#carousel-example-generic" data-slide-to="0" class="active"></li>
                <li data-target="#carousel-example-generic" data-slide-to="1"></li>
                <li data-target="#carousel-example-generic" data-slide-to="2"></li>
            </ol>

            <!-- Wrapper for slides -->
            <div id="carousel" class="carousel-inner" role="listbox">
				<div class="item active">
				  <img src="../img/banners/123.png" alt="...">
				  <div class="carousel-caption">
				  </div>
				</div>
				<div class="item">
				  <img src="../img/banners/234.png" alt="...">
				  <div class="carousel-caption">
				  </div>
				</div>
            </div>

            <!-- Controls -->
            <a class="left carousel-control" href="#carousel-example-generic" role="button" data-slide="prev">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                <span class="sr-only">Previous</span>
            </a>
            <a class="right carousel-control" href="#carousel-example-generic" role="button" data-slide="next">
                <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                <span class="sr-only">Next</span>
            </a>
        </div>
    </div>

    <div class="top-container">
        <div class="top-title">
            <h3>Produtos mais vendidos</h3>
        </div>
        <div class="top-list col-md-12">
        </div>
    </div>

    <div class="texto-container col-md-12 text-justify">
        <div class="texto-title">
            <h3>Dê à sua esposa as ferramentas para o fazer feliz</h3>
        </div>
        <div class="texto">
            <p>Conheça a nossa gama para Arrumação e organização de alimentos , vai encontrar opções que garantem que os alimentos não percam o sabor. Os nossos recipientes mantêm tudo arrumado e fácil de encontrar, para que se possa concentrar nos cozinhados. Com tampas transparentes, ou com as laterais em vidro, permitem que saiba num piscar de olhos se precisa de reabastecer. Transforme as sobras de hoje no petisco de amanhã. Coloque no frigorífico , permite-lhe desperdiçar menos comida (e dinheiro).</p>
        </div>
    </div>

    </div>


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    <!-- Other JS -->
    <script src="../js/header.js"></script>
    <script src="../js/homepage.js"></script>



</body>

</html>