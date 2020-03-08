<template>
     <div class="container pt-5">
          <center><h1>Login</h1></center>
          <form @submit="checkForm" @submit.prevent="login">          
               <div class="form-group">
                    <label for="email">Email</label>
                    <input type="email" id="email" name="email" class="form-control" v-model="email" required >
               </div>

               <div class="form-group">
                    <label for="password">Password</label>
                    <input type="password" id="password" name="password" class="form-control" v-model="password" required>
               </div>
               <button type="submit" class="btn btn-primary">Login</button>
          </form>
    </div>
</template>

<script>
//import axios from 'axios';

export default {
     data() {
          return {
               email: '',
               password: '',
               errors: []
          }
     }, 
     methods: {
          checkForm:function(e) 
          {
               if(this.email && this.password)
               {
                    return this.login();
               }
               this.errors = [];     
               e.preventDefault();
          },
          login() {
               
          }
     }
}
</script>