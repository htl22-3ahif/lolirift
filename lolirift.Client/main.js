var ws = new WebSocket("ws://127.0.0.1:844");
var isMouseDown = false;
var posX = 0;
var posY = 0;

var controllers = [
    function(j){
        if (j["controller"] != "map")
            return;
        
        console.log ( "Map controller aktive" );
        
        var width = parseInt( j["width"] );
        var height = parseInt( j["height"] );
        
        for (i = 0; i < width * height; i++){
            var elem = $( '<div class="gridelem"></div>' );
            elem.css({
                'width': (1/width)*100+'%',
                'height': (1/height)*100+'%'
            });
            $('#grid').append(elem);
        }
        
        $('#grid').css({
            'width': width * 100+'px',
            'height': height * 100+'px'
        });
    }
];

ws.onopen = function(){
    ws.send( '{ "controller": "map" }' );
    
    console.log("defining events");
    
    $( '#grid' ).mousedown(function(e){
        console.log("mousedown");
        isMouseDown = true;
        posX = e.pageX;
        poxY = e.pageY;
    });
    
    $( '#grid' ).mouseup(function(){
        console.log("mouseup");
        isMouseDown = false;
    });
    
    $( '#grid' ).mousemove(function(e){
        if (!isMouseDown)
            return;
        
        console.log(e.pageX + " " + e.pageY);
    });
    console.log("event made");
};

ws.onerror = function(error){
    
};

ws.onmessage = function(e){
    console.log( "Server Message: " + e.data );
    var j = JSON.parse( e.data );
    
    controllers.forEach(function(c){
        c( j );
    });
};
