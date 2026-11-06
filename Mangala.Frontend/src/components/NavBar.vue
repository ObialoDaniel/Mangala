<script setup>
import { useAuthStore } from '@/stores/auth'
import { useCartStore } from '@/stores/cart'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const cart = useCartStore()
const router = useRouter()

function logout() {
  auth.logout()
  cart.clear()
  router.push('/')
}
</script>

<template>
  <header class="nav">
    <div class="container nav-inner">
      <RouterLink to="/" class="brand">
        <span class="logo-dot" style="background-image: url('./images/logo.png');"></span> Mangala
      </RouterLink>

      <nav class="links">
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/menu">Menu</RouterLink>
        <RouterLink to="/my-orders">My Orders</RouterLink>
      </nav>

      <div class="actions">
        <template v-if="!auth.isLoggedIn">
          <RouterLink to="/login" class="login-link">Log in</RouterLink>
          <RouterLink to="/register" class="btn btn-primary">Create account</RouterLink>
        </template>
        <button v-else class="login-link" @click="logout">Log out</button>

       <RouterLink to="/cart" class="cart">
          <span class="badge" v-if="cart.count > 0">{{ cart.count }}</span>
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-shopping-cart"><circle cx="9" cy="21" r="1"></circle><circle cx="20" cy="21" r="1"></circle><path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"></path></svg>
        </RouterLink>
      </div>
    </div>
  </header>
</template>

<style scoped>
.nav { background: var(--cream); border-bottom: 1px solid var(--border); position: sticky; top: 0; z-index: 10; }
.nav-inner { display: flex; align-items: center; justify-content: space-between; height: 76px; }
.brand { display: flex; align-items: center; gap: 10px; font-size: 24px; font-weight: 700; color: var(--green-900); }
.logo-dot { width: 38px; height: 38px; border-radius: 50%; background: var(--green-700); display: inline-block; }
.links { display: flex; gap: 32px; font-weight: 500; color: var(--green-700); }
.links a.router-link-exact-active { color: var(--green-900); border-bottom: 2px solid var(--green-700); padding-bottom: 6px; }
.actions { display: flex; align-items: center; gap: 20px; }
.login-link { background: none; color: var(--muted); font-size: 15px; }
.cart { position: relative; width: 46px; height: 46px; border-radius: 50%; background: var(--white); border: 1px solid var(--border); display: grid; place-items: center; }
.badge { position: absolute; top: -4px; right: -4px; background: var(--orange); color: var(--white); font-size: 11px; font-weight: 700; width: 20px; height: 20px; border-radius: 50%; display: grid; place-items: center; }
@media (max-width: 760px) { .links { display: none; } }
</style>