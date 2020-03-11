import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import Vue from 'vue'
import App from './App.vue'
import BootstrapVue from 'bootstrap-vue'
import VueRouter from 'vue-router'
import Cars from './components/Cars/Cars.vue';
import ShowCar from './components/Cars/Show.vue';
import Edit from './components/Cars/Edit.vue';
import About from './components/About.vue';
import Home from './components/Home.vue';
import Login from './components/Auth/Login.vue';
import Register from './components/Auth/Register.vue';
import Tracking from './components/Cars/Tracking.vue';
import Converter from './components/ExcangeConverter/Converter.vue';
import InsertOther from './components/Cars/InsertOther.vue';


Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.config.productionTip = false

const routes = [ 
    { path: '/', component: Home },
    { path: '/cars', component: Cars },
    { path: '/cars/:id', component: ShowCar },
    { path: '/cars/:id/tracking', component: Tracking },
    { path: '/cars/:id/edit', component: Edit },
    { path: '/login', component: Login },
    { path: '/register', component: Register },
    { path: '/converter', component: Converter },
    { path: '/about', component: About },
    { path: '/not-implemented', component: InsertOther },

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
