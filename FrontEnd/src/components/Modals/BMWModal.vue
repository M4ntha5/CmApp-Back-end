<template>
       <!-- BMW modal -->
      <div>
            <div v-show="value" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
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
                                    <button @click.prevent="close" class="btn btn-default">Close</button>
                                    <button @click="addBMWCar()" class="btn btn-primary">Save</button>
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
                  insert: {
                        
                  }
            }
      },
      methods: {
            close() {
                  this.$emit("input", !this.value);
            },

            async addBMWCar()
            {
                  this.insert.make = "BMW";
                  var vm = this;
                  console.log(this.insert);
                  axios.post('https://localhost:44348/api/cars', this.insert)
                        .then(function (response) {
                              console.log(response);
                              if(response.status == 200)
                                    vm.$emit("input", !vm.value);
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
      }
}
</script>

<style lang="css" scoped>
      .modal {
      background-color: rgba(0, 0, 0, 0.7);
}
</style>