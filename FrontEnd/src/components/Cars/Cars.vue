<template>
<div>
      <div class="container pt-5" v-if="!loading">
            <b-alert v-model="alertFlag" variant="success" dismissible>{{alertMessage}}</b-alert>
            <button v-b-modal.modal-prevent-closing class="btn btn-primary"
            @click="showBmwModal">
                  Add new
            </button>

            <button class="btn btn-primary" style="float:right;"
            data-toggle="modal" data-target="#repairModal">
                  Add repair
            </button>


            <!-- bmw modal-->
            <bmwModal v-show="isBmwModalVisible" @click="closeBmwModal"/>

            <div class="row">
                  <div class="pt-5 col-4" v-for="car in cars" v-bind:key="car._id">    
                        <a v-bind:href="'/cars/'+ car._id">
                              <div class="card" style="width: 20rem; height: 30rem;">                                                          
                                    <img :src='car.mainImgUrl' class="card-img-top img-thumbnail" alt="Responsive image">
                                    
                                    <div class="pt-3 card-body">
                                          <h2>{{car.make}} {{car.model}}</h2>
                                          <h4 class="card-text">
                                                {{car.vin}}
                                          </h4>
                                    </div>
                              </div>      
                        </a>
                  </div>
            </div> 
      </div>

      <div class="pt-3" v-else>
            <center><h1>Loading... please wait</h1></center> 
      </div>
 

      <!-- repair modal -->
      <div>
            <div class="modal fade" id="repairModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                  <div class="modal-dialog" role="document">
                        <div class="modal-content">
                              <div class="modal-header">                                  
                              <h4 class="modal-title" id="myModalLabel">Add new repair</h4>
                              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                              </div>
                              
                                    <div class="modal-body">
                                          <div class="form-row mb-3">
                                                <label for="comm">Name</label>
                                                <input type ="text" name="name" id="name" required class="form-control" placeholder="Front wind shield" v-model="insertRepair.name" />
                                                <div class="valid-feedback">
                                                      Looks good!
                                                </div>
                                          </div>                                        
                                          <div class="form-row mb-3" >
                                                <label for="comm">Price (€)</label>
                                                <input id="price" type ="number" name="price" min=1 step=".01" placeholder="100" required class="form-control" v-model="insertRepair.price" />
                                                <div class="valid-feedback">
                                                      Looks good!
                                                </div>      
                                          </div>                                         
                                          <div class="form-row mb-3s">
                                                <label for="comm">Car</label>
                                                <select class="form-control" required v-model="insertRepair.car">
                                                      <option v-for='data in cars' :key='data._id' :value='data._id'>{{data.make}} {{data.model}}</option>
                                                </select>
                                                <div class="valid-feedback">
                                                      Looks good!
                                                </div>      
                                          </div>
                                          
                                    </div>
                              
                              <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="repairModal">Close</button>
                                    <button @click="addRepairToCar()" type="submit" class="btn btn-primary">Save</button>
                              </div>
                              
                        </div>
                  </div>
            </div>
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

import bmwModal from '../Modals/BmwModal.vue';
import axios from 'axios';

export default {      
      data() {
            return {
                  modalShow: true,
                  alertMessage:'',
                  alertFlag: false,
                  cars: [],
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
                        equipment: [],
                        summary: {
                              boughtPrice:'',
                              soldPrice:'',
                              totalShipping: '',
                              sold: Boolean,
                        },
                                       
                  },
                  errors: [],
                  insertCar: {
                        vin: '',
                        boughtPrice: '',
                        make: '',
                        Base64images: []
                  } ,
                  insertRepair: {
                        name: '',
                        price: '',
                        car: ''
                  },
                  loading: true  ,
                  isBmwModalVisible: false

            }
            
      },

      watch: {
            //'$route' (to, from) {
              //    alert("to", to.params.id);
            //      alert("from", from.params.id);
          //  }
      },
      created() {
            this.fetchCars();
            this.fetchCarSummary();
         ///   alert(this.$route.params.id);
      },
            components: {
            bmwModal,
      },
      methods: {
            showBmwModal(){
                  this.isBmwModalVisible = true;
            },
            closeBmwModal() {
                  this.isBmwModalVisible = false;
            },
            fetchCars() {
                  let vm = this;
                  fetch('https://localhost:44348/api/cars')
                  .then(res => res.json())
                  .then(res => {
                        if(res)
                        {
                              this.cars = res;
                              vm.loading = false;
                              //setting repair value to dafault - first of a list
                              vm.insertRepair.car = vm.cars[0]._id;
                        }
                              
                  })
                  .catch(err => console.log(err));
            },
            fetchCarSummary() {
                  for(let i =0; i< this.cars.length;i++)
                  {
                        fetch(`https://localhost:44348/api/cars/${this.cars[i]._id}/summary`)
                        .then(res => res.json())
                        .then(res => {
                              if(res != null)
                                    this.car.summary = res;
                        })
                        .catch(err => console.log(err));
                  }
              
            },
            showAlert(message){
                  this.alertFlag = true;
                  this.alertMessage = message;
            },
            checkFormValidity() {
                  const valid = this.$refs.form.checkValidity()
                  this.price = valid
                  return valid
            },
            resetModal() {
                  this.vin = ''
                  this.price = null
            },
            handleOk(bvModalEvt) {
                  // Prevent modal from closing
                  bvModalEvt.preventDefault()
                  // Trigger submit handler
                  this.handleSubmit()
            },
            handleSubmit() {
                  // Exit when the form isn't valid
                  if (!this.checkFormValidity()) {
                        return
                  }
                  // Push the name to submitted names
                  this.submittedNames.push(this.price)
                  // Hide the modal manually
                  this.$nextTick(() => {
                        this.$bvModal.hide('modal-prevent-closing')
                  })
            },

            onFileSelected(e) {
                  for(let i=0; i < e.target.files.length; i++)
                  {
                  var reader = new FileReader();
                  reader.readAsDataURL(e.target.files[i]);
                  reader.onload = (e) => {
                              this.insertCar.Base64images[i] = e.target.result;
                  }                 
                  }
                  console.log(this.insertCar.Base64images);                
            },

            addRepairToCar()
            {
                 // let vm = this;
                  axios.post(`https://localhost:44348/api/cars/${this.insertRepair.car}/repairs`, this.insertRepair)
                        .then(function (response) {
                              console.log(response.statusText);
                              //vm.$router.push('/cars');
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },



            goHome() {
                  this.$router.push('/cars');
            },  

      }
}
</script>