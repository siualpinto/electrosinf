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
    <div class="container">
        <h2>Registar Conta</h2>
        <form class="form-horizontal" role="form">
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Email:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="email" placeholder="Enter email">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="pwd">Password:</label>
                <div class="col-sm-10">
                    <input type="password" class="form-control" id="pwd" placeholder="Enter password">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Nome:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="nome" placeholder="Enter nome">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">NºContribuinte:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="numContribuinte" placeholder="Enter número contribuinte">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Moeda:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="moeda" placeholder="Enter Moeda">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Morada:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="morada" placeholder="Enter morada">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Localidade:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="localidade" placeholder="Enter Localidade">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Codigo Postal:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="CodPostal" placeholder="Enter CodPostal">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Distrito:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="distrito" placeholder="Enter Distrito">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">País:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="pais" placeholder="Enter País">
                </div>
            </div>         
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <button type="submit" class="btn btn-default">Registar</button>
                </div>
            </div>
        </form>
    </div>


</body>