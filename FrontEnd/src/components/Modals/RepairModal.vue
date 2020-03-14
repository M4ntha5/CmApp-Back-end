<template>
     <div>
        <b-modal id="repair-insert-modal" ref="modal" title="Insert new repair"
        @show="resetModal"
        @ok="handleOk"
        @close="resetModal">
        <b-alert v-model="alertFlag" dismissible>Inserting please wait...</b-alert>

            <form ref="form" @submit.stop.prevent="handleSubmit">

                <b-form-group :state="nameState" label="Name" label-for="name-input" invalid-feedback="Name is required">  
                    <b-form-input v-model='insert.name' id="name-input" :state="nameState" required></b-form-input>
                </b-form-group>

                <b-form-group :state="priceState"  label="Price (€)" label-for="price-input" 
                            invalid-feedback="Price is required and should be more/equal 0">
                    <b-form-input v-model='insert.price' id="price-input" :state="priceState" type="number" min="0" step=".01" required></b-form-input>
                </b-form-group>

                <b-form-group :state="carState" label="Car" label-for="car-input"  
                            invalid-feedback="You must select one of the cars">
                    <b-form-select v-model="insert.car" required >
                         <b-form-select-option
                         v-for="car in cars" v-bind:key="car.id" :value="car.id">
                              {{car.name}}
                         </b-form-select-option>   
                    </b-form-select>
                </b-form-group>
            </form>
        </b-modal>
    </div>
</template>

<script>
import axios from'axios';
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
export default {
     data(){
          return {
               insert: {
                    name: '',
                    price: '',
                    car: ''
               },
               cars: [],
               car: {
                    id: '',
                    name: ''
               },
               nameState: null,
               priceState: null,
               carState: null,
               alertFlag: false,
          }
     },
     created() {
          this.getCarNames();
     },
     methods: {
          checkFormValidity() {
            const valid = this.$refs.form.checkValidity()
            this.nameState = this.$refs.form[0].checkValidity()
            this.priceState = this.$refs.form[1].checkValidity()
            this.carState = this.$refs.form[2].checkValidity()
            return valid
          },
          resetModal() {
               this.insert.name = ''
               this.insert.price = ''
               this.insert.car = this.cars[0].id
               this.priceState = null
               this.nameState = null
               this.carState = null
          },
          handleOk(bvModalEvt) {
               // Prevent modal from closing
               bvModalEvt.preventDefault()
               // Trigger submit handler
               this.handleSubmit()
          },
          handleSubmit() {
               // Exit when the form isn't valid
               if (!this.checkFormValidity()) {
                    return
               }
               else
               {
                    let vm = this;
                    vm.alertFlag = true;
                    axios.post(backEndUrl + `/api/cars/${vm.insert.car}/repairs`, vm.insert)
                    .then(function (response) {
                         if(response.status == 200)
                         {
                              // Hide the modal manually
                              vm.$nextTick(() => {
                                   vm.$bvModal.hide('repair-insert-modal')
                              })
                         }
                    })
                    .catch(function (error) {
                         console.log(error);
                    });          
               }
          },
          getCarNames() {
               let vm = this;
               axios.get(backEndUrl + '/api/user-car-names')
               .then(function (response) {
                    vm.cars = response.data;
                    vm.insert.car = vm.cars[0].id;
               })
               .catch(function (error) {
                    console.log(error);
               });
          }
     }
}
</script>