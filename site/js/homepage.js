$(document).ready(function() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "http://localhost:49234/api/search",
        data: {},
        async: false
    }).done(function (data) {
        console.log(data);
        var a = ['frigo', 'maquinalavar', 'micro', 'ml1', 'ml2', 'ml3', 'ml4', 'tv'];      
        for(var i = 0; i < data.length; ++i){         
            var rand = a[Math.floor(Math.random() * a.length)];
        $('.novidades-list').append('<div class="novidades-item col-md-4"><img src="../img/'+rand+'.png" class="img-responsive" alt="'+rand+'"><p>'+data[i]['DescArtigo']+'</p></div>');
        }       
    });
});
