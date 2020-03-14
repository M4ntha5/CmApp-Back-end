<template>
     <div>
          <center class="pt-3"><h1>Here you can convert between most popular world currencies!</h1></center>
          
          <form class="container pt-5">
               <div class="row">
                    <div class="col-sm-5 col-12">
                         <div class="form-group">
                              <label for="exampleFormControlInput1">Enter amount</label>
                              <input type="number" class="form-control" required v-model="data.amount">
                         </div>
                         <div class="form-group ">
                              <label for="exampleInputEmail1">Currency from</label>
                              <select class="form-control" required v-model='data.from'>
                                   <option v-for='(rate, key) in rates' :key='key' :value='rate'>{{rate}}</option>
                              </select>
                         </div>                    
                    </div>
                    <div class="col-sm-2 col-12 pt-5">
                         <div class="form-group pt-5">
                         </div> 
                    </div>
                    <div class="col-sm-5 col-12">      
                         <div class="form-group">
                              <label>Currency to</label>
                              <select class="form-control" v-model='data.to' required>
                                   <option v-for='(rate, key) in rates' :key='key' :value='rate'>{{rate}}</option>
                              </select>
                         </div>  
                    </div>                     
               </div>
               <button @click.prevent="convert()" type="submit" class="btn btn-primary">Convert</button>
          </form>
          <div class="container mb-5">
               <center v-if="result" style="float:right;"><h2>Result: <b>{{result}} {{currTo}}</b></h2></center>
          </div>
     </div>
</template>

<script>
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
import axios from 'axios';

export default {
     data(){
          return{
               rates: [],
               data: {
                    from: 'EUR',
                    to: 'USD',
                    amount: ''
               },
               result: '',
               currTo: ''
          }
     },

     created() {
          this.getCurrencies();
     },

     methods: {
          getCurrencies() {
               let vm = this;
               axios.get(backEndUrl + '/api/currency')
               .then(function (response) {
                    vm.rates = response.data;
                    console.log(response.data);
               })
               .catch(function (error){
                    console.log(error)
               });
          },

          convert() {
               let vm = this;
               vm.currTo = vm.data.to;
               axios.post(backEndUrl + '/api/currency', this.data)
               .then(function (response) {
                    if(response.data)
                    {
                         vm.data.to = 'USD';
                         vm.data.from = 'EUR';
                         vm.result = response.data;
                    }
               })
               .catch(function (error){
                    console.log(error)
               });
          }

     }
}
</script>