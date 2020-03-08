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
import Test2 from './components/Modals/MultiCarousel.vue';
import Login from './components/Auth/Login.vue';
import Register from './components/Auth/Register.vue';
import Tracking from './components/Cars/Tracking.vue';


Vue.use(VueRouter);
Vue.config.productionTip = false

const routes = [ 
    { path: '/', component: Home },
    { path: '/cars', component: Cars },
    { path: '/cars/:id', component: ShowCar },
    { path: '/about', component: About },
    { path: '/create', component: Create },
    { path: '/repair2', component: Test2 },
    { path: '/login', component: Login },
    { path: '/register', component: Register },
    { path: '/cars/:id/tracking', component: Tracking },

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
