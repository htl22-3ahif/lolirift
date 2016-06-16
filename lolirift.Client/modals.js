var modals = {
    'build': `
<div class="modal-content">
  <div class="modal-title">Build</div>
  <div class="row">
    <div class="input-field col s12">
      <input id="building-name" type="text" class="validate">
      <label for="building-name">Name of building</label>
    </div>
  </div>
  <div class="row">
    <div class="input-field col s6">
      <input id="position-x" type="text" class="validate">
      <label for="position-x">Position X</label>
    </div>
    <div class="input-field col s6">
      <input id="position-y" type="text" class="validate">
      <label for="position-y">Position Y</label>
    </div>
    <a class="btn waves-effect waves-light" onclick="submit()">Submit
      <i class="material-icons right">send</i>
    </a>
  </div> 
  <script>
  function submit(){
      var name = $ ( '#building-name' ).val();
      var x = $ ( '#position-x' ).val().trim();
      var y = $ ( '#position-y' ).val().trim();
      
      console.log(JSON.stringify({"controller":"build","name": name,"x":x,"y":y}));
      ws.send(JSON.stringify({"controller":"build","name": name,"x":x,"y":y}));
      
      $('#modal').empty();
      $('#modal').css('display', 'none');
  }
  </script>
</div>
`
};