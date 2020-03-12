<template>
<div>         
      <div class="container pt-5" >  
            <div v-if="!loading">      
                  <div class="row">
                        <div class="col-sm-6 col-12">
                              <h1>{{car.make}} {{car.model}}</h1>
                        </div>
                        <div class="col-sm-2 col-12">
                              <button @click="editCar(car._id)" class="btn btn-primary" style="float:right;">
                                    Edit
                              </button>
                        </div>
                        <div class="col-sm-2 col-12">
                              <button @click="openTracking(car._id)" class="btn btn-primary">
                                    Look for tracking
                              </button>
                        </div>
                  </div>
                  <div class="row mb-3 pt-5">
                        <div class="img-fluid col-sm-4 col-12">              
                              <gallery :images="car.base64images" :index="index" @close="index = null"></gallery>
                              <div class="image" 
                                    @click="index = 0"
                                    :style="{ backgroundImage: 'url(' + car.base64images[0] + ')', width:'350px', height:'300px' }"
                              /> 
                              <!--  <div v-for="(carImage, imageIndex) in car.base64images" :key="imageIndex">
                                    <img class="img-thumbnail" height="400px" width="400px" alt="Responsive image" :src='carImage'>
                              </div>--> 
                        </div>
            
                        <div class="col-sm-8 col-12">
                              <table class="table">
                                    <tr>
                                          <th>Manufacture Date</th>
                                          <td>{{car.manufactureDate}}</td>
                                    </tr>
                                    <tr>
                                          <th>Engine / power</th>
                                          <td>{{car.engine}} {{car.power}}</td>
                                    </tr>
                                    <tr>
                                          <th>Series</th>
                                          <td>{{car.series}}</td>
                                    </tr>
                                    <tr>
                                          <th>Body type</th>
                                          <td>{{car.bodyType}}</td>
                                    </tr>
                                    <tr>
                                          <th>Driven wheels</th>
                                          <td>{{car.drive}}</td>
                                    </tr>
                                    <tr>
                                          <th>Gearbox</th>
                                          <td>{{car.transmission}}</td>
                                    </tr>
                                    <tr>
                                          <th>Interior</th>
                                          <td>{{car.interior}}</td>
                                    </tr>
                                    <tr>
                                          <th>Steering</th>
                                          <td>{{car.steering}}</td>
                                    </tr>
                              </table>
                        </div>
                  </div>

                  <div class="pt-2">
                        <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#equipmentCollapse" aria-expanded="false" aria-controls="collapseExample">
                              Equipment
                        </button>
                  </div>
            
                  <!-- equipment collapse table -->
                  <div class="collapse" id="equipmentCollapse">
                        <table class="table table-dark">
                              <div class="row">
                                    <div class="col-4" v-for="eq in car.equipment" v-bind:key="eq.id">
                                          <tbody>
                                                <tr>
                                                      <td>{{eq.name}}</td>
                                                </tr>
                                          </tbody>
                                    </div>
                              </div>
                        </table>
                  </div>

                  <div class="pt-2">
                        <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#repairsCollapse" aria-expanded="false" aria-controls="collapseExample">
                              Repairs
                        </button>
                  </div>
                  

                  <!-- repairs collapse table -->
                  <div class="collapse" id="repairsCollapse">
                        <table class="table table-dark" v-if="repairs.length">
                              <div class="row" >
                                    <div class="col-4" v-for="repair in repairs" v-bind:key="repair._id">
                                          <tbody>
                                                <tr>
                                                      <td>{{repair.name}}     {{repair.price}}€</td>
                                                </tr>
                                          </tbody>
                                    </div>                 
                              </div>
                              <h2 class="pl-3">Total: {{repairs[0].total}}€</h2>             
                        </table>
                        <div v-else>
                              <h3>No repairs yet</h3>
                        </div>
                  </div>

                  <div class="pt-2">
                        <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#shippingCollapse" aria-expanded="false" aria-controls="collapseExample">
                              Shipping
                        </button>
                  </div>
                  

                  <!-- shipping collapse table -->
                  <div class="collapse" id="shippingCollapse">
                        <table class="table table-dark" v-if="shipping.customs != 0">
                              <div class="row">
                                    <div class="col-6">
                                          <tbody>
                                                <tr>
                                                      <th>Auction fee</th>
                                                      <td>{{shipping.auctionFee}}</td>
                                                </tr>
                                                <tr>
                                                      <th>Transfer fee</th>
                                                      <td>{{shipping.transferFee}}</td>
                                                </tr>
                                                <tr>
                                                      <th>Transportation fee</th>
                                                      <td>{{shipping.transportationFee}}</td>
                                                </tr>
                                                <tr>
                                                      <th>Customs</th>
                                                      <td>{{shipping.customs}}</td>
                                                </tr>
                                          </tbody>
                                    </div>                       
                              </div>                
                        </table>
                        <div v-else>
                              <h3>No shipping yet</h3>
                        </div>
                  </div>                 
            </div>
            <div class="pt-3" v-else>
                 <center><h1>Loading... please wait</h1></center> 
            </div>
      </div>
      
</div>
<!--

      <div class="pt-5">
            <div class="col-8">
                  <div v-for="(carImage, imageIndex) in car.base64images" :key="imageIndex">
                        <img class="img-thumbnail" height="200px" width="200px" alt="Responsive image" :src='carImage'>
                  </div>
                  <h1>{{car.make}} {{car.model}} </h1>
                  <h2>{{car.power}} {{car.bodyType}}</h2>
                  <h2><b>{{car.series}}</b></h2>
            </div>
      </div>
-->

</template>

<script>

import VueGallery from 'vue-gallery';
import axios from 'axios';

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
                        equipment: [],
                        base64images: [],
                        summary: {
                              boughtPrice:'',
                              soldPrice:'',
                              totalShipping: '',
                              sold: Boolean,
                        }
                  },
                  repairs: [],
                  repair: {
                        name: '',
                        price: '',
                        total: ''
                  },
                  shipping: {
                        customs: '',
                        auctionFee: '',
                        transferFee: '',
                        transportationFee: ''
                  },
                  insertRepair: {
                        name: '',
                        price: '',
                  }, 
                  index: null,
                  loading: true,
            }
            
      },
      components: {
            'gallery': VueGallery,
      },
      watch: {
            //'$route' (to, from) {
              //    alert("to", to.params.id);
            //      alert("from", from.params.id);
          //  }
      },
      async created() {
            this.fetchCar();
            this.fetchCarRepairs();
            this.fetchCarShipping();
      },

      methods: {
            async fetchCar() {
                  var vm = this;
                  axios.get(`https://localhost:44348/api/cars/${vm.$route.params.id}`)
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
            async fetchCarRepairs() {
                  var vm = this;
                  axios.get(`https://localhost:44348/api/cars/${vm.$route.params.id}/repairs`)
                  .then(function (response) {
                        if(response.status == 200)
                        {
                              vm.repairs = response.data;
                        } 
                  })
                  .catch(function (error) {
                        console.log(error);
                  });
            },

            async fetchCarShipping() {
                  var vm = this;
                  axios.get(`https://localhost:44348/api/cars/${vm.$route.params.id}/shipping`)
                  .then(function (response) {
                        if(response.status == 200)
                        {
                              vm.shipping = response.data;
                        }
                  })
                  .catch(function (error) {
                        console.log(error);
                  });
            },
            async fetchCarSummary() {
                  var vm = this;
                  axios.get(`https://localhost:44348/api/cars/${vm.$route.params.id}/summary`)
                  .then(function (response) {
                        if(response.status == 200)
                        {
                              vm.car.summary = response.data;
                        }
                  })
                  .catch(function (error) {
                        console.log(error);
                  });           
            },
            async openTracking(id)
            {
                  window.location.href = `/cars/${id}/tracking`;    
            },
            async editCar(id)
            {
                  window.location.href = `/cars/${id}/edit`;  
            }
      }
}
</script>
<style scoped>
  .image {
    float: left;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center center;
    border: 1px solid #ebebeb;
    margin: 5px;
  }
  .divider{
    width:5px;
    height:auto;
    display:inline-block;
}
</style> 