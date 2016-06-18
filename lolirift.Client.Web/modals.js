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
  </div> 
  <div class="row">
    <a class="btn waves-effect waves-light right" onclick="submit()">Submit
      <i class="material-icons right">send</i>
    </a>
    <a class="waves-effect waves-teal btn-flat right hoverable" style="margin-right:5%" onclick="cancel()">cancel</a>
  </div>
  <script>
  function submit(){
      var name = $ ( '#building-name' ).val();
      var x = $ ( '#position-x' ).val().trim();
      var y = $ ( '#position-y' ).val().trim();
      
      ws.send(JSON.stringify({"controller":"build","building": name,"x":x,"y":y}));
      cancel();
  }
  function cancel(){
      $('#modal').empty();
      $('#modal').css('display', 'none');
  }
  </script>
</div>
`
    ,'name': `
<div class="modal-content">
  <div class="modal-title">Name</div>
  <div class="row">
    <div class="input-field col s12">
      <input id="name" type="text" class="validate">
      <label for="name">Your name</label>
    </div>
  </div>
  <div class="row">
    <a class="btn waves-effect waves-light right" onclick="submit()">Submit
      <i class="material-icons right">send</i>
    </a>
    <a class="waves-effect waves-teal btn-flat right hoverable" style="margin-right:5%" onclick="cancel()">cancel</a>
  </div>
  <script>
  function submit(){
      var name = $ ( '#name' ).val();
      ws.send(JSON.stringify({"controller":"name","name": name}));
      cancel();
  }
  function cancel(){
      $('#modal').empty();
      $('#modal').css('display', 'none');
  }
  </script>
</div>
`
};