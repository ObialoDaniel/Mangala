
<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '@/api/client'
import { useCartStore } from '@/stores/cart'

const router = useRouter()
const cart = useCartStore()

const orders = ref([])
const loading = ref(true)
const error = ref('')
const activeTab = ref('active')
const expandedOrderId = ref(null)
const reorderingId = ref(null)
const actionMessage = ref('')

// Keep these names consistent with your menu images.
const menuImages = {
  'jollof-rice': '/images/jollof.png',
  'fried-rice': '/images/fried-rice.png',
  'egusi-soup': '/images/egusi.png',
  'grilled-chicken': '/images/chicken-bowl.png',
  'pepper-soup': '/images/pepper-soup.png',
  spaghetti: '/images/spag.png'
}

const orderStatusNames = [
  'Pending',
  'Confirmed',
  'Preparing',
  'Ready',
  'OutForDelivery',
  'Delivered',
  'Cancelled'
]

const trackingSteps = [
  'Pending',
  'Confirmed',
  'Preparing',
  'Ready',
  'OutForDelivery',
  'Delivered'
]

const paymentStatusNames = [
  'Pending',
  'Processing',
  'Paid',
  'Failed',
  'Cancelled',
  'Refunded',
  'PartiallyRefunded'
]

function getPaymentStatus(status) {
  if (typeof status === 'number') {
    return paymentStatusNames[status] ?? 'Unknown'
  }

  return String(status ?? 'Unknown')
}

function getStatus(status) {
  if (typeof status === 'number') {
    return orderStatusNames[status] ?? 'Unknown'
  }

  return String(status ?? 'Unknown')
}

function getStatusIndex(status) {
  return trackingSteps.indexOf(getStatus(status))
}

function isStepComplete(order, index) {
  const currentIndex = getStatusIndex(order.status)

  return currentIndex >= 0 && index <= currentIndex
}

function isCurrentStep(order, index) {
  return getStatusIndex(order.status) === index
}

function getStatusClass(status) {
  return getStatus(status).toLowerCase()
}

function formatPrice(price) {
  return new Intl.NumberFormat('en-NG', {
    style: 'currency',
    currency: 'NGN',
    maximumFractionDigits: 0
  }).format(price)
}

function formatOrderReference(id) {
  return `MG-${String(id).padStart(5, '0')}`
}

function getItemCount(order) {
  return order.items?.reduce(
    (total, item) => total + item.quantity,
    0
  ) ?? 0
}

function formatDate(date) {
  return new Date(date).toLocaleDateString('en-NG', {
    day: 'numeric',
    month: 'short',
    year: 'numeric'
  })
}

function formatTime(date) {
  return new Date(date).toLocaleTimeString('en-NG', {
    hour: 'numeric',
    minute: '2-digit',
    hour12: true
  })
}

function formatOrderDate(date) {
  const orderDate = new Date(date)
  const today = new Date()

  const isToday =
    orderDate.getDate() === today.getDate() &&
    orderDate.getMonth() === today.getMonth() &&
    orderDate.getFullYear() === today.getFullYear()

  return isToday
    ? `Placed today at ${formatTime(date)}`
    : `Placed ${formatDate(date)} at ${formatTime(date)}`
}

function getMenuImage(name) {
  const key = name?.trim().toLowerCase().replace(/[\s-]+/g, '-')
  return menuImages[key]
}

function getFirstItem(order) {
  return order.items?.[0]?.menuItemName ?? 'Your meal'
}

function getRemainingItems(order) {
  const count = order.items?.length ?? 0
  return count > 1 ? `+ ${count - 1} more item${count > 2 ? 's' : ''}` : ''
}

const activeOrders = computed(() =>
  orders.value.filter(order =>
    !['Delivered', 'Cancelled'].includes(getStatus(order.status))
  )
)

const pastOrders = computed(() =>
  orders.value.filter(order =>
    ['Delivered', 'Cancelled'].includes(getStatus(order.status))
  )
)

const recentOrders = computed(() =>
  pastOrders.value.slice(0, 2)
)

const displayedOrders = computed(() =>
  activeTab.value === 'active'
    ? activeOrders.value
    : pastOrders.value
)

async function loadOrders() {
  loading.value = true
  error.value = ''

  try {
    const { data } = await api.get('/orders')
    orders.value = data.data ?? []
  } catch (err) {
    error.value =
      err.response?.data?.message ||
      "Couldn't load your orders. Please try again."
  } finally {
    loading.value = false
  }
}

function toggleDetails(orderId) {
  expandedOrderId.value =
    expandedOrderId.value === orderId ? null : orderId
}

async function orderAgain(order) {
  reorderingId.value = order.id
  actionMessage.value = ''

  try {
    // Fetch the latest menu to verify availability and stock.
    const { data } = await api.get('/menu/today')
    const menuItems = data.data ?? []

    cart.reconcile(menuItems)

    let addedCount = 0
    let skippedCount = 0

    for (const orderedItem of order.items ?? []) {
      const freshItem = menuItems.find(
        item => item.id === orderedItem.menuItemId
      )

      if (
        !freshItem ||
        !freshItem.isAvailable ||
        freshItem.quantity <= 0
      ) {
        skippedCount++
        continue
      }

      for (let i = 0; i < orderedItem.quantity; i++) {
        const currentQuantity = cart.qty(freshItem.id)

        if (currentQuantity >= Math.min(freshItem.quantity, 100)) {
          skippedCount++
          break
        }

        cart.add({
          id: freshItem.id,
          name: freshItem.name,
          price: freshItem.price,
          stock: freshItem.quantity
        })

        addedCount++
      }
    }

    if (addedCount === 0) {
      actionMessage.value = 'None of these meals are currently available.'
      return
    }

    if (skippedCount > 0) {
      actionMessage.value =
        'Some meals or quantities were unavailable and were skipped.'
    }

    await router.push('/cart')
  } catch {
    actionMessage.value = "Couldn't add these meals to your cart."
  } finally {
    reorderingId.value = null
  }
}

onMounted(loadOrders)
</script>

<template>
  <main class="orders-page">
    <div class="orders-container">

      <!-- Header -->
      <header class="page-header">
        <p class="eyebrow">YOUR MANGALA HISTORY</p>
        <h1>My orders</h1>
        <p class="subtitle">
          Track an active order or revisit a past favourite.
        </p>
      </header>

      <!-- Tabs -->
      <div class="tabs">
        <button
          :class="{ selected: activeTab === 'active' }"
          @click="activeTab = 'active'"
        >
          Active orders
          <span>{{ activeOrders.length }}</span>
        </button>

        <button
          :class="{ selected: activeTab === 'past' }"
          @click="activeTab = 'past'"
        >
          Past orders
        </button>
      </div>

      <!-- Loading -->
      <div v-if="loading" class="page-state">
        <p>Loading your orders...</p>
      </div>

      <!-- Error -->
      <div v-else-if="error" class="page-state error-state">
        <p>{{ error }}</p>
        <button @click="loadOrders">Try again</button>
      </div>

      <template v-else>

        <!-- Order again message -->
        <p v-if="actionMessage" class="action-message">
          {{ actionMessage }}
        </p>

        <!-- No orders -->
        <section
          v-if="displayedOrders.length === 0"
          class="empty-state"
        >
          <div class="empty-icon">📦</div>

          <h2>
            {{ activeTab === 'active'
              ? 'No active orders'
              : 'No past orders yet'
            }}
          </h2>

          <p>
            {{
              activeTab === 'active'
                ? "You don't have any orders being processed right now."
                : "Your completed orders will appear here."
            }}
          </p>

          <RouterLink to="/menu" class="menu-btn">
            Explore menu
          </RouterLink>
        </section>

        <!-- Active and past orders -->
        <template v-else>

          <!-- Active order cards -->
          <section
            v-for="order in displayedOrders"
            :key="order.id"
            class="order-card"
          >
            <div class="order-top">
              <div>
                <span
                  class="status-badge"
                  :class="getStatusClass(order.status)"
                >
                  {{ getStatus(order.status) }}
                </span>

                <h2>Order #{{ formatOrderReference(order.id) }}</h2>

                <p class="order-date">
                  {{ formatOrderDate(order.dateCreated) }}
                  · {{ getItemCount(order) }}
                  {{ getItemCount(order) === 1 ? 'item' : 'items' }}
                </p>
              </div>

              <strong class="order-total">
                {{ formatPrice(order.totalAmount) }}
              </strong>
            </div>

            <!-- Tracking timeline -->
            <div
              v-if="
                activeTab === 'active' &&
                getStatus(order.status) !== 'Cancelled'
              "
              class="timeline"
            >
              <div
                v-for="(step, index) in trackingSteps"
                :key="step"
                class="timeline-step"
                :class="{
                  completed: isStepComplete(order, index),
                  current: isCurrentStep(order, index)
                }"
              >
                <div class="timeline-marker">
                  <span v-if="isStepComplete(order, index)">✓</span>
                  <span v-else>{{ index + 1 }}</span>
                </div>

                <strong>{{ step }}</strong>
              </div>
            </div>

            <!-- Meal preview -->
            <div class="meal-preview">
              <div
                v-for="(item, index) in order.items?.slice(0, 2)"
                :key="item.menuItemId"
                class="preview-image"
              >
                <img
                  v-if="getMenuImage(item.menuItemName)"
                  :src="getMenuImage(item.menuItemName)"
                  :alt="item.menuItemName"
                />
                <span v-else>🍽️</span>
              </div>

              <div class="preview-details">
                <strong>{{ getFirstItem(order) }}</strong>
                <span v-if="getRemainingItems(order)">
                  {{ getRemainingItems(order) }}
                </span>
              </div>
            </div>

            <!-- Actions -->
            <div class="order-actions">
              <button
                class="details-btn"
                @click="toggleDetails(order.id)"
              >
                {{
                  expandedOrderId === order.id
                    ? 'Hide order details'
                    : 'View order details'
                }}
              </button>

              <button
                v-if="activeTab === 'past'"
                class="reorder-btn"
                :disabled="reorderingId === order.id"
                @click="orderAgain(order)"
              >
                {{ reorderingId === order.id ? 'Adding...' : 'Order again' }}
              </button>
            </div>

            <!-- Expanded details -->
            <div
              v-if="expandedOrderId === order.id"
              class="expanded-details"
            >
              <h3>Order items</h3>

              <div
                v-for="item in order.items"
                :key="item.menuItemId"
                class="detail-item"
              >
                <div>
                  <strong>{{ item.menuItemName }}</strong>
                  <p>
                    {{ item.quantity }} × {{ formatPrice(item.unitPrice) }}
                  </p>
                </div>

                <strong>{{ formatPrice(item.subtotal) }}</strong>
              </div>

              <div class="detail-total">
                <span>Order total</span>
                <strong>{{ formatPrice(order.totalAmount) }}</strong>
              </div>

              <div class="payment-status">
                <span>Payment status</span>
                <strong>{{ getPaymentStatus(order.paymentStatus) }}</strong>
              </div>
            </div>
          </section>

          <!-- Recent orders on active tab -->
          <section
            v-if="activeTab === 'active' && recentOrders.length"
            class="recent-section"
          >
            <div class="recent-heading">
              <div>
                <h2>Recent orders</h2>
                <p>Your previous purchases</p>
              </div>

              <button @click="activeTab = 'past'">
                View all →
              </button>
            </div>

            <article
              v-for="order in recentOrders"
              :key="order.id"
              class="recent-order"
            >
              <div class="recent-image">
                <img
                  v-if="getMenuImage(getFirstItem(order))"
                  :src="getMenuImage(getFirstItem(order))"
                  :alt="getFirstItem(order)"
                />
                <span v-else>🍽️</span>
              </div>

              <div class="recent-info">
                <strong>#{{ formatOrderReference(order.id) }}</strong>
                <p>
                  {{ formatDate(order.dateCreated) }}
                  · {{ getItemCount(order) }} items
                </p>
                <strong>{{ formatPrice(order.totalAmount) }}</strong>
              </div>

              <div class="recent-actions">
                <span
                  class="status-badge"
                  :class="getStatusClass(order.status)"
                >
                  {{ getStatus(order.status) }}
                </span>

                <button
                  :disabled="reorderingId === order.id"
                  @click="orderAgain(order)"
                >
                  {{ reorderingId === order.id ? 'Adding...' : 'Order again' }}
                </button>
              </div>
            </article>
          </section>

        </template>
      </template>
    </div>
  </main>
</template>

<style scoped>
.orders-page {
  min-height: 100vh;
  padding: 48px 6%;
  background: #fcf5d8;
  color: #390d04;
}

.orders-container {
  max-width: 1050px;
  margin: 0 auto;
}

.page-header {
  margin-bottom: 32px;
}

.eyebrow {
  color: #79a206;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 2px;
  margin-bottom: 10px;
}

.page-header h1 {
  font-size: 36px;
  margin: 0 0 8px;
}

.subtitle,
.order-date {
  color: #806b63;
  font-size: 14px;
}

.subtitle {
  margin: 0;
}

.tabs {
  display: flex;
  gap: 28px;
  border-bottom: 1px solid #e5ddc5;
  margin-bottom: 28px;
}

.tabs button {
  background: none;
  border: none;
  padding: 0 0 14px;
  color: #806b63;
  font: inherit;
  cursor: pointer;
  border-bottom: 2px solid transparent;
}

.tabs button.selected {
  color: #390d04;
  font-weight: 700;
  border-color: #79a206;
}

.tabs button span {
  margin-left: 5px;
  background: #e8edcf;
  color: #557400;
  padding: 3px 8px;
  border-radius: 20px;
  font-size: 11px;
}

.order-card {
  background: white;
  padding: 26px;
  border-radius: 14px;
  margin-bottom: 22px;
}

.order-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 16px;
}

.order-top h2 {
  font-size: 19px;
  margin: 12px 0 6px;
}

.order-date {
  margin: 0;
  font-size: 12px;
}

.order-total {
  font-size: 20px;
  white-space: nowrap;
}

.status-badge {
  display: inline-block;
  padding: 6px 11px;
  border-radius: 30px;
  font-size: 11px;
  font-weight: 700;
  background: #f0f0f0;
  color: #666;
}

.status-badge.pending {
  background: #fff1d5;
  color: #a56b08;
}

.status-badge.confirmed {
  background: #e8edcf;
  color: #557400;
}

.status-badge.shipped {
  background: #e5efff;
  color: #2863a5;
}

.status-badge.delivered {
  background: #e5f3e5;
  color: #27733c;
}

.status-badge.cancelled {
  background: #fbe7e5;
  color: #a33c2d;
}

.timeline {
  display: flex;
  justify-content: space-between;
  position: relative;
  margin: 38px 10px;
}

.timeline::before {
  content: '';
  position: absolute;
  height: 2px;
  background: #e8e4dc;
  left: 0;
  right: 0;
  top: 13px;
}

.timeline-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
  position: relative;
  z-index: 1;
  color: #a49a90;
  font-size: 12px;
}

.timeline-marker {
  width: 27px;
  height: 27px;
  border-radius: 50%;
  display: grid;
  place-items: center;
  background: #eeeae3;
  color: #806b63;
  border: 3px solid white;
  font-size: 11px;
}

.timeline-step.completed .timeline-marker {
  background: #79a206;
  color: white;
}

.timeline-step.current {
  color: #390d04;
}

.timeline-step.current strong {
  font-weight: 800;
}

.meal-preview {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 26px 0;
}

.preview-image {
  width: 66px;
  height: 66px;
  border-radius: 9px;
  overflow: hidden;
  background: #f4f0e7;
  display: grid;
  place-items: center;
  font-size: 24px;
}

.preview-image img,
.recent-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.preview-details {
  display: flex;
  flex-direction: column;
  gap: 6px;
  font-size: 13px;
}

.preview-details span {
  color: #806b63;
  font-size: 12px;
}

.order-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-top: 1px solid #eee9e4;
  padding-top: 18px;
  gap: 12px;
}

.details-btn,
.reorder-btn {
  border: none;
  background: none;
  color: #557400;
  font-weight: 700;
  font-size: 13px;
  cursor: pointer;
}

.reorder-btn {
  background: #79a206;
  color: white;
  padding: 11px 18px;
  border-radius: 7px;
}

.reorder-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.expanded-details {
  border-top: 1px solid #eee9e4;
  margin-top: 20px;
  padding-top: 20px;
}

.expanded-details h3 {
  font-size: 15px;
  margin: 0 0 16px;
}

.detail-item,
.detail-total,
.payment-status {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
  padding: 12px 0;
  font-size: 13px;
}

.detail-item p {
  margin: 5px 0 0;
  color: #806b63;
  font-size: 12px;
}

.detail-total,
.payment-status {
  border-top: 1px solid #eee9e4;
}

.recent-section {
  margin-top: 42px;
}

.recent-heading {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
}

.recent-heading h2 {
  font-size: 20px;
  margin: 0 0 5px;
}

.recent-heading p {
  color: #806b63;
  font-size: 12px;
  margin: 0;
}

.recent-heading button {
  background: none;
  border: none;
  color: #557400;
  font-weight: 700;
  cursor: pointer;
}

.recent-order {
  display: flex;
  align-items: center;
  gap: 16px;
  background: white;
  padding: 18px;
  border-radius: 12px;
  margin-bottom: 12px;
}

.recent-image {
  width: 76px;
  height: 76px;
  flex-shrink: 0;
  border-radius: 9px;
  overflow: hidden;
  background: #f4f0e7;
  display: grid;
  place-items: center;
  font-size: 24px;
}

.recent-info {
  flex: 1;
  min-width: 0;
}

.recent-info p {
  font-size: 12px;
  color: #806b63;
  margin: 6px 0;
}

.recent-info strong {
  font-size: 13px;
}

.recent-actions {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 10px;
}

.recent-actions button {
  border: none;
  background: none;
  color: #557400;
  font-weight: 700;
  cursor: pointer;
  font-size: 12px;
}

.recent-actions button:disabled {
  opacity: 0.5;
}

.page-state,
.empty-state {
  background: white;
  border-radius: 14px;
  padding: 50px 24px;
  text-align: center;
}

.error-state {
  color: #a33c2d;
}

.error-state button {
  border: none;
  background: #79a206;
  color: white;
  padding: 10px 18px;
  border-radius: 7px;
  cursor: pointer;
}

.empty-icon {
  font-size: 45px;
  margin-bottom: 12px;
}

.empty-state p {
  color: #806b63;
  margin-bottom: 24px;
}

.menu-btn {
  display: inline-block;
  background: #79a206;
  color: white;
  text-decoration: none;
  padding: 12px 22px;
  border-radius: 7px;
  font-weight: 600;
  font-size: 13px;
}

.action-message {
  color: #a33c2d;
  font-size: 13px;
  margin-bottom: 16px;
}

@media (max-width: 650px) {
  .orders-page {
    padding: 28px 16px;
  }

  .page-header h1 {
    font-size: 29px;
  }

  .order-card {
    padding: 18px;
  }

  .order-top {
    flex-direction: column;
  }

  .order-total {
    font-size: 18px;
  }

  .timeline {
    margin: 30px 0;
  }

  .timeline-step {
    font-size: 10px;
  }

  .recent-order {
    gap: 10px;
    padding: 12px;
  }

  .recent-image {
    width: 60px;
    height: 60px;
  }

  .recent-actions {
    gap: 8px;
  }
}

@media (max-width: 400px) {
  .timeline-step {
    font-size: 9px;
  }

  .timeline-marker {
    width: 24px;
    height: 24px;
  }

  .recent-actions .status-badge {
    font-size: 9px;
  }
}
</style>
