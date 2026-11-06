<script setup>
import MenuItemCard from '@/components/MenuItemCard.vue'

const categories = [
  { name: 'Rice meals', count: 12, icon: '🍚' },
  { name: 'Soups & swallows', count: 9, icon: '🥣' },
  { name: 'Grills', count: 8, icon: '🍗' },
  { name: 'Healthy bowls', count: 7, icon: '🥗' }
]

const popular = [
  { name: 'Smoky Party Jollof', description: 'Firewood-style jollof rice with tender grilled chicken and plantain.', price: 4200, rating: 4.9, time: '25 min', image: '/images/jollof.png' },
  { name: 'Mangala Chicken Bowl', description: 'Herb-grilled chicken, seasoned rice, garden slaw and house sauce.', price: 5600, rating: 4.8, time: '30 min', image: '/images/chicken-bowl.png' },
  { name: 'Egusi & Pounded Yam', description: 'Rich melon seed soup with assorted meats and smooth pounded yam.', price: 4800, rating: 4.9, time: '35 min', image: '/images/egusi.png' }
]
</script>

<template>
  <!-- Hero -->
  <section class="hero container">
    <div class="hero-text">
      <span class="eyebrow">— Lagos' favourite kitchen</span>
      <h1>Good food,<br /><em>made easy.</em></h1>
      <p>Thoughtfully prepared meals made with fresh ingredients, delivered warm to your door.</p>
      <RouterLink to="/menu" class="btn btn-primary">Explore our menu →</RouterLink>
    </div>
    <div class="hero-img">
      <div class="circle" style="background-image: url('/images/hero.png')"></div>
      <div class="float rating">★ <b>4.9</b> Excellent rating</div>
      <div class="float delivery">⏱ <b>Fast delivery</b> 25–40 minutes</div>
    </div>
  </section>

  <!-- Categories -->
  <section class="container section">
    <span class="eyebrow">Find your craving</span>
    <h2>What are you in the mood for?</h2>
    <div class="cats">
      <RouterLink v-for="c in categories" :key="c.name" to="/menu" class="cat">
        <span class="cat-icon">{{ c.icon }}</span>
        <div>
          <strong>{{ c.name }}</strong>
          <small>{{ c.count }} dishes</small>
        </div>
        <span class="chev">›</span>
      </RouterLink>
    </div>
  </section>

  <!-- Popular -->
  <section class="container section">
    <span class="eyebrow">Our community favourites</span>
    <h2>Popular right now</h2>
    <div class="cards">
      <MenuItemCard v-for="d in popular" :key="d.name" v-bind="d" />
    </div>
  </section>

  <!-- Promo -->
  <section class="container section">
    <div class="promo">
      <div>
        <span class="tag">FIRST ORDER</span>
        <h2>Your first taste is on us.</h2>
        <p>Enjoy <b>20% off</b> your first order with code <code>HELLOMANGALA</code>.</p>
        <RouterLink to="/menu" class="btn btn-light">Order now →</RouterLink>
      </div>
      <div class="promo-ring">
            <div class="promo-plate" style="background-image: url('/images/bowl.png ')"></div>
        </div>
    </div>
  </section>
</template>

<style scoped>
.hero { display: grid; grid-template-columns: 1.1fr 1fr; align-items: center; gap: 40px; padding-top: 60px; padding-bottom: 40px; }
h1 { font-size: 76px; line-height: 1.02; font-weight: 700; color: var(--green-900); margin: 24px 0; letter-spacing: -0.03em; }
h1 em { color: var(--green-700); }
.hero-text p { color: var(--muted); font-size: 19px; line-height: 1.6; margin-bottom: 32px; max-width: 460px; }
.hero-img { position: relative; }
.circle { width: 100%; aspect-ratio: 1; border-radius: 50%; background-color: var(--green-100); background-size: cover; background-position: center; }
.float { position: absolute; background: var(--white); padding: 12px 18px; border-radius: 14px; box-shadow: var(--shadow); font-size: 13px; }
.rating { top: 12%; right: -4%; }
.delivery { bottom: 8%; left: -6%; }

.section { padding-top: 64px; }
h2 { font-size: 40px; font-weight: 500; margin: 12px 0 32px; }
.cats { display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px; }
.cat { display: flex; align-items: center; gap: 14px; background: var(--white); border: 1px solid var(--border); border-radius: var(--radius); padding: 20px; }
.cat small { display: block; color: var(--muted); font-size: 13px; margin-top: 4px; }
.cat-icon { width: 48px; height: 48px; border-radius: 12px; background: var(--green-100); display: grid; place-items: center; font-size: 22px; }
.chev { margin-left: auto; color: var(--green-700); font-size: 22px; }
.cards { display: grid; grid-template-columns: repeat(3, 1fr); gap: 24px; }

.promo { background: var(--green-700); color: var(--white); border-radius: 28px; padding: 64px; display: grid; grid-template-columns: 1.4fr 1fr; align-items: center; }
.promo h2 { font-size: 52px; font-weight: 500; max-width: 420px; margin: 16px 0; }
.promo p { margin-bottom: 24px; }
.tag { background: rgba(255, 255, 255, 0.16); padding: 6px 12px; border-radius: 8px; font-size: 11px; font-weight: 700; letter-spacing: 0.12em; }
code { background: rgba(0, 0, 0, 0.2); padding: 2px 8px; border-radius: 6px; }
.promo-ring {
  width: 300px;
  aspect-ratio: 1;
  border-radius: 50%;
  background: var(--orange);
  display: grid;
  place-items: center;
  justify-self: center;
}
.promo-plate {
  width: 82%;
  aspect-ratio: 1;
  border-radius: 50%;
  background-color: var(--white);
  background-size: cover;
  background-position: center;
  border: 6px solid var(--white);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.18);
}

@media (max-width: 900px) {
  .hero, .promo { grid-template-columns: 1fr; }
  h1 { font-size: 48px; }
  .cats { grid-template-columns: 1fr 1fr; }
  .cards { grid-template-columns: 1fr; }
  .promo { padding: 32px; }
}
</style>