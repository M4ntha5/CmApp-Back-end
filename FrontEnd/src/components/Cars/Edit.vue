<template>
     <div class="container">
          <form class="pt-4 needs-validation" novalidate >
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer01">Make</label>
                         <input type="text" class="form-control" id="validationServer01" placeholder="BMW" required>
                         <div class="valid-feedback">
                         Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer02">Model</label>
                         <input type="text" class="form-control " id="validationServer02" placeholder="M5"  required>
                         <div class="valid-feedback">
                         Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer03">Series</label>
                         <input type="text" class="form-control " id="validationServer03" placeholder="F30" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-6 mb-3 disabled">
                         <label for="validationServer04">Vin</label>
                         <input type="text" class="form-control" id="validationServer04" placeholder="WDD545asd45w" readonly>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServerDate">Manufacture date</label>
                         <input type="date" class="form-control " id="validationServerDate" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">                  
                    <div class="col-md-3 mb-3">
                         <label for="validationServer04">Body type</label>
                         <input type="text" class="form-control " id="validationServer04" placeholder="Coupe"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer05">Steering</label>
                         <input type="text" class="form-control " id="validationServer05" placeholder="Left Hand Drive" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer06">Drive</label>
                         <input type="text" class="form-control " id="validationServer06" placeholder="HECK" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer7">Transmission</label>
                         <input type="text" class="form-control " id="validationServer7" placeholder="Automatic"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div> 
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer08">Engine</label>
                         <input type="text" class="form-control " id="validationServer08" placeholder="N55" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer09">Displacement</label>
                         <input type="number" class="form-control" id="validationServer09" placeholder="3" min="0.1" value="0.1" step=".1" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer10">Power</label>
                         <input type="text" class="form-control" id="validationServer10" placeholder="180kw / 245hp" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-6 mb-3">
                         <label for="validationServer11">Color</label>
                         <input type="text" class="form-control" id="validationServer11" placeholder="APLINWEISS 3 (300)" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServer12">Interior</label>
                         <input type="text" class="form-control " id="validationServer12" placeholder="Leather/nappa/semi-aniline..."  required/>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               
               <button class="btn btn-primary" type="submit">Submit</button>
          </form>
     </div>

</template>

<script>



// Example starter JavaScript for disabling form submissions if there are invalid fields
(function() 
{
     'use strict';
     window.addEventListener('load', function() 
     {
          // Fetch all the forms we want to apply custom Bootstrap validation styles to
          var forms = document.getElementsByClassName('needs-validation');
          // Loop over them and prevent submission
          Array.prototype.filter.call(forms, function(form) 
          {
               form.addEventListener('submit', function(event) 
               {
                    if (form.checkValidity() === false)
                    {
                         event.preventDefault();
                         event.stopPropagation();
                    }
                    form.classList.add('was-validated');
               }, false);
          });
     }, false);
})();


export default {
     data() {
          return {

          }
     }
}
</script>