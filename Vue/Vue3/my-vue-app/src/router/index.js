//1、按需引用方法
import { createRouter, createWebHashHistory } from "vue-router"
//2、定义一些路由
const routes = [
    {
        // 每个路由都需要映射到一个组件
        path: "/",
        component: () => import("../pages/index.vue")
    }
]
//3、创建路由实例
const router = createRouter({
    routes,
    history: createWebHashHistory("./")
})

export default router