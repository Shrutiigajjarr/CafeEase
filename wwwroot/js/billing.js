(() => {
  const rows = [...document.querySelectorAll(".bill-menu-row")];
  const total = document.getElementById("billTotal");
  const summary = document.getElementById("summaryLines");
  if (!rows.length || !total || !summary) return;
  const money = value => new Intl.NumberFormat("en-IN", { style: "currency", currency: "INR" }).format(value);
  function update() {
    let amount = 0;
    const selected = [];
    rows.forEach(row => {
      const input = row.querySelector(".quantity-input");
      const qty = Math.max(0, Math.min(Number(input.value) || 0, Number(input.dataset.max) || 0));
      if (Number(input.value) !== qty) input.value = qty;
      const price = Number(row.dataset.price) || 0;
      if (qty > 0) {
        amount += qty * price;
        selected.push({ name: row.querySelector(".bill-menu-info strong").textContent, qty, line: qty * price });
      }
    });
    summary.innerHTML = selected.length
      ? selected.map(x => `<div class="summary-line"><span>${x.qty} × ${x.name.replace(/[&<>"]/g, c => ({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;"}[c]))}</span><strong>${money(x.line)}</strong></div>`).join("")
      : '<p class="text-muted">Choose menu quantities to begin.</p>';
    total.textContent = money(amount);
  }
  rows.forEach(row => row.querySelector(".quantity-input").addEventListener("input", update));
  update();
})();