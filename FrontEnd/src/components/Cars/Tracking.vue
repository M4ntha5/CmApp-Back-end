<template>
      <div class="container pt-5">
            <b-alert v-model="alertFlag" variant="warning" dismissible>{{alertMessage}}</b-alert>
            <div v-if="!loading && !empty">
                  <div class="row mb-3 pt-5" >
                        <div class="img-fluid col-sm-4 col-12">              
                              <gallery :images="tracking.base64images" :index="index" @close="index = null"></gallery>
                              <div class="image" 
                                    @click="index = 0"
                                    :style="{ backgroundImage: 'url(' + tracking.base64images[0] + ')', width:'350px', height:'300px' }"
                              /> 
                              <!--  <div v-for="(carImage, imageIndex) in car.base64images" :key="imageIndex">
                                    <img class="img-thumbnail" height="400px" width="400px" alt="Responsive image" :src='carImage'>
                              </div>--> 
                        </div>

                        <div class="col-sm-8 col-12">
                              <table class="table">
                                    <tr>
                                          <th>Container number</th>
                                          <td>{{tracking.containerNumber}}</td>
                                    </tr>
                                    <tr>
                                          <th>Bokking number</th>
                                          <td>{{tracking.bookingNumber}}</td>
                                    </tr>
                                    <tr>
                                          <th>Url to full tracking info</th>
                                          <td><a :href='tracking.url' target="_blank">Click here</a></td>
                                    </tr>
                              </table>
                        </div>
                  </div>
            </div>
            <div class="pt-3" v-else-if="empty && !loading">
                  <center>
                        <h1>There is no tracking data yet</h1>
                  </center>
                  <center class="pt-3">
                        <button @click="lookFortracking"  class="btn btn-primary">Check for available tracking info?</button>
                  </center>
            </div>
            <div class="pt-3" v-else-if="loading">
                 <center><h1>Loading... please wait</h1></center> 
            </div>   
      </div>
</template>

<script>
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
import VueGallery from 'vue-gallery';
import axios from 'axios';

export default { 
      data() {
            return {               
                  tracking: {
                        containerNumber: '',
                        bookingNumber: '',
                        base64images: [],
                        url: '',
                        car: ''
                  }, 
                  index: null,
                  loading: true,
                  empty: true,
                  alertFlag: false,
                  alertMessage: ''
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
            this.fetchTracking();
      },

      methods: {
            async fetchTracking() {
                  let vm = this;
                  axios.get(backEndUrl + `/api/cars/${this.$route.params.id}/tracking`)
                  .then(function (response) {
                        if(response.data)
                        {
                              vm.tracking = response.data;
                              vm.loading = false;
                              vm.empty = false;
                        }
                        else
                        {
                              vm.loading = false;
                              vm.empty = true;
                        }                             
                  })
                  .catch(function (error) {
                        console.log(error);
                  });                                        
            },
            lookFortracking(){
                  if(confirm("Attention! This action could take more then a couple of minutes. Are you sure want to continue?"))
                  {
                        let vm = this;
                        axios.post(backEndUrl + `/api/cars/${this.$route.params.id}/tracking`)
                        .then(function (response) {
                              if(response.data)
                              {
                                    vm.tracking = response.data;
                                    vm.loading = false;
                                    vm.empty = false;
                              }
                              else
                              {
                                    vm.loading = false;
                                    vm.empty = true;
                              }
                                    
                        })
                        .catch(function (error) {
                              vm.alertMessage = error.response.data;
                              vm.alertFlag = true;
                              console.log(error);
                        }); 
                  }
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