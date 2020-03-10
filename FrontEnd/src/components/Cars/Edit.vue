<template>
     <div class="container">
          <form>
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer01">Make</label>
                         <input type="text" class="form-control is-valid" id="validationServer01" placeholder="BMW" required>
                         <div class="valid-feedback">
                         Looks good!
                         </div>
                         <div class="invalid-feedback">
                         Looks bad!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer02">Model</label>
                         <input type="text" class="form-control is-valid" id="validationServer02" placeholder="M5"  required>
                         <div class="invalid-feedback">
                         Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer03">Series</label>
                         <input type="text" class="form-control is-invalid" id="validationServer03" placeholder="F30" required>
                         <div class="invalid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServerDate">Manufacture date</label>
                         <input type="date" class="form-control is-valid" id="validationServerDate" required>
                         <div class="invalid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer04">Body type</label>
                         <input type="text" class="form-control is-valid" id="validationServer04" placeholder="Coupe"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer05">Steering</label>
                         <input type="date" class="form-control is-invalid" id="validationServer05" placeholder="Left Hand Drive" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
                              <div class="form-row">
                    <div class="col-md-6 mb-3">
                         <label for="validationServer06">Drive</label>
                         <input type="text" class="form-control is-valid" id="validationServer06" placeholder="HECK" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServer7">Transmission</label>
                         <input type="text" class="form-control is-valid" id="validationServer7" placeholder="Automatic"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer08">Engine</label>
                         <input type="text" class="form-control is-valid" id="validationServer08" placeholder="N55" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer09">Displacement</label>
                         <input type="number" class="form-control is-valid" id="validationServer09" placeholder="3"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer10">Power</label>
                         <input type="text" class="form-control is-invalid" id="validationServer10" placeholder="180kw / 245hp" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-6 mb-3">
                         <label for="validationServer11">Color</label>
                         <input type="text" class="form-control is-valid" id="validationServer11" placeholder="APLINWEISS 3 (300)" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServer12">Interior</label>
                         <texarea type="text" class="form-control is-valid" id="validationServer12" placeholder="Leather/nappa/semi-aniline..."  required/>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               
               <button class="btn btn-primary" type="submit">Submit form</button>
          </form>
     </div>

</template>

<script>

export default {
     data() {
          return {

          }
     }
}
</script>