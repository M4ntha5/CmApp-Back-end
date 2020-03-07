<template>
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
</template>