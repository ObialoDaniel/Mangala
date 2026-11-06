import { createRouter, createWebHistory }  from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import CartView from '../views/CartView.vue'
import MyOrdersView from '../views/MyOrdersView.vue'

const routes = [
  { path: '/', name: 'home', component: () => import('@/views/HomeView.vue') },
  { path: '/login', name: 'login', component: () => import('@/views/LoginView.vue') },
  { path: '/register', name: 'register', component: () => import('@/views/RegisterView.vue') },
  { path: '/menu', name: 'menu', component: () => import('@/views/MenuView.vue')},
  { path: '/cart', name: 'cart', component: CartView }, 
  { path: '/orders', name: 'orders', component: () => import('@/views/OrdersView.vue'), meta: { requiresAuth: true } },
  { path: '/auth/callback', name: 'auth-callback', component: () => import('@/views/GoogleCallbackView.vue') },
  { path: '/my-orders', name: 'my-orders', component: MyOrdersView, meta: { requiresAuth: true }}
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.isLoggedIn) { 
    return { name: 'login', query: { redirect: to.fullPath } }
  }
})

export default router