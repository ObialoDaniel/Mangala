<template>
  <footer class="footer">
    <div class="container grid">
      <div>
        <div class="brand">Mangala</div>
        <p>Thoughtfully made meals,<br />delivered with care.</p>
      </div>
      <div>
        <h4>EXPLORE</h4>
        <RouterLink to="/menu">Our menu</RouterLink>
        <RouterLink to="/orders">Track an order</RouterLink>
      </div>
      <div>
        <h4>COMPANY</h4>
        <a href="#">About us</a>
        <a href="#">Careers</a>
      </div>
      <div>
        <h4>GET IN TOUCH</h4>
        <a href="#">hello@mangala.ng</a>
        <a href="#">Mon–Sun, 9am–10pm</a>
      </div>
    </div>
    <div class="container copy">© 2026 Mangala Foods. All rights reserved.</div>
  </footer>
</template>

<style scoped>
.footer { background: var(--green-900); color: #cfe3d8; padding: 64px 0 24px; margin-top: 80px; }
.grid { display: grid; grid-template-columns: 2fr 1fr 1fr 1fr; gap: 32px; }
.brand { color: var(--white); font-size: 22px; font-weight: 700; margin-bottom: 12px; }
h4 { font-size: 12px; letter-spacing: 0.14em; margin-bottom: 16px; color: #9dbfae; }
a { display: block; margin-bottom: 10px; font-size: 14px; color: var(--white); }
.copy { border-top: 1px solid rgba(255, 255, 255, 0.12); margin-top: 40px; padding-top: 20px; font-size: 12px; }
@media (max-width: 760px) { .grid { grid-template-columns: 1fr 1fr; } }
</style>    