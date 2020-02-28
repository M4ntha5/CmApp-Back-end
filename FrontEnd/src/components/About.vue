<template>
  <div class="container pt-5">
      <h1>About here</h1>
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