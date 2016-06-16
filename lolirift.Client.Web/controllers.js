var controllers = [
    function(j){
        if (j["controller"] != "map")
            return;
        
        console.log ( "Map controller aktive" );
        
        var width = parseInt( j["width"] );
        var height = parseInt( j["height"] );
        
        for (i = 0; i < width * height; i++){
            var elem = $( '<div class="field"></div>' );
            elem.css({
                'width': (1/width)*100+'%',
                'height': (1/height)*100+'%'
            });
            elem.attr('pos', i%width+'/'+Math.floor(i/width));
            $('#grid').append(elem);
        }
        
        $('#grid').css({
            'width': width * 100+'px',
            'height': height * 100+'px'
        });
    }
    ,function(j){
        if (j["controller"] != "see")
            return;
        
        console.log("see controller aktive");
        
        j["seeable"].forEach(function(s){
            $ ( '.field[pos="'+s['x']+'/'+s['y']+'"]' ).html(s["unit"]+" by "+s["owner"]);
        });
    }
];