<template>
  <div>   
    <navbar/>  
    <router-view/>
    <foter/>
  </div>
</template>

<script>

import navbar from './components/Navigation/Navbar.vue';
import foter from './components/Navigation/Footer.vue';

export default {
  name: 'App',
  components: {
    navbar,
    foter
  }
}
</script>
