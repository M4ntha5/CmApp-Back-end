<template>
<div>
      <div class="container pt-5">

            <button class="btn btn-primary"
            data-toggle="modal" data-target="#BMWModal">Add new</button>

            <button class="btn btn-primary" style="float:right;"
             data-toggle="modal" data-target="#repairModal"
            >Add repair</button>

            <div class="row">
                  <div class="pt-5 col-4" v-for="car in cars" v-bind:key="car._id">    
                        <a v-bind:href="'/cars/'+ car._id">
                              <div class="card" style="width: 20rem; height: 30rem;">                                                          
                                    <img :src='car.base64images[0]' class="card-img-top img-thumbnail" alt="Responsive image">
                                    
                                    <div class="pt-3 card-body">
                                          <h2>{{car.make}} {{car.model}}</h2>
                                          <h4 class="card-text" style="color:red;">
                                                Bought price: {{car.summary.boughtPrice}} €
                                          </h4>
                                    </div>
                              </div>      
                        </a>
                  </div>
            </div>   
      </div>

      <!-- BMW modal -->
      <div>
            <div class="modal fade" id="BMWModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                  <div class="modal-dialog" role="document">
                        <div class="modal-content">
                              <div class="modal-header">                                  
                              <h4 class="modal-title" id="myModalLabel">Add new car</h4>

                              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                              </div>
                              <nav class="navbar navbar-expand-lg navbar-light bg-light">
                                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                                          <ul class="nav nav-tabs">
                                                <li class="nav-item">
                                                      <a class="nav-link active" href="">BMW</a>
                                                </li>
                                                <li class="nav-item">
                                                      <a class="nav-link" data-dismiss="modal" data-toggle="modal" data-target="#MBModal" href="">
                                                            Mercedes-benz
                                                      </a>
                                                </li>
                                                <li class="nav-item">
                                                      <a class="nav-link" data-dismiss="modal" data-toggle="modal" data-target="#OtherModal" href="">
                                                            Other
                                                      </a>
                                                </li>
                                          </ul>
                                    </div>
                              </nav>
                              <div class="modal-body" enctype="multipart/form-data">
                                    <div class="form-group">
                                          <label for="comm">Vin</label>
                                          <input type ="text" name="vin" id="vin" class="form-control"  v-model="insertCar.vin" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Price</label>
                                          <input type ="number" name="price" min=0 id="price" class="form-control"  v-model="insertCar.boughtPrice" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Images</label>
                                          <input type ="file" multiple name="price" min=0 id="price" class="form-control" @change="onFileSelected" />
                                    </div>
                              </div>
                              <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    <button @click="addBMWCar()" class="btn btn-primary">Save</button>
                              </div>
                        </div>
                  </div>
            </div>
      </div>

      <!-- MB modal -->
      <div>
            <div class="modal fade" id="MBModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                  <div class="modal-dialog" role="document">
                        <div class="modal-content">
                              <div class="modal-header">                                  
                              <h4 class="modal-title" id="myModalLabel">Add new car</h4>

                              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                              </div>
                              <nav class="navbar navbar-expand-lg navbar-light bg-light">
                                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                                          <ul class="nav nav-tabs">
                                                <li class="nav-item">
                                                      <a class="nav-link" data-dismiss="modal" data-toggle="modal" data-target="#BMWModal" href="">
                                                            BMW
                                                      </a>
                                                </li>
                                                <li class="nav-item">
                                                      <a class="nav-link active" href="">Mercedes-benz</a>
                                                </li>
                                                <li class="nav-item">
                                                      <a class="nav-link" data-dismiss="modal" data-toggle="modal" data-target="#OthersModal" href="">
                                                            Other
                                                      </a>
                                                </li>
                                          </ul>
                                    </div>
                              </nav>
                              <div class="modal-body" enctype="multipart/form-data">
                                    <div class="form-group">
                                          <label for="comm">Vin2</label>
                                          <input type ="text" name="vin" id="vin" class="form-control" v-model="insertCar.vin" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Price2</label>
                                          <input type ="number" name="price" min=0 id="price" class="form-control" v-model="insertCar.boughtPrice" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Images2</label>
                                          <input type ="file" multiple name="price" min=0 id="price" class="form-control" @change="onFileSelected" />
                                    </div>
                              </div>
                              <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    <button @click="addMBCar()" class="btn btn-primary">Save</button>
                              </div>
                        </div>
                  </div>
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
                                    <div class="form-group">
                                          <label for="comm">Car</label>
                                          <select class="form-control" required v-model="insertRepair.car">
                                                <option value="0">Select</option>
                                                <option v-for='data in cars' :key='data._id' :value='data._id'>{{data.model}}</option>
                                          </select>
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
</template>

<script>

import axios from 'axios';

export default {      
      data() {
            return {
                  modalShow: true,
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
                        images: [],
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
                  }         
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
      methods: {
            fetchCars() {
                  fetch('https://localhost:44348/api/cars')
                  .then(res => res.json())
                  .then(res => {
                        if(res != null)
                              this.cars = res;
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

            addBMWCar()
            {
                  this.insertCar.make = "BMW";
                  console.log(this.insertCar);
                  axios.post('https://localhost:44348/api/cars', this.insertCar)
                        .then(function (response) {
                              if(response.statusTesxt == "OK")
                                    location.reload();
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },
            addMBCar()
            {
                  var vm = this;
                  this.insertCar.make = "Mercedes-benz";
                  console.log(this.insertCar);
                  axios.post('https://localhost:44348/api/cars', this.insertCar)
                        .then(function (response) {
                              console.log(response);
                              if(response.status == 200)
                                    vm.modalShow = false;
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },

            addRepairToCar()
            {
                  axios.post(`https://localhost:44348/api/cars/${this.insertRepair.car}/repairs`, this.insertRepair)
                        .then(function (response) {
                              console.log(response.statusText);
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
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

            goHome() {
                  this.$router.push('/');
            },

      }
}
</script>