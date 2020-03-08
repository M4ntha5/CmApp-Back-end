<template>
     <div class="container pt-5">
          <center><h1>Register</h1></center>
          <form @submit="checkForm" @submit.prevent="register">               
               <div class="form-group">
                    <label for="email">Email</label>
                    <input type="email" id="email" name="email" class="form-control" v-model="email" required >
               </div>
               <div class="form-group">
                    <label for="password">Password</label>
                    <input type="password" id="password" name="password" class="form-control" v-model="password" required>
               </div>
               <div class="form-group">
                    <label for="password">Repeat password</label>
                    <input type="password" id="password2" name="password2" class="form-control" v-model="password2" required>
               </div>

               <button type="submit" class="btn btn-primary">Register</button>
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
               password2: '',
               errors:''
          }
     }, 
     methods: {
          checkForm:function(e) 
          {
               if(this.email && this.password && this.password2 && this.password == this.password2)
               {
                    return this.register();
               }
               this.errors = [];
               e.preventDefault();
          },
          register() {
               
          }
     }
}
</script>