import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import Cars from './components/Cars/Cars.vue';
import ShowCar from './components/Cars/Show.vue';
import Create from './components/Cars/Create.vue';
import About from './components/About.vue';
import Home from './components/Home.vue';


Vue.use(VueRouter);
Vue.config.productionTip = false

const routes = [ 
    { path: '/', component: Home },
    { path: '/cars', component: Cars },
    { path: '/cars/:id', component: ShowCar },
    { path: '/about', component: About },
    { path: '/create', component: Create },
    

 ];

 const router = new VueRouter({
   routes,
   mode:'history'
 });

new Vue({
  el: '#app',
  router,
  render: h =>h(App)
});
