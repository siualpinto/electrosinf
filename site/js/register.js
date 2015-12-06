 $('#registarbtn').click(function() {
     var email = $('#registar #email').val();
     var pwd = $('#registar #pwd').val();
     var nome = $('#registar #nome').val();
     var numContribuinte = $('#registar #numContribuinte').val();
     var moeda = $('#registar #moeda').val();
     var morada = $('#registar #morada').val();
     var localidade = $('#registar #localidade').val();
     var CodPostal = $('#registar #CodPostal').val();
     var distrito = $('#registar #distrito').val();
     var pais = $('#registar #pais').val();
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
             CodPostal: CodPostal,
             Distrito: distrito,
             Pais: pais
         },
         async: false
     }).done(function() {
     });
 });