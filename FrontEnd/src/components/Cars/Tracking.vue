<template>
     <div class="container pt-5">
          tracking here
          <button @click="getTracking" class="btn btn-primary">click me</button>

          <div class="row mb-3 pt-5">
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

</template>

<script>
import VueGallery from 'vue-gallery';
//import axios from 'axios';
export default { 
      data() {
            return {               
                  tracking: {
                        containerNumber: '',
                        bookingNumber: '',
                        bse64images: [],
                        url: '',
                        car: ''
                  }, 
                  index: null,
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
           this.getTracking();
      },

      methods: {
            async getTracking() {
                  //if(confirm('Warning! This operation could take longer than 1 min! Want to continue? '))
                    fetch(`https://localhost:44348/api/cars/${this.$route.params.id}/tracking`)
                    .then(res => res.json())
                    .then(res => {
                         if(res != null)
                              this.tracking = res;
                    })
                    .catch(function (error) {
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