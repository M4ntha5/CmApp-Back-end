<template>
<div>
      <div v-if="loading">
            <div class="container">
                  <h1 class="center">Loading...</h1>
            </div>        
      </div>
      <div class="container pt-5" v-if="!loading">       
               <div class="row">
                  <div class="col-sm-6 col-12">
                        <h1>{{car.make}} {{car.model}}</h1>
                  </div>
                  <div class="col-sm-2 col-12">
                        <button class="btn btn-primary" style="float:right;">Edit</button>
                  </div>
                  <div class="col-sm-2 col-12">
                        <button class="btn btn-primary" style="float:right;"
                        data-toggle="modal" data-target="#repairModal">Add repair</button>
                  </div>
                  <div class="col-sm-2 col-12">
                        <a v-bind:href="'/cars/'+ car._id + '/tracking'">Look for tracking</a>
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

            <!-- repair modal -->
            <div>
                  <div class="modal fade" id="repairModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                        <div class="modal-dialog" role="document">
                              <div class="modal-content">
                                    <div class="modal-header">                                  
                                    <h4 class="modal-title" id="myModalLabel">Add new car</h4>

                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                                    </div>
                                    <div class="modal-body">
                                          <div class="form-group">
                                                <label for="comm">Name</label>
                                                <input type ="text" name="name" id="name" required class="form-control" v-model="insertRepair.name" />
                                          </div>
                                          <div class="form-group">
                                                <label for="comm">Price</label>
                                                <input type ="number" name="price" min=0 id="price" required class="form-control" v-model="insertRepair.price" />
                                          </div>
                                    </div>
                                    <div class="modal-footer">
                                          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                          <button @click="addRepairToCar()" class="btn btn-primary">Save</button>
                                    </div>
                              </div>
                        </div>
                  </div>
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
                  loading: false
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
            await this.fetchCar();
            await this.fetchCarRepairs();
      },

      methods: {
            async fetchCar() {
                  var vm = this;
                  vm.loading = true;
                  fetch(`https://localhost:44348/api/cars/${this.$route.params.id}`)
                        .then(res => res.json())
                        .then(res => {                             
                              this.car = res;
                              vm.loading = false;
                        })
                        .catch(function (error) {
                              vm.loading = false;
                              console.log(error);
                        });
                  
                  
            },
            async fetchCarRepairs() {
                  fetch(`https://localhost:44348/api/cars/${this.$route.params.id}/repairs`)
                  .then(res => res.json())
                  .then(res => {
                        this.loading = false;
                        this.repairs = res;
                  })
                  .catch(function (error) {
                        this.loading = false;
                        console.log(error);
                  });
            },

            async fetchCarShipping() {
                  fetch(`https://localhost:44348/api/cars/${this.$route.params.id}/shipping`)
                  .then(res => res.json())
                  .then(res => {
                        this.loading = false;
                        this.shipping = res;
                  })
                  .catch(function (error) {
                        this.loading = false;
                        console.log(error);
                  });
            },
            async fetchCarSummary() {
                  fetch(`https://localhost:44348/api/cars/${this.$route.params.id}/summary`)
                  .then(res => res.json())
                  .then(res => {
                        this.loading = false;
                        if(res != null)
                              this.car.summary = res;
                  })
                  .catch(function (error) {
                        this.loading = false;
                        console.log(error);
                  });           
            },
            async getTracking() {
                  if(confirm('Warning! This operation could take longer than 1 min! Want to continue? '))
                  {
                        fetch(`https://localhost:44348/api/cars/${this.$route.params.id}/tracking`)
                        .then(res => res.json())
                        .then(res => {
                              this.loading = false;
                              if(res != null)
                                    this.car.summary = res;
                        })
                        .catch(function (error) {
                              this.loading = false;
                              console.log(error);
                        });  
                  }         
            },
            async addRepairToCar()
            {
                  this.loading = true;
                  var vm = this;
                  axios.post(`https://localhost:44348/api/cars/${this.$route.params.id}/repairs`, this.insertRepair)
                        .then(function (response) {
                              vm.loading = false;
                              console.log(response);
                              if(response.status == 200)
                              {
                                    vm.modalShow = false;
                              }
                        })
                        .catch(function (error) {
                              this.loading = false;
                              console.log(error);
                        });
            },
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