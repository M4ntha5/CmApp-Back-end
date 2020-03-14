<template>
     <div>
        <b-modal id="shipping-modal" ref="modal" title="Shipping info"
        @show="resetModal"
        @ok="handleOk"
        @close="resetModal">
        <b-alert v-model="alertFlag" dismissible>Inserting please wait...</b-alert>

            <form ref="form" @submit.stop.prevent="handleSubmit">

                <b-form-group :state="transportationFeeState" label="Transportation fee" label-for="transportationFee-input" 
                              invalid-feedback="Transportation fee is required and cannot be less than 0">  
                    <b-form-input v-model='insert.transportationFee' :state="transportationFeeState" 
                         id="transportationFee-input" type="number" min="0" step=".01" required>
                    </b-form-input>
                </b-form-group>

                <b-form-group :state="transferFeeState"  label="Transfer fee" label-for="transferFee-input" 
                              invalid-feedback="Transfer fee is required and cannot be less than 0">
                    <b-form-input v-model='insert.transferFee' :state="transferFeeState" 
                         id="transferFee-input" type="number" min="0" step=".01" required>
                    </b-form-input>
                </b-form-group>

                <b-form-group :state="auctionFeeState"  label="Auction fee" label-for="auctionFee-input" 
                              invalid-feedback="Auction fee is required and cannot be less than 0">
                    <b-form-input v-model='insert.auctionFee' :state="auctionFeeState" 
                         id="auctionFee-input" type="number" min="0" step=".01" required>
                    </b-form-input>
                </b-form-group>

                <b-form-group :state="customsState"  label="Customs" label-for="customs-input" 
                            invalid-feedback="Customs is required and cannot be less than 0">
                    <b-form-input v-model='insert.customs' :state="customsState" 
                         id="customs-input" type="number" min="0" step=".01" required>
                    </b-form-input>
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
                    customs: '',
                    auctionFee: '',
                    transferFee: '',
                    transportationFee: '',
                    car: this.$route.params.id
               },
               cars: [],
               car: {
                    id: '',
                    name: ''
               },
               customsState: null,
               auctionFeeState: null,
               transferFeeState: null,
               transportationFeeState: null,
               alertFlag: false,
          }
     },
     methods: {
          checkFormValidity() {
            const valid = this.$refs.form.checkValidity()
            this.transportationFeeState = this.$refs.form[0].checkValidity()
            this.transferFeeState = this.$refs.form[1].checkValidity()
            this.auctionFeeState = this.$refs.form[2].checkValidity()
            this.customsState = this.$refs.form[3].checkValidity()
            return valid
          },
          resetModal() {
               this.insert.customs = ''
               this.insert.auctionFee = ''
               this.insert.transferFee = ''
               this.insert.transportationFee = ''
               this.insert.car = this.$route.params.id

               this.transportationFeeState = null
               this.transferFeeState = null
               this.auctionFeeState = null
               this.customsState = null
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
                    axios.post(backEndUrl + `/api/cars/${vm.insert.car}/shipping`, vm.insert)
                    .then(function (response) {
                         if(response.status == 200)
                         {
                              // Hide the modal manually
                              vm.$nextTick(() => {
                                   vm.$bvModal.hide('shipping-modal')
                              })
                         }
                    })
                    .catch(function (error) {
                         console.log(error);
                    });          
               }
          },
     }
}
</script>
