<template>
<div>
     <div class="container" v-if="!loading">
          <form class="pt-4 needs-validation" novalidate>
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer01">Make</label>
                         <input v-model="car.make" type="text" class="form-control" id="validationServer01" placeholder="BMW" required>
                         <div class="valid-feedback">
                         Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer02">Model</label>
                         <input v-model="car.model" type="text" class="form-control " id="validationServer02" placeholder="M5"  required>
                         <div class="valid-feedback">
                         Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer03">Series</label>
                         <input v-model="car.series" type="text" class="form-control " id="validationServer03" placeholder="F30" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-6 mb-3 disabled">
                         <label for="validationServer04">Vin</label>
                         <input v-model="car.vin" type="text" class="form-control" id="validationServer04" placeholder="WDD545asd45w" readonly>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServerDate">Manufacture date</label>
                         <input v-model="car.manufactureDate" type="date" class="form-control " id="validationServerDate" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">                  
                    <div class="col-md-3 mb-3">
                         <label for="validationServer04">Body type</label>
                         <input v-model="car.bodyType" type="text" class="form-control " id="validationServer04" placeholder="Coupe"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer05">Steering</label>
                         <input v-model="car.steering" type="text" class="form-control " id="validationServer05" placeholder="Left Hand Drive" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer06">Drive</label>
                         <input v-model="car.drive" type="text" class="form-control " id="validationServer06" placeholder="HECK" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-3 mb-3">
                         <label for="validationServer7">Transmission</label>
                         <input v-model="car.transmission" type="text" class="form-control " id="validationServer7" placeholder="Automatic"  required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div> 
               <div class="form-row">
                    <div class="col-md-4 mb-3">
                         <label for="validationServer08">Engine</label>
                         <input v-model="car.engine" type="text" class="form-control " id="validationServer08" placeholder="N55" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer09">Displacement</label>
                         <input v-model="car.displacement" type="number" class="form-control" id="validationServer09" placeholder="3" min="0.1" value="0.1" step=".1" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-4 mb-3">
                         <label for="validationServer10">Power</label>
                         <input v-model="car.power" type="text" class="form-control" id="validationServer10" placeholder="180kw / 245hp" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
               </div>
               <div class="form-row">
                    <div class="col-md-6 mb-3">
                         <label for="validationServer11">Color</label>
                         <input v-model="car.color" type="text" class="form-control" id="validationServer11" placeholder="APLINWEISS 3 (300)" required>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>
                    <div class="col-md-6 mb-3">
                         <label for="validationServer12">Interior</label>
                         <input v-model="car.interior" type="text" class="form-control " id="validationServer12" placeholder="Leather/nappa/semi-aniline..."  required/>
                         <div class="valid-feedback">
                              Looks good!
                         </div>
                    </div>        
               </div>
               <a @click="updateCar" class="btn btn-primary" style="color:white;">Submit</a>
          </form>       
     </div>
      <div class="pt-3" v-else>
            <center><h1>Loading... please wait</h1></center> 
      </div>
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

import axios from 'axios';
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
export default {
     data() {
          return {
               car: {          
                    _id: '',
                    make:'',
                    model:'',
                    vin:'',
                    manufactureDate:'',
                    series:'',
                    bodyType:'',
                    steering:'',
                    engine:'',
                    displacement:'',
                    power:'',
                    drive:'',
                    transmission:'',
                    color:'',
                    interior:'',
                    created_at:'',
                    mainImgUrl:'',
                    images: [],
                    base64images: [],
                    equipment: [],
               },
               summary: {
                    boughtPrice:'',
                    soldPrice:'',
                    totalShipping: '',
                    sold: '',
               }, 
               loading: true
          }
     }, 
     created() {
          this.fetchCar();
     },
     methods: {
          
          async fetchCar() {
               var vm = this;
               axios.get(backEndUrl + `/api/cars/${vm.$route.params.id}`)
               .then(function (response) {
                    if(response.status == 200)
                    {
                         vm.car = response.data;  
                         //trimming unnecessary dat ending           
                         vm.car.manufactureDate = vm.car.manufactureDate.substring(0, 10);
                         vm.loading = false;
                    }
               })
               .catch(function (error) {
                    console.log(error);
               });               
          },
          async updateCar() {
               let vm = this;
               axios.put(backEndUrl + `/api/cars/${this.$route.params.id}`, this.car)
               .then(function (response) {
                    if(response.status == 204)
                         vm.$router.push(`/cars/${vm.$route.params.id}`);
               })
               .catch(function (error) {
                    console.log(error);
               });     
          }
     }
}
</script>