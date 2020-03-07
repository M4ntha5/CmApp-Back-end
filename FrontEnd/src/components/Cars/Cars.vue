<template>
<div>
      <div class="container pt-5">
            cars here
            
            <button class="btn btn-primary" 
                        data-toggle="modal" 
                        data-target="#BMWModal">Add new</button>
            <div class="pt-5" v-for="car in cars" v-bind:key="car._id">
                  <div class="card" >
                        <div class="card-body" >
                              <div class="row mb-3">
                                    <div class="col-4">
                                          <a v-bind:href="'/cars/'+ car.id">
                                          <img src="/storage/images/" width="240px" height="180px" class="img-fluid" alt="Responsive image">
                                          </a>
                                    </div>
                                    <div class="col-8">
                                          <a v-bind:href="'/cars/'+ car._id">
                                          <h1>{{car.make}} {{car.model}} </h1>
                                          <h2>{{car.power}} {{car.bodyType}}</h2>
                                          </a>
                                          <h2><b>{{car.series}}</b></h2>
                                    </div>
                              </div>
                        </div>
                  </div>
            </div>
      </div>

      <!-- BMW modal -->
      <div>
            <div class="modal fade" id="BMWModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" :class="{'is-active' : modalShow}">
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
                                          <input type ="text" name="vin" id="vin" class="form-control"  v-model="insert.vin" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Price</label>
                                          <input type ="number" name="price" min=0 id="price" class="form-control"  v-model="insert.boughtPrice" />
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
            <div class="modal fade" id="MBModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" v-show="modalShow">
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
                                          <input type ="text" name="vin" id="vin" class="form-control" v-model="insert.vin" />
                                    </div>
                                    <div class="form-group">
                                          <label for="comm">Price2</label>
                                          <input type ="number" name="price" min=0 id="price" class="form-control" v-model="insert.boughtPrice" />
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
                        equipment: []
                  },
                  insert: {
                        vin: '',
                        boughtPrice: '',
                        make: '',
                        Base64images: []
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
         ///   alert(this.$route.params.id);
      },
      methods: {
            fetchCars() {
                  fetch('https://localhost:44348/api/cars')
                  .then(res => res.json())
                  .then(res => {
                        this.cars = res;
                  })
                  .catch(err => console.log(err));
            },

            addBMWCar()
            {
                  this.insert.make = "BMW";
                  var vm = this;
                  console.log(this.insert);
                  axios.post('https://localhost:44348/api/cars', this.insert)
                        .then(function (response) {
                              console.log(response);
                              if(response.status == 200)
                                    vm.modalShow = false;
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },
            addMBCar()
            {
                  var vm = this;
                  this.insert.make = "MB";
                  console.log(this.insert);
                  axios.post('https://localhost:44348/api/cars', this.insert)
                        .then(function (response) {
                              console.log(response);
                              if(response.status == 200)
                                    vm.modalShow = false;
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },

            async onFileSelected(e) {
                  for(let i=0; i < e.target.files.length; i++)
                  {
                        var reader = new FileReader();
                        reader.readAsDataURL(e.target.files[i]);
                        reader.onload = (e) => {
                              this.insert.Base64images[i] = e.target.result;
                        }                 
                  }
                  console.log(this.insert.Base64images);
                  
            }, 

            goHome() {
                  this.$router.push('/');
            }
      }
}
</script>