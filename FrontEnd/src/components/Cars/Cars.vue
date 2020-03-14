<template>
<div>
      <div class="container pt-5" v-if="!loading">
            <b-alert v-model="alertFlag" variant="success" dismissible>{{alertMessage}}</b-alert>

            <button v-b-modal.car-insert-modal class="btn btn-primary"
            @click="showBmwModal">
                  Add new
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

import bmwModal from '../Modals/CarModal.vue';
import axios from 'axios';
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
export default {      
      data() {
            return {
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
                        equipment: [],
                        summary: {
                              boughtPrice:'',
                              soldPrice:'',
                              totalShipping: '',
                              sold: Boolean,
                        },
                  },
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
                  loading: true,
                  isBmwModalVisible: false,
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
                  axios.get(backEndUrl + '/api/cars')
                  .then(function (response) {
                        if(response.status == 200)
                        {
                              vm.cars = response.data;
                              vm.loading = false;
                              //setting repair value to dafault - first of a list
                              vm.insertRepair.car = vm.cars[0]._id;
                        }
                        
                  })
                  .catch(function (err) {
                        console.log(err);
                  });
            },
            fetchCarSummary() {
                  let vm = this;
                  for(let i =0; i< this.cars.length;i++)
                  {
                        fetch(backEndUrl + `/api/cars/${this.cars[i]._id}/summary`)
                        .then(function (response) {
                              if(response.status == 200)
                              {
                                    vm.car.summary = response.data;
                                    vm.loading = false;
                                    //setting repair value to dafault - first of a list
                                    vm.insertRepair.car = vm.cars[0]._id;
                              }

                        })
                        .catch(function (err) {
                              console.log(err);
                        });
                  }      
            },
            showAlert(message){
                  this.alertFlag = true;
                  this.alertMessage = message;
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

      }
}
</script>