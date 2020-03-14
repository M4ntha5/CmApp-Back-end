<template>
    <div>
        <b-modal id="car-insert-modal" ref="modal" title="Insert new car"
        @show="resetModal"
        @ok="handleOk"
        @close="resetModal">
        <b-alert v-model="alertFlag" dismissible>Fetching data please wait...</b-alert>

            <form ref="form" @submit.stop.prevent="handleSubmit">
                <div class="mb-4">
                    <b-nav tabs>
                        <b-nav-item :active="activeBmwItem">
                            <a @click="bmwClick()">BMW</a>
                            </b-nav-item>
                        <b-nav-item :active="activeMbItem">
                            <a @click="mbClick()">Mercedes-benz</a>
                        </b-nav-item>
                        <b-nav-item>
                            <a @click="otherClick()">
                                Other
                            </a>
                        </b-nav-item>
                    </b-nav>
                </div>
                <b-form-group :state="vinState" label="Vin" label-for="vin-input" invalid-feedback="Vin is required">  
                    <b-form-input v-model='insert.vin' id="vin-input" :state="vinState" required></b-form-input>
                </b-form-group>

                <b-form-group :state="priceState"  label="Price" label-for="price-input" 
                            invalid-feedback="Price is required and should be more/equal 0">
                    <b-form-input v-model='insert.price' id="price-input" :state="priceState" type="number" min="0" step=".01" required></b-form-input>
                </b-form-group>

                <b-form-group label="Choose images" label-for="image-input">
                    <input  type="file" multiple @change="onFileSelected" id="image-input"/>       
                </b-form-group>

            </form>
        </b-modal>
    </div>
</template>

<script>
import axios from 'axios'
const backEndUrl = process.env.VUE_APP_BACK_END_URL;
export default {
    data() {
        return {
            insert: {
                vin: '',
                price: '',
                Base64images: [],
                make: 'BMW'
            },           
            priceState: null,
            vinState: null,
            alertFlag: false,
            activeBmwItem: true,
            activeMbItem: false,
        }
    },
    methods: {
        checkFormValidity() {
            const valid = this.$refs.form.checkValidity()
            this.vinState = this.$refs.form[0].checkValidity()
            this.priceState = this.$refs.form[1].checkValidity()
            return valid
        },
        resetModal() {
            this.insert.vin = ''
            this.insert.price = ''
            this.insert.Base64images = []
            this.insert.make = 'BMW'
            this.priceState = null
            this.vinState = null
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
                this.showAlert();
                // Push the name to submitted names
                let vm = this;
                axios.post(backEndUrl + '/api/cars', vm.insert)
                    .then(function (response) {             
                        if(response.status == 200)
                        {
                            console.log(response);
                            this.alertFlag = false;
                            // Hide the modal manually
                            vm.$nextTick(() => {
                                vm.$bvModal.hide('car-insert-modal')
                            })
                        }              
                    })
                    .catch(function (error) {
                            console.log(error);
                    });
            }
        },
        onFileSelected(e) {
            for(let i=0; i < e.target.files.length; i++)
            {
                var reader = new FileReader();
                reader.readAsDataURL(e.target.files[i]);
                reader.onload = (e) => {
                        this.insert.Base64images[i] = e.target.result;
                }                 
            }
            console.log(this.insert.Base64images);                
        }, 
        showAlert(){
            this.alertFlag = true;
        },
        bmwClick() {
            this.activeMbItem = false;
            this.activeBmwItem = true;
            this.insert.make = 'BMW'
        },
        mbClick() {
            this.activeMbItem = true;
            this.activeBmwItem = false;
            this.insert.make = 'Mercedes-benz'
        },
        otherClick() {
            window.location.href= "/not-implemented";
        }
  }
}
</script>