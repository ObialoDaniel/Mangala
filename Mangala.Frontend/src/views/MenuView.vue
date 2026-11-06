<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '@/api/client'
import { useCartStore } from '@/stores/cart';
import MenuItemCard from '@/components/MenuItemCard.vue'

const cart = useCartStore()
const items = ref([])
const loading = ref(true)
const error = ref('')
const activeCategory = ref('All')

const today = new Date().toLocaleDateString('en-NG', { weekday: 'long', day: 'numeric', month: 'long' })

const categories = computed(() => [
  'All',
  ...new Set(items.value.map((i) => i.categoryName).filter(Boolean))
])
const visible = computed(() => 
  activeCategory.value === 'All'
  ? items.value
  : items.value.filter((i) => i.categoryName === activeCategory.value)
)
const menuImages = {
  'jollof rice': '/images/jollof-rice.png',
  'fried rice': '/images/fried-rice.png',
  'egusi soup': '/images/egusi-soup.png',
  'grilled chicken': '/images/grilled-chicken.png',
  'pepper-soup': '/images/pepper-soup.png',
  'spaghetti': '/images/spag.png',
}

const getMenuImage = (name) => {
  const key = name?.trim().toLowerCase().replace(/[\s-]+/g, '-')
  return menuImages[key]
}

async function load() {
  loading.value = true
  error.value = ''

  try {
    const { data } = await api.get('/menu/today')

    items.value = data.data
    cart.reconcile(items.value)

  } catch {
    error.value = "Couldn't load today's menu items. Is the API running?"
  } finally {
    loading.value = false
  }
}
onMounted(load)
</script>
<template>
  <main class="container menu-page">
    <span class="eyebrow">Today . {{ today }}</span>
    <h1>Today's Menu</h1>

   <!-- Category filters -->
    <div v-if="categories.length > 2" class="chips">
      <button
        v-for="c in categories"
        :key="c"
        :class="['chip', { active: c === activeCategory }]"
        @click="activeCategory = c"
      >
        {{ c }}
      </button>
    </div>

    <!-- Menu items -->
    <p v-if="!items.length" class="state">
      Nothing on the menu yet today. Check back soon.
    </p>


    <div v-else-if="visible.length" class="cards">
      <MenuItemCard
        v-for="item in visible"
        :key="item.id"
        :id="item.id"
        :name="item.name"
        :description="item.description"
        :price="item.price"
        :stock="item.quantity"
        :available="item.isAvailable"
        :category="item.categoryName"
        :image="getMenuImage(item.name)"
      />
    </div>

    <p v-else class="state">
      No menu items found in this category.
    </p>

    <RouterLink v-if="cart.count > 0" to="/cart" class="cart-bar">
      <span>🛍 {{ cart.count }} {{ cart.count === 1 ? 'item' : 'items' }}</span>
      <strong>View cart · ₦{{ cart.total.toLocaleString() }}</strong>
    </RouterLink>
  </main>
</template>


<style scoped>
.menu-page { padding-top: 48px; padding-bottom: 120px; }
h1 { font-size: 44px; font-weight: 600; margin: 10px 0 24px; color: var(--green-900); }
.chips { display: flex; flex-wrap: wrap; gap: 10px; margin-bottom: 28px; }
.chip { padding: 9px 18px; border-radius: 999px; background: var(--white); border: 1px solid var(--border); font-weight: 600; font-size: 14px; color: var(--muted); }
.chip.active { background: var(--green-700); border-color: var(--green-700); color: var(--white); }
.cards { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; }
.state { text-align: center; color: var(--muted); padding: 80px 0; display: flex; flex-direction: column; align-items: center; gap: 16px; }
.cart-bar { position: fixed; left: 50%; bottom: 24px; transform: translateX(-50%); display: flex; align-items: center; justify-content: space-between; gap: 32px; min-width: min(480px, calc(100% - 32px)); background: var(--green-900); color: var(--white); padding: 16px 24px; border-radius: 16px; box-shadow: 0 10px 30px rgba(12, 61, 39, 0.35); z-index: 20; }
@media (max-width: 900px) { .cards { grid-template-columns: 1fr 1fr; } }
@media (max-width: 600px) { .cards { grid-template-columns: 1fr; } h1 { font-size: 32px; } }
</style>