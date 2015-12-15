 $('#registarbtn').click(function() {
     var $myForm = $('#registar');
     if ($myForm[0].checkValidity()) {         
         $('#registarbtn').disabled = true;
         var email = $('#registar #email').val();
         var pwd = $('#registar #pwd').val();
         var nome = $('#registar #nome').val();
         var numContribuinte = $('#registar #numContribuinte').val();
         var moeda = $('#registar #moeda').val();
         var morada = $('#registar #morada').val();
         var localidade = $('#registar #localidade').val();
         var localidadeCodPostal = $('#registar #localidadeCodPostal').val();
         var numTelefone = $('#registar #numTelefone').val();
         var codPostal = $('#registar #codPostal').val();
         var distrito = $('#registar #distrito').val();
         var pais = $('#registar #pais').val();
         var pagamento = $('#registar #pagamento').val();
         var condicao = $('#registar #condicao').val();
         $.ajax({
             method: "POST",
             url: "http://localhost:49234/api/clientes",
             data: {
                 Email: email,
                 Password: pwd,
                 NomeCliente: nome,
                 NumContribuinte: numContribuinte,
                 Moeda: moeda,
                 Morada: morada,
                 Localidade: localidade,
                 LocalidadeCodPostal: localidadeCodPostal,
                 NumTelefone: numTelefone,
                 CodPostal: codPostal,
                 Distrito: distrito,
                 Pais: pais,
                 ModoPagamento: pagamento,
                 CondicaoPagamento: condicao
             },
             async: false
         }).done(function(data) {           
            if(data=="0")
                 { 
                 alert("Conta criada com sucesso!!!");
                      $myForm.submit(function(e) {
                     e.preventDefault();
                 });
				 window.location.href = "http://localhost:3000/electrosinf/site/pages/homepage.php";
                     return;
                 }
             else if (data=="-1")
                {
                 alert("Já existe uma pessoa com esse email!!!");
                 $myForm.submit(function(e) {
                     e.preventDefault();
                 });
                 return;
                }
             else
                 {
                 alert("Algum erro ocurreu na base de dados");
                 window.location.href("http://localhost:3000/electrosinf/site/pages/homepage.php");
                 }
         });
     }
 });