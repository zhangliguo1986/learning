<script>
import { useRouter } from "vue-router";
import { useStore } from "vuex";

export default {
  setup() {
    const router = useRouter();
    const store = useStore();
    console.log("store", store.state.name);
    store.commit("changeName", "我就是我，不一样的烟火");
    console.log("store", store.state.name);
    store.dispatch("changeVal");
    function go() {
      router.push({ path: "/my" });
    }
    return { go };
  },
};
</script>

<template>
  <img alt="Vue logo" src="./assets/logo.png" />
  <router-view></router-view>
  <router-link to="/">首页</router-link>
  <button @click="go">我的</button>

  获取 state 内的状态信息
  {{ this.$store.state }}
  获取 getters 内的属性
  {{ this.$store.getters.getName }}
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
