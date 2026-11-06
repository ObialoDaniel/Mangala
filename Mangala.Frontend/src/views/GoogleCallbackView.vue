<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { decodeJwt } from '@/utils/jwt'

const router = useRouter()
const auth = useAuthStore()

onMounted(() => {
    const token = new URLSearchParams(window.location.hash.slice(1)).get('token')
    const user = token ? decodeJwt(token) : null
    if (!user) return router.replace({ name: 'login', query: { error: 'google'}})
    auth.setAuth(token, user)
    router.replace('/menu')     
})
</script>

<template>
  <main style="min-height: 60vh; display: grid; place-items: center">Signing you in…</main>
</template>