
<script setup>
import { computed, ref } from 'vue'
import { useCartStore } from '@/stores/cart'

const cart = useCartStore()

const orderNote = ref('')
// const promoCode = ref('')
// const promoApplied = ref(false)

const menuImages = {
  'jollof-rice': '/images/jollof.png',
  'fried-rice': '/images/fried-rice.png',
  'egusi-soup': '/images/egusi.png',
  'grilled-chicken': '/images/chicken-bowl.png',
  'pepper-soup': '/images/pepper-soup.png',
  spaghetti: '/images/spag.png'
}

function getMenuImage(name) {
  const key = name?.trim().toLowerCase().replace(/[\s-]+/g, '-')
  return menuImages[key]
}

function formatPrice(price) {
  return new Intl.NumberFormat('en-NG', {
    style: 'currency',
    currency: 'NGN',
    maximumFractionDigits: 0
  }).format(price)
}

const subtotal = computed(() => cart.total)

const deliveryFee = 1200

const isFirstOrder = ref(true)

const discount = computed(() =>
  isFirstOrder.value ? subtotal.value * 0.10 : 0
)

const total = computed(() =>
  subtotal.value + deliveryFee - discount.value
)

// function applyPromo() {
//   if (promoCode.value.trim()) {
//     // Temporary demonstration.
//     // We will validate promo codes through the backend later.
//     promoApplied.value = true
//   }
// }
</script>

<template>
  <main class="cart-page">
    <div class="cart-container">

      <RouterLink to="/menu" class="back-link">
        ← Continue shopping
      </RouterLink>

      <header class="cart-header">
        <h1>Your cart</h1>
        <p>{{ cart.count }} items ready for checkout</p>
      </header>

      <section v-if="cart.items.length === 0" class="empty-cart">
        <div class="empty-icon">🛒</div>
        <h2>Your cart is empty</h2>
        <p>Discover something delicious on our menu.</p>
        <RouterLink to="/menu" class="continue-btn">
          Explore menu
        </RouterLink>
      </section>

      <section v-else class="cart-layout">

        <!-- Cart items -->
        <div class="cart-content">
          <article
            v-for="item in cart.items"
            :key="item.id"
            class="cart-item"
          >
            <div class="item-image">
              <img
                v-if="getMenuImage(item.name)"
                :src="getMenuImage(item.name)"
                :alt="item.name"
              />
              <span v-else>🍽️</span>
            </div>

            <div class="item-info">
              <p class="category">{{ item.category || 'Meal' }}</p>
              <h2>{{ item.name }}</h2>
              <p class="price">{{ formatPrice(item.price) }} each</p>

              <div class="quantity-control">
                <button
                  aria-label="Decrease quantity"
                  @click="cart.decrement(item.id)"
                >
                  −
                </button>

                <span>{{ item.quantity }}</span>

                <button
                  aria-label="Increase quantity"
                  :disabled="item.quantity >= Math.min(item.stock, 100)"
                  @click="cart.add(item)"
                >
                  +
                </button>
              </div>
            </div>

            <div class="item-right">
              <strong>
                {{ formatPrice(item.price * item.quantity) }}
              </strong>

              <button
                class="remove-btn"
                @click="cart.remove(item.id)"
              >
                Remove
              </button>
            </div>
          </article>

          <!-- Order note -->
          <div class="order-note">
            <label for="orderNote">
              Order note <span>Optional</span>
            </label>

            <textarea
              id="orderNote"
              v-model="orderNote"
              placeholder="Any special instructions for your order?"
              rows="3"
            ></textarea>
          </div>
        </div>

        <!-- Order summary -->
        <aside class="summary-card">
          <h2>Order summary</h2>

          <div class="summary-row">
            <span>Subtotal</span>
            <strong>{{ formatPrice(subtotal) }}</strong>
          </div>

          <div class="summary-row">
            <span>Delivery fee</span>
            <strong>{{ formatPrice(deliveryFee) }}</strong>
          </div>

        <div class="promo-section">
        <div class="promo-input">
            <input
            placeholder="Enter promo code"
            disabled
            />

            <button disabled>
            Apply
            </button>
        </div>
        </div>

          <div v-if="discount > 0" class="summary-row discount-row">
            <span>First order discount</span>
            <strong>−{{ formatPrice(discount) }}</strong>
          </div>

          <div class="summary-divider"></div>

          <div class="total-row">
            <strong>Total</strong>
            <strong>{{ formatPrice(total) }}</strong>
          </div>

          <button class="checkout-btn" disabled>
            Proceed to checkout
          </button>

          <p class="secure-note">
            <span>🔒</span>
            Secure payments powered by Bachs
          </p>
        </aside>

      </section>
    </div>
  </main>
</template>

<style scoped>
.cart-page {
  min-height: 100vh;
  background: #fcf5d8;
  color: #390d04;
  padding: 48px 6%;
}

.cart-container {
  max-width: 1150px;
  margin: 0 auto;
}

.back-link {
  display: inline-block;
  color: #806b63;
  text-decoration: none;
  font-size: 14px;
  margin-bottom: 32px;
}

.back-link:hover {
  color: #79a206;
}

.cart-header {
  margin-bottom: 36px;
}

.cart-header h1 {
  font-size: 36px;
  margin: 0 0 8px;
}

.cart-header p {
  color: #806b63;
  margin: 0;
  font-size: 14px;
}

.cart-layout {
  display: grid;
  grid-template-columns: 1.6fr 1fr;
  gap: 40px;
  align-items: start;
}

.cart-content {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.cart-item {
  background: white;
  padding: 20px;
  border-radius: 14px;
  display: flex;
  gap: 20px;
  align-items: center;
}

.item-image {
  width: 135px;
  height: 135px;
  flex-shrink: 0;
  overflow: hidden;
  border-radius: 10px;
  background: #f4f0e7;
  display: grid;
  place-items: center;
  font-size: 32px;
}

.item-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-info {
  flex: 1;
  min-width: 0;
}

.category {
  color: #79a206;
  font-size: 12px;
  font-weight: 600;
  margin: 0 0 7px;
}

.item-info h2 {
  font-size: 19px;
  margin: 0 0 8px;
}

.price {
  color: #806b63;
  font-size: 13px;
  margin: 0 0 14px;
}

.quantity-control {
  display: flex;
  align-items: center;
  gap: 14px;
}

.quantity-control button {
  width: 29px;
  height: 29px;
  border: 1px solid #e5e0d8;
  background: white;
  border-radius: 6px;
  cursor: pointer;
  font-size: 17px;
}

.quantity-control button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.item-right {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 14px;
  align-self: stretch;
  justify-content: space-between;
}

.item-right strong {
  white-space: nowrap;
}

.remove-btn {
  border: none;
  background: none;
  color: #a33c2d;
  cursor: pointer;
  font-size: 12px;
}

.order-note {
  background: white;
  padding: 22px;
  border-radius: 14px;
}

.order-note label {
  display: block;
  font-size: 14px;
  font-weight: 600;
  margin-bottom: 12px;
}

.order-note label span {
  color: #a49a90;
  font-weight: 400;
  margin-left: 5px;
}

.order-note textarea {
  width: 100%;
  resize: vertical;
  border: 1px solid #e5e0d8;
  border-radius: 8px;
  padding: 12px;
  font: inherit;
  font-size: 13px;
  outline: none;
}

.order-note textarea:focus,
.promo-input input:focus {
  border-color: #79a206;
}

.summary-card {
  background: white;
  padding: 26px;
  border-radius: 14px;
  position: sticky;
  top: 24px;
}

.summary-card h2 {
  font-size: 19px;
  margin: 0 0 26px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  gap: 12px;
  margin-bottom: 20px;
  font-size: 14px;
}

.summary-row span {
  color: #806b63;
}

.summary-row strong {
  white-space: nowrap;
}

.promo-section {
  margin: 24px 0;
}

.promo-input {
  display: flex;
  gap: 10px;
}

.promo-input input {
  min-width: 0;
  flex: 1;
  padding: 12px;
  border: 1px solid #e5e0d8;
  border-radius: 7px;
  outline: none;
}

.promo-input button {
  padding: 0 18px;
  border: none;
  border-radius: 7px;
  background: #e8edcf;
  color: #390d04;
  font-weight: 600;
  cursor: pointer;
}

.promo-input button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.discount-row strong {
  color: #79a206;
}

.summary-divider {
  border-top: 1px solid #eee9e4;
  margin: 24px 0;
}

.total-row {
  display: flex;
  justify-content: space-between;
  font-size: 19px;
  margin-bottom: 24px;
}

.checkout-btn,
.continue-btn {
  display: block;
  width: 100%;
  padding: 15px;
  border: none;
  border-radius: 8px;
  background: #79a206;
  color: white;
  text-align: center;
  text-decoration: none;
  font-size: 14px;
  font-weight: 600;
}

.checkout-btn:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}

.secure-note {
  text-align: center;
  color: #806b63;
  font-size: 11px;
  margin: 16px 0 0;
}

.secure-note span {
  margin-right: 4px;
}

.empty-cart {
  background: white;
  text-align: center;
  padding: 70px 24px;
  border-radius: 14px;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 16px;
}

.empty-cart p {
  color: #806b63;
  margin-bottom: 24px;
}

.continue-btn {
  max-width: 200px;
  margin: 0 auto;
}

@media (max-width: 850px) {
  .cart-layout {
    grid-template-columns: 1fr;
    gap: 24px;
  }

  .summary-card {
    position: static;
  }
}

@media (max-width: 520px) {
  .cart-page {
    padding: 28px 16px;
  }

  .cart-header h1 {
    font-size: 29px;
  }

  .cart-item {
    padding: 13px;
    gap: 12px;
  }

  .item-image {
    width: 85px;
    height: 100px;
  }

  .item-info h2 {
    font-size: 15px;
  }

  .item-right strong {
    font-size: 13px;
  }

  .summary-card {
    padding: 20px;
  }
}
</style>
