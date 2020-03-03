<template>
<div>
      <div class="container pt-5">
            cars here
            
            <button class="btn btn-primary" 
                        data-toggle="modal" 
                        data-target="#add">Pridėti</button>
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

      <!-- modal -->
      <div>
            <div class="modal fade" id="add" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                  <div class="modal-dialog" role="document">
                        <div class="modal-content">
                              <div class="modal-header">
                              <h4 class="modal-title" id="myModalLabel">Pridėti automobili</h4>
                              <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                              
                              </div>
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
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Uždaryti</button>
                                    <button @click="addCar()" class="btn btn-primary">Išsaugoti</button>
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

            addCar()
            {
                  console.log(this.insert);
                  axios.post('https://localhost:44348/api/cars', this.insert)
                        .then(function (response) {
                              console.log(response);
                        })
                        .catch(function (error) {
                              console.log(error);
                        });
            },

            onFileSelected(e) {
                  var reader = new FileReader();
                  reader.readAsDataURL(e.target.files[0]);
                  reader.onload = (e) => {
                        this.insert.Base64images[0] = e.target.result;
                  }
                  console.log(this.insert);
            }, 

            goHome() {
                  this.$router.push('/');
            }
      }
}
</script>