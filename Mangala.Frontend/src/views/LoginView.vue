<script setup>
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '@/api/client'
import { useAuthStore } from '@/stores/auth'
import { decodeJwt } from '@/utils/jwt'

const router = useRouter()
const route = useRoute()
const auth = useAuthStore()

const username = ref('')
const password = ref('')
const error = ref(route.query.error ? 'Google sign-in failed. Please try again.' : '')
const loading = ref(false)

const googleUrl = 'https://localhost:7001/api/auth/login-google'

async function submit() {
  error.value = ''
  if (!username.value.trim() || !password.value) {
    error.value = 'Enter your username and password.'
    return
  }
  loading.value = true
  try {
    const { data } = await api.post('/auth/login', {
      username: username.value.trim(),
      password: password.value
    })
    const token = data.data.token
    auth.setAuth(token, decodeJwt(token))
    router.push(route.query.redirect || '/menu')
  } catch (e) {
    if (e.response?.status === 429) error.value = 'Too many attempts. Wait a minute and try again.'
    else if (e.response?.status === 401) error.value = e.response.data?.message || 'Invalid username or password.'
    else error.value = 'Cannot reach the server. Is the API running?'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <main class="auth-wrap">
    <form class="auth-card" @submit.prevent="submit">
      <h1>Welcome back</h1>
      <p class="sub">Log in to order today's menu.</p>

      <label>Username
        <input v-model="username" autocomplete="username" placeholder="your username" />
      </label>
      <label>Password
        <input v-model="password" type="password" autocomplete="current-password" placeholder="••••••••" />
      </label>

      <p v-if="error" class="error">{{ error }}</p>

      <button class="btn btn-primary full" :disabled="loading">
        {{ loading ? 'Logging in…' : 'Log in' }}
      </button>

      <div class="divider"><span>or</span></div>

      <a :href="googleUrl" class="btn google full">
        <svg width="18" height="18" viewBox="0 0 48 48"><path fill="#EA4335" d="M24 9.5c3.5 0 6.6 1.2 9.1 3.6l6.8-6.8C35.8 2.4 30.3 0 24 0 14.6 0 6.5 5.4 2.6 13.2l7.9 6.1C12.4 13.6 17.7 9.5 24 9.5z"/><path fill="#4285F4" d="M46.5 24.5c0-1.6-.1-3.1-.4-4.5H24v9h12.7c-.6 3-2.3 5.5-4.8 7.2l7.5 5.8c4.4-4.1 7.1-10.1 7.1-17.5z"/><path fill="#FBBC05" d="M10.5 28.7A14.5 14.5 0 0 1 9.5 24c0-1.6.3-3.2.8-4.7l-7.9-6.1A24 24 0 0 0 0 24c0 3.9.9 7.5 2.6 10.8l7.9-6.1z"/><path fill="#34A853" d="M24 48c6.5 0 11.9-2.1 15.9-5.8l-7.5-5.8c-2.1 1.4-4.9 2.3-8.4 2.3-6.3 0-11.6-4.1-13.5-9.8l-7.9 6.1C6.5 42.6 14.6 48 24 48z"/></svg>
        Continue with Google
      </a>

      <p class="switch">New here? <RouterLink to="/register">Create an account</RouterLink></p>
    </form>
  </main>
</template>

<style scoped>
.auth-wrap { min-height: calc(100vh - 76px); display: grid; place-items: center; padding: 40px 24px; }
.auth-card { width: 100%; max-width: 420px; background: var(--white); border: 1px solid var(--border); border-radius: 24px; padding: 40px; box-shadow: var(--shadow); display: flex; flex-direction: column; gap: 16px; }
h1 { font-size: 32px; font-weight: 700; color: var(--green-900); }
.sub { color: var(--muted); margin-top: -8px; }
label { display: flex; flex-direction: column; gap: 6px; font-size: 14px; font-weight: 600; }
input { padding: 12px 14px; border: 1px solid var(--border); border-radius: 12px; font-size: 15px; font-family: inherit; background: var(--cream); }
input:focus { outline: 2px solid var(--green-600); background: var(--white); }
.error { background: #fdecec; color: #b3261e; padding: 10px 14px; border-radius: 10px; font-size: 14px; }
.full { width: 100%; justify-content: center; }
.btn:disabled { opacity: 0.6; cursor: not-allowed; }
.divider { display: flex; align-items: center; gap: 12px; color: var(--muted); font-size: 13px; }
.divider::before, .divider::after { content: ''; flex: 1; height: 1px; background: var(--border); }
.google { background: var(--white); border: 1px solid var(--border); color: var(--text); }
.google:hover { background: var(--cream); }
.switch { text-align: center; color: var(--muted); font-size: 14px; }
.switch a { color: var(--green-700); font-weight: 600; }
</style>