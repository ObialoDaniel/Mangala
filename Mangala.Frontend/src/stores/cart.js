import { defineStore } from 'pinia'

const KEY = 'cart'
const MAX_PER_ITEM = 100

export const useCartStore = defineStore('cart', {
    state: () => ({ 
        items: JSON.parse(localStorage.getItem(KEY) || '[]')
    }),
    getters: {
        count: (s) => s.items.reduce((n, i) => n + i.quantity, 0),
        total: (s) => s.items.reduce((sum, i) => sum + i.price * i.quantity, 0),
        qty: (s) => (id) => s.items.find((i) => i.id === id)?.quantity ?? 0
    },
    actions: {
        save() {
            localStorage.setItem(KEY, JSON.stringify(this.items))
        },
        add({ id, name, price, stock = 1 }) {
            const limit = Math.min(stock, MAX_PER_ITEM)
            const existing = this.items.find((i) => i.id === id)
            if (existing) {
                if (existing.quantity < limit) existing.quantity++
            } else if (limit > 0) {
                this.items.push({ id, name, price, stock, quantity: 1})
            }
            this.save()
        },
        decrement(id) {
            const item = this.items.find((i) => i.id === id)
            if (!item) return
            item.quantity--
            if (item.quantity <= 0) this.items = this.items.filter((i) => i.id !== id)
            this.save()
        },
        remove(id) {
            this.items = this.items.filter((i) => i.id !== id)
            this.save()
        },
        clear() {
            this.items = []
            this.save()
        },
        // Here we Sync the saved cart with today's real menu: drop items that are gone,
        // refresh prices and stock, and cap quantities.
        reconcile(menu) {
            this.items = this.items.map((cartItem) => {
                const fresh = menu.find((m) => m.id === cartItem.id)
                if (!fresh || !fresh.isAvailable || fresh.quantity <= 0) return null    
                const limit = Math.min(fresh.quantity, MAX_PER_ITEM)
                return {
                    ...cartItem,
                    name: fresh.name,
                    price: fresh.price,
                    stock: fresh.quantity,
                    quantity: Math.min(cartItem.quantity, limit)
                }
            })
            .filter(Boolean)
            this.save()
        }
   }
})
// reconcile matters because the cart is saved in localStorage and your menu changes daily. Without it, someone could come back tomorrow with yesterday’s items in their cart. 
// The cart prices are only for display. 
// The OrderService recalculates the real price from the database when the order is placed.