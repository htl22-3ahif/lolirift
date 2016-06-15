var ws = new WebSocket("ws://127.0.0.1:844");
var isMouseDown = false;
var posX = 0;
var posY = 0;
var gridX = 0;
var gridY = 0;
var scale = 1;

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
    
    
    $( 'html' ).mousedown(function(e){
        isMouseDown = true;
        posX = e.pageX;
        posY = e.pageY;
    });
    
    $( 'html' ).mouseup(function(){
        isMouseDown = false;
    });
    
    $( 'html' ).mousemove(function(e){
        if (!isMouseDown)
            return;
		gridX += (posX - e.pageX) * (1/scale);
		gridY += (posY - e.pageY) * (1/scale);
        posX = e.pageX;
        posY = e.pageY;
		$('#grid').css({
            'right': gridX,
            'bottom': gridY
		});
    });
    
    $ ( 'html' ).bind('mousewheel', function(e){
        if(e.originalEvent.wheelDelta /120 > 0) {
            scale += 0.1 * scale;
        }
        else{
            scale -= 0.1 * scale;
        }
        $(this).css({
            'transform': 'scale('+scale+')'
        });
    });
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
