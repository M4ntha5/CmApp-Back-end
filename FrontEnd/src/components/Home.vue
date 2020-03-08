<template>
  <!--Carousel Wrapper-->
          <div id="carousel-example-2" class="carousel slide carousel-fade z-depth-1-half" data-ride="carousel">
          <!--Indicators-->
          <ol class="carousel-indicators">
          <li data-target="#carousel-example-2" data-slide-to="0" class="active"></li>
          <li data-target="#carousel-example-2" data-slide-to="1"></li>
          <li data-target="#carousel-example-2" data-slide-to="2"></li>
          </ol>
          <!--/.Indicators-->
          <!--Slides-->
          <div class="carousel-inner" role="listbox">
          <div class="carousel-item active">
               <div class="view">
               <img class="d-block w-100" height="600px" src="https://i.pinimg.com/originals/e9/94/6b/e9946b570081bc51c3ad1f442e6bdb49.jpg" alt="First slide">
               <div class="mask rgba-black-light"></div>
               </div>
               <div class="carousel-caption">
               <h1 class="h1-responsive">Full equipment only by vin number</h1>
               <p>First text</p>
               </div>
          </div>
          <div class="carousel-item">
               <!--Mask color-->
               <div class="view">
               <img class="d-block w-100" height="600px" src="https://images.idgesg.net/images/article/2020/01/package-tracking-android-100827158-large.jpg" alt="Second slide">
               <div class="mask rgba-black-light"></div>
               </div>
               <div class="carousel-caption">
               <h1 class="h3-responsive" >See where is your car</h1>
               <p>Secondary text</p>
               </div>
          </div>
          <div class="carousel-item">
               <!--Mask color-->
               <div class="view">
               <img class="d-block w-100" height="600px" src="https://pictures.dealer.com/m/mercedesbenzofstcharles/0888/69faa4ca876fe2d10bee55c57cd1e0c1x.jpg" alt="Third slide">
               <div class="mask rgba-black-light"></div>
               </div>
               <div class="carousel-caption">
               <h1 class="h3-responsive">Fast access to BMW and Mercedes-benz dealers</h1>
               <p>Third text</p>
               </div>
          </div>
          </div>
          <!--/.Slides-->
          <!--Controls-->
          <a class="carousel-control-prev" href="#carousel-example-2" role="button" data-slide="prev">
          <span class="carousel-control-prev-icon" aria-hidden="true"></span>
          <span class="sr-only">Previous</span>
          </a>
          <a class="carousel-control-next" href="#carousel-example-2" role="button" data-slide="next">
          <span class="carousel-control-next-icon" aria-hidden="true"></span>
          <span class="sr-only">Next</span>
          </a>
          <!--/.Controls-->
          </div>
          <!--/.Carousel Wrapper-->
</template>

<script>
export default {
      watch: {
            //'$route' (to, from) {
              //    alert("to", to.params.id);
            //      alert("from", from.params.id);
          //  }
      },
      created() {
         ///   alert(this.$route.params.id);
      },
      methods: {
            goHome() {
                  this.$router.push('/');
            }
      }
}
</script>