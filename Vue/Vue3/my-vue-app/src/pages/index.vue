
<template>
  <div>
    <input type="text" v-model="msg" />
    {{ msg }}
  </div>
</template>

<script>
import { ref } from "vue";
export default {
  setup() {
    const msg = ref("Hello, Vue3");
    setTimeout(() => {
      msg.value = "Hello, World";
    }, 3000);
    return {
      msg,
    };
  },
};
</script>
