<template>
  <div class="container pt-5">
      <h1>About here</h1>

      <img class="img-thumbnail" alt="Responsive image" width="350" height="300" src="https://cm-9b932d71-5bb4-4579-9d69-c61e6dbca245.s3.eu-central-1.amazonaws.com/system/db/images/cars/6aa28832-8c1b-48ad-b26e-a00eb4cb378c.png">
  </div>
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