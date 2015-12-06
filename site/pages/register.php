<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
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
        <form class="form-horizontal" role="form" id="registar">
            <div class="form-group">
                <label class="control-label col-sm-2" for="email">Email:</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" name="email" id="email" placeholder="Enter email">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="pwd">Password:</label>
                <div class="col-sm-10">
                    <input type="password" class="form-control" id="pwd" placeholder="Enter Password. Min length: 5" required pattern="(?=.*[\d.a-z.A-Z]).{5,256}" title="Deve ter 5 ou mais caracteres">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="nome">Nome:</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" name="nome" id="nome" placeholder="Enter nome">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="numContribuinte">NºContribuinte:</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" name="numContribuinte" required pattern="(^\d{9}$)" title="Deve conter 9 numeros" id="numContribuinte" placeholder="Enter número contribuinte">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="moeda">Moeda:</label>
                <div class="col-xs-2">
                    <select class="form-control" name="moeda">
                        <option id="moeda" name="moeda" value="EUR">Euro</option>
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="morada">Morada:</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" name="morada" id="morada" placeholder="Enter morada">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="localidade">Localidade:</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" name="localidade" id="localidade" placeholder="Enter Localidade">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" data-fv-zipcode="true" for="CodPostal">Codigo Postal:</label>
                <div class="col-sm-10">
                    <input type="text" id="CodPostal" name="codigoPostal" required pattern="(^\d{4}-\d{3}$)" title="exemplo(4563-132)" class="form-control" placeholder="Enter CodPostal">
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="distrito">Distrito:</label>
                <div class="col-xs-2">
                    <select class="form-control" id="distrito" name="distrito">
                        <option value="01">Aveiro</option>
                        <option value="02">Beja</option>
                        <option value="03">Braga</option>
                        <option value="04">Bragança</option>
                        <option value="05">Castelo Branco</option>
                        <option value="06">Coimbra</option>
                        <option value="07">Évora</option>
                        <option value="08">Faro</option>
                        <option value="09">Guarda</option>
                        <option value="10">Leiria</option>
                        <option value="11">Lisboa</option>
                        <option value="12">Portalegre</option>
                        <option value="13">Porto</option>
                        <option value="14">Santarém</option>
                        <option value="15">Setúbal</option>
                        <option value="16">Viana do Castelo</option>
                        <option value="17">Vila Real</option>
                        <option value="18">Viseu</option>
                        <option value="31">Ilha da Madeira</option>
                        <option value="32">Ilha de Porto Santo</option>
                        <option value="41">Ilha de Santa Maria</option>
                        <option value="42">Ilha de São Miguel</option>
                        <option value="43">Ilha Terceira</option>
                        <option value="44">Ilha da Graciosa</option>
                        <option value="45">Ilha de São Jorge</option>
                        <option value="46">Ilha do Pico</option>
                        <option value="47">Ilha do Faial</option>
                        <option value="48">Ilha das Flores</option>
                        <option value="49">Ilha do Corvo</option>
                    </select>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="pais">País:</label>
                <div class="col-xs-2">
                    <select class="form-control" id="pais" name="pais">
                        <option value="PT">Portugal</option>
                    </select>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <button id="registarbtn" type="submit" class="btn btn-default">Registar</button>
                </div>
            </div>
        </form>
    </div>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.min.js"></script>
    <!-- Other JS -->
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.11.1/jquery.validate.min.js"></script>
    <script src="../js/register.js"></script>
    <script src="../js/header.js"></script>
</body>