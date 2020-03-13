<template>
<div>
       <nav class="navbar navbar-expand-lg navbar-light bg-light" >
            <router-link to="/" class="navbar-brand">Big Logo</router-link>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                  <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                  <ul class="navbar-nav mr-auto">
                        <li class="nav-item">
                              <router-link to="/about" class="nav-link">About</router-link>
                        </li>
                        <li class="nav-item">
                              <router-link to="/cars" class="nav-link">Cars</router-link>
                        </li>
                        <li class="nav-item">
                              <router-link to="/converter" class="nav-link">Exchange converter</router-link>
                        </li>
                  </ul>
                  <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                              <a @click.prevent="showRepairModal" href v-b-modal.repair-insert-modal class="nav-link">Add repair</a>
                        </li>
                        <li class="nav-item">
                              <router-link to="/login" class="nav-link">Login</router-link>
                        </li>
                        <li class="nav-item">
                              <router-link to="/register" class="nav-link">Register</router-link>
                        </li>
                  </ul>
            </div>
      </nav>
      <!-- repair modal-->
      <RepairModal v-show="isRepairModalVisible" @click="closeRepairModal"/>
</div>
</template>
<script>
import RepairModal from '../Modals/RepairModal.vue';
export default {
      data() {
            return {
                  isRepairModalVisible: false,
            }
      },
      components: {
            RepairModal,
      },
      methods: {
            showRepairModal() {
                  this.isRepairModalVisible = true;
            },
            closeRepairModal() {
                  this.isRepairModalVisible = false;
            }
      }
}
</script>