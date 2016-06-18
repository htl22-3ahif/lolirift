var controllers = [
    function(j){
        if (j["controller"] != "map")
            return;
        
        console.log ( "Map controller aktive" );
        
        var width = parseInt( j["width"] );
        var height = parseInt( j["height"] );
        var heightmap = j["heightmap"];
        var elem = 0;
        
        for (i = 0; i < width * height; i++){
            elem = $( '<div class="field"><div class="card hoverable" style="height: 100%;"></div></div>' );
            elem.attr('pos', i%width+'/'+Math.floor(i/width));
            $('#grid').append(elem);
        }
        
        $( '#grid' ).css({
          'width': $('.field').outerWidth()*width+'px',
          'height': $('.field').outerHeight()*height+'px'
        });
    }
    ,function(j){
        if (j["controller"] != "see")
            return;
        
        console.log("see controller aktive");
        
        j["seeable"].forEach(function(s){
            $ ( '#grid .field[pos="'+s['x']+'/'+s['y']+'"] .card' ).html(s["unit"]+" by "+s["owner"]);
        });
    }
];