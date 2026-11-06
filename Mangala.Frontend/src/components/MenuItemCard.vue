<script setup>
import { computed } from 'vue'
import { useCartStore } from '@/stores/cart'

const props = defineProps({
  id: Number,
  name: String,
  description: String,
  price: Number,
  rating: Number,
  time: String,
  image: String,
  stock: Number,
  category: String,
  available: { type: Boolean, default: true }
})

const cart = useCartStore()
const inCart = computed(() => (props.id === undefined ? 0 : cart.qty(props.id)))
const soldOut = computed(() => !props.available || (props.stock !== undefined && props.stock <= 0))
const limit = computed(() => Math.min(props.stock ?? 100, 100))

function add() {
  cart.add({
    id: props.id,
    name: props.name,
    price: props.price,
    stock: props.stock ?? 100
  })
}
</script>

<template>
  <article class="card">
    <div class="img" :style="{ backgroundImage: image ? `url(${image})` : 'none' }">
      <img v-if="image" :src="image" :alt="name" />
      <span v-else class="ph">🍽</span>
      
      <span v-if="time" class="time">⏱ {{ time }}</span>
      <span v-if="category" class="cat-tag">{{ category }}</span>
    </div>
    <div class="body">
      <div class="row">
        <h3>{{ name }}</h3>
        <span v-if="rating" class="rating">☆ {{ rating }}</span>
      </div>
      <p>{{ description }}</p>
      <small v-if="!soldOut && stock !== undefined && stock <= 5" class="low">Only {{ stock }} left</small>
      <div class="row foot">
        <strong>₦{{ price.toLocaleString() }}</strong>
        <RouterLink v-if="id === undefined" to="/menu" class="add">+ Add</RouterLink>
        <button v-else-if="soldOut" class="add" disabled>Sold out</button>
        <button v-else-if="inCart === 0" class="add" @click="add">+ Add</button>
        <div v-else class="stepper">
          <button @click="cart.decrement(id)" aria-label="Remove one">−</button>
          <span>{{ inCart }}</span>
          <button :disabled="inCart >= limit" @click="add" aria-label="Add one">+</button>
        </div>
      </div>
    </div>
  </article>
</template>

<style scoped>
.card { background: var(--white); border: 1px solid var(--border); border-radius: var(--radius); overflow: hidden; box-shadow: var(--shadow); }
.img {
  height: 220px;
  background-color: var(--green-100);
  position: relative;
  overflow: hidden;
  display: grid;
  place-items: center;
}

.img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}
.time { position: absolute; bottom: 12px; left: 12px; background: var(--white); border-radius: 20px; padding: 4px 10px; font-size: 12px; font-weight: 600; }
.cat-tag { position: absolute; top: 12px; left: 12px; background: var(--white); border-radius: 20px; padding: 4px 10px; font-size: 12px; font-weight: 600; color: var(--green-700); }
.body { padding: 18px; }
.ph { font-size: 48px; opacity: 0.5; }
.row { display: flex; justify-content: space-between; align-items: center; }
h3 { font-size: 18px; font-weight: 600; }
.rating { font-size: 13px; color: var(--muted); }
p { color: var(--muted); font-size: 14px; margin: 8px 0 18px; line-height: 1.5; }
.low { display: block; color: #b45309; font-size: 12px; font-weight: 600; margin-bottom: 10px; }
.foot strong { font-size: 20px; }
.add { display: inline-block; background: var(--green-100); color: var(--green-700); font-weight: 600; padding: 9px 16px; border-radius: 10px; font-size: 14px; }
.add:hover { background: var(--green-700); color: var(--white); }
.add:disabled { opacity: 0.5; cursor: not-allowed; }
.stepper { display: flex; align-items: center; gap: 12px; background: var(--green-100); border-radius: 10px; padding: 4px; }
.stepper button { width: 32px; height: 32px; border-radius: 8px; background: var(--white); color: var(--green-700); font-size: 18px; font-weight: 700; }
.stepper button:disabled { opacity: 0.4; cursor: not-allowed; }
.stepper span { min-width: 18px; text-align: center; font-weight: 700; }
</style>