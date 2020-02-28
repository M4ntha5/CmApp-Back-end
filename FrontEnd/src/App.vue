<template>
  <div>
    <navbar/>
    <router-view/>
  </div>
</template>

<script>
import navbar from './components/Navigation/Navbar.vue';

export default {
  name: 'App',
  components: {
    navbar
  }
}
</script>
