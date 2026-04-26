/* ═══════════════════════════════════════
   shared.js  —  FinTrack shared data & helpers
═══════════════════════════════════════ */

const CAT_META = {
    Food: { icon: '🍕', color: '#8b7cf8' },
    Transport: { icon: '⛽', color: '#f5b942' },
    Shopping: { icon: '🛍', color: '#f07060' },
    Entertainment: { icon: '🎬', color: '#34d9a5' },
    Health: { icon: '💊', color: '#e06fff' },
    Utilities: { icon: '💡', color: '#60b8f0' },
    Income: { icon: '💰', color: '#34d9a5' },
    Other: { icon: '📦', color: '#888880' },
};

const BUDGET_LIMITS = {
    Food: 10000, Transport: 6000, Shopping: 15000,
    Entertainment: 5000, Health: 4000, Utilities: 3000,
};

const fmt = d => d.toISOString().split('T')[0];
const fmt_inr = n => '₹' + Math.round(n).toLocaleString('en-IN');

function randDate(daysAgo) {
    const d = new Date(); d.setDate(d.getDate() - daysAgo); return fmt(d);
}

const SEED = [
    { id: 1, type: 'income', desc: 'Salary credit', cat: 'Income', amt: 78000, date: randDate(4) },
    { id: 2, type: 'expense', desc: 'Zomato order', cat: 'Food', amt: 450, date: randDate(1) },
    { id: 3, type: 'expense', desc: 'Petrol — Vasco', cat: 'Transport', amt: 1200, date: randDate(2) },
    { id: 4, type: 'expense', desc: 'Amazon — headphones', cat: 'Shopping', amt: 3499, date: randDate(3) },
    { id: 5, type: 'expense', desc: 'Netflix subscription', cat: 'Entertainment', amt: 649, date: randDate(5) },
    { id: 6, type: 'expense', desc: 'Pharmacy', cat: 'Health', amt: 820, date: randDate(6) },
    { id: 7, type: 'expense', desc: 'Electricity bill', cat: 'Utilities', amt: 2100, date: randDate(7) },
    { id: 8, type: 'expense', desc: 'Goa Brew Co.', cat: 'Entertainment', amt: 1200, date: randDate(8) },
    { id: 9, type: 'expense', desc: 'Grocery — Mapusa market', cat: 'Food', amt: 2800, date: randDate(9) },
    { id: 10, type: 'expense', desc: 'Rapido ride', cat: 'Transport', amt: 180, date: randDate(10) },
    { id: 11, type: 'expense', desc: 'Reliance Digital', cat: 'Shopping', amt: 5200, date: randDate(11) },
    { id: 12, type: 'expense', desc: 'Gym membership', cat: 'Health', amt: 2500, date: randDate(12) },
    { id: 13, type: 'income', desc: 'Freelance project', cat: 'Income', amt: 25000, date: randDate(13) },
    { id: 14, type: 'expense', desc: 'Swiggy breakfast', cat: 'Food', amt: 320, date: randDate(0) },
    { id: 15, type: 'expense', desc: 'Internet bill', cat: 'Utilities', amt: 999, date: randDate(14) },
];

function loadEntries() {
    try { const raw = sessionStorage.getItem('ft_entries'); return raw ? JSON.parse(raw) : SEED; }
    catch { return SEED; }
}
function saveEntries(entries) { sessionStorage.setItem('ft_entries', JSON.stringify(entries)); }
function getNextId(entries) { return entries.length ? Math.max(...entries.map(e => e.id)) + 1 : 1; }

/* ── Nav page definitions ── */
const NAV_PAGES = [
    {
        id: 'dashboard', label: 'Dashboard', href: '/Member/Index',
        icon: `<rect x="1" y="1" width="6" height="6" rx="1.5" fill="currentColor" opacity=".8"/><rect x="9" y="1" width="6" height="6" rx="1.5" fill="currentColor"/><rect x="1" y="9" width="6" height="6" rx="1.5" fill="currentColor"/><rect x="9" y="9" width="6" height="6" rx="1.5" fill="currentColor" opacity=".5"/>`,
    },
    {
        id: 'transactions', label: 'Transactions', href: '/Member/Transactions',
        icon: `<path d="M2 4h12M2 8h8M2 12h5" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"/>`,
    },
    {
        id: 'budget', label: 'Budget', href: '/Member/Budget',
        icon: `<circle cx="8" cy="8" r="6" stroke="currentColor" stroke-width="1.5"/><path d="M8 5v3l2 2" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"/>`,
    },
    {
        id: 'reports', label: 'Reports', href: '/Member/Reports',
        icon: `<path d="M2 14V9l4-4 3 3 5-6" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>`,
    },
    {
        id: 'logout', label: 'Logout', href: '/Home/Logout',
        icon: `<path d="M2 14V9l4-4 3 3 5-6" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>`,
    },
];

/* ── Desktop sidebar HTML ── */
function renderSidebar(activePage) {
    return `
  <aside class="sidebar">
    <div class="logo">
      <div class="logo-mark">₹</div>
      <div class="logo-text">FinTrack</div>
      <div class="logo-sub">v2.1 · dark pro</div>
    </div>
    <nav class="nav">
      <div class="nav-label">Menu</div>
      ${NAV_PAGES.map(p => `
        <a class="nav-item${p.id === activePage ? ' active' : ''}" href="${p.href}">
          <svg class="nav-icon" viewBox="0 0 16 16" fill="none">${p.icon}</svg>
          ${p.label}
        </a>`).join('')}
    </nav>
    <div class="sidebar-footer">
      <div class="avatar-row">
        <div class="avatar">${decodeURIComponent(document.cookie.split('; ').find(row => row.startsWith('MemberName='))?.split('=')[1] || '').split(' ').map(word => word.charAt(0).toUpperCase()).join('')}</div>
        <div>
          <div class="avatar-name">${document.cookie.split('; ').find(row => row.startsWith('MemberName=')).split('=')[1]}</div>
        </div>
      </div>
    </div>
  </aside>`;
}

/* ── Mobile drawer + bottom nav HTML ── */
function renderMobileNav(activePage) {
    return `
  <!-- Backdrop -->
  <div class="drawer-backdrop" id="drawerBackdrop" onclick="closeDrawer()"></div>

  <!-- Slide-in drawer -->
  <div class="mobile-drawer" id="mobileDrawer">
    <div class="drawer-logo">
      <div class="logo-mark" style="margin-bottom:8px;">₹</div>
      <div class="logo-text">FinTrack</div>
      <div class="logo-sub">v2.1 · dark pro</div>
    </div>
    <nav class="drawer-nav">
      <div class="nav-label">Menu</div>
      ${NAV_PAGES.map(p => `
        <a class="nav-item${p.id === activePage ? ' active' : ''}" href="${p.href}" onclick="closeDrawer()">
          <svg class="nav-icon" viewBox="0 0 16 16" fill="none">${p.icon}</svg>
          ${p.label}
        </a>`).join('')}
    </nav>
    <div class="drawer-footer">
      <div class="avatar-row">
        <div class="avatar">${decodeURIComponent(document.cookie.split('; ').find(row => row.startsWith('MemberName='))?.split('=')[1] || '').split(' ').map(word => word.charAt(0).toUpperCase()).join('')}</div>
        <div>
          <div class="avatar-name">${document.cookie.split('; ').find(row => row.startsWith('MemberName=')).split('=')[1]}</div>
        </div>
      </div>
    </div>
  </div>

  <!-- Bottom tab bar -->
  <nav class="bottom-nav">
    ${NAV_PAGES.map(p => `
      <a class="btab${p.id === activePage ? ' active' : ''}" href="${p.href}">
        <svg viewBox="0 0 16 16" fill="none">${p.icon}</svg>
        <span>${p.label}</span>
        <div class="btab-dot"></div>
      </a>`).join('')}
  </nav>`;
}

/* ── Drawer open/close ── */
function openDrawer() {
    document.getElementById('mobileDrawer').classList.add('open');
    document.getElementById('drawerBackdrop').classList.add('open');
    document.body.style.overflow = 'hidden';
}
function closeDrawer() {
    document.getElementById('mobileDrawer').classList.remove('open');
    document.getElementById('drawerBackdrop').classList.remove('open');
    document.body.style.overflow = '';
}

/* ── Add modal HTML ── */
function renderModal() {
    return `
  <div class="overlay" id="overlay" onclick="closeModal(event)">
    <div class="modal">
      <h3>New Entry</h3>
      <div class="form-row">
        <label>Type</label>
        <div class="type-toggle">
          <button class="type-btn active-neg" id="btnExpense" onclick="setType('expense')" value="expense">Expense −</button>
          <button class="type-btn"            id="btnIncome"  onclick="setType('income')" value="income">Income +</button>
        </div>
      </div>
      <div class="form-row">
        <label>Description</label>
        <input type="text" id="fDesc" placeholder="e.g. Zomato order" />
      </div>
      <div class="form-row">
        <label>Amount (₹)</label>
        <input type="number" id="fAmt" placeholder="0.00" min="0" inputmode="decimal" />
      </div>
      <div class="form-row">
        <label>Category</label>
        <select id="fCat">
          <option value="Food">🍕 Food</option>
          <option value="Transport">⛽ Transport</option>
          <option value="Shopping">🛍 Shopping</option>
          <option value="Entertainment">🎬 Entertainment</option>
          <option value="Health">💊 Health</option>
          <option value="Utilities">💡 Utilities</option>
          <option value="Income">💰 Income</option>
          <option value="Other">📦 Other</option>
        </select>
      </div>
      <div class="form-row">
        <label>Date</label>
        <input type="date" id="fDate" />
      </div>
      <div class="modal-actions">
        <button class="btn-cancel" onclick="closeModal()">Cancel</button>
        <button class="btn-save"   onclick="saveEntry()">Save Entry</button>
      </div>
    </div>
  </div>`;
}


/* ── Shared modal state & logic ── */
let _entryType = 'expense';

function openModal() {
    document.getElementById('fDate').value = fmt(new Date());
    document.getElementById('overlay').classList.add('open');
}
function closeModal(e) {
    if (!e || e.target === document.getElementById('overlay'))
        document.getElementById('overlay').classList.remove('open');
}
function setType(t) {
    _entryType = t;
    document.getElementById('btnExpense').className = 'type-btn' + (t === 'expense' ? ' active-neg' : '');
    document.getElementById('btnIncome').className = 'type-btn' + (t === 'income' ? ' active-pos' : '');
    if (t === 'income') document.getElementById('fCat').value = 'Income';
    else if (document.getElementById('fCat').value === 'Income') document.getElementById('fCat').value = 'Food';
}