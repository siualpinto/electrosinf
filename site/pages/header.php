<?
    session_set_cookie_params(0, '/', 'http://localhost:3000/'); 
    session_start();  
    $_SESSION['clienteID']='C001';
?>
<nav class="navbar navbar-default">
        <div class="container">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <a class="navbar-brand" href="homepage.php">EletroSinf</a>
            </div>

            <!-- Collect the nav links, forms,  and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <div class="navbar-form navbar-left">
                    <div class="form-group">
                        <input type="text" class="form-control" id="search" placeholder="Search">
                    </div>
                    <button type="button" class="btn btn-default btn-sm" id="search_button">Procurar</button>
                </div>
                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <a href="http://localhost:3000/electrosinf/site/pages/register.php"> <span class="glyphicon glyphicon-share" aria-hidden="true"></span> </a>
                    </li>
                    <li>
                        <a href="carrinho.php"> <span class="glyphicon glyphicon-shopping-cart" aria-hidden="true"></span> </a>
                    </li>
                    <li>
                        <a href="#"> <span class="glyphicon glyphicon-question-sign" aria-hidden="true"></span> </a>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>

 <div class="container">

        <div class="text-center">
            <div class="btn-group" id="displayshit" role="group" aria-label="...">

            </div>
        </div>