<template>
<div class="container pt-5">
      show one car here
      <div class="pt-5">
            <div class="col-8">
                  <h1>{{car.make}} {{car.model}} </h1>
                  <h2>{{car.power}} {{car.bodyType}}</h2>
                  <h2><b>{{car.series}}</b></h2>
            </div>
      </div>
</div>
</template>

<script>


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
                        images:'',
                        equipment: ''
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
                  fetch(`https://localhost:44373/api/cars/${this.$route.params.id}`)
                  .then(res => res.json())
                  .then(res => {
                        this.car = res;
                  })
                  .catch(err => console.log(err));
            },


            goHome() {
                  this.$router.push('/');
            }
      }
}
</script>