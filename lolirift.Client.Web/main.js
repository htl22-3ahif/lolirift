var ws = new WebSocket("ws://127.0.0.1:844");
var isMouseDown = false;
var posX = 0;
var posY = 0;
var gridX = 0;
var gridY = 0;
var scale = 1;

ws.onopen = function(){
    
    $('#modal').append(modals['name']);
    $('#modal').css('display', 'block');
    
    ws.send( '{ "controller": "map" }' );
  
    $('#modal').click(function(e){
        if (e.target.id != 'modal')
            return;
    
        $('#modal').empty();
        $('#modal').css('display', 'none');
    });
  
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
            'transform': 'translate('+gridX+','+gridY+')'
        });
    });
  
    $( '#build' ).click(function(e){
        $('#modal').append(modals['build']);
        $('#modal').css('display', 'block');
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
