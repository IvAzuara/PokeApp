// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function exportarExcel(btn) {
    const tabla = document.querySelector("table");
    const workbook = XLSX.utils.table_to_book(tabla, { sheet: "Pokédex" });

    const pagina = btn.dataset.page || 1;
    const search = btn.dataset.search;
    const nombre = search
        ? `pokedex_${search}.xlsx`
        : `pokedex_pagina_${pagina}.xlsx`;

    XLSX.writeFile(workbook, nombre);
}

// Autocomplete de especies
const speciesToggle = document.getElementById("speciesToggle");
const speciesPanel  = document.getElementById("speciesPanel");
const speciesSearch = document.getElementById("speciesSearch");
const speciesList   = document.getElementById("speciesList");

let debounceTimer;

if (speciesToggle) {

    // Abrir / cerrar
    speciesToggle.addEventListener("click", () => {
        const isOpen = speciesPanel.style.display === "block";
        speciesPanel.style.display = isOpen ? "none" : "block";

        if (!isOpen) {
            speciesSearch.value = "";
            fetchSpecies("");
            speciesSearch.focus();
        }
    });

    // Cerrar al hacer click fuera
    document.addEventListener("click", (e) => {
        if (!speciesToggle.contains(e.target) && !speciesPanel.contains(e.target))
            speciesPanel.style.display = "none";
    });

    // Buscar con debounce 300ms
    speciesSearch.addEventListener("input", () => {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => fetchSpecies(speciesSearch.value.trim()), 300);
    });
}

async function fetchSpecies(q) {
    renderLoading();
    try {
        const res   = await fetch(`/Pokemon/GetSpecies?q=${encodeURIComponent(q)}`);
        const items = await res.json();
        renderSpecies(items);
    } catch {
        renderError();
    }
}

function renderSpecies(items) {
    speciesList.innerHTML = "";

    if (items.length === 0) {
        speciesList.innerHTML = `<li class="list-group-item text-muted small">Sin resultados</li>`;
        return;
    }

    items.forEach(name => {
        const li = document.createElement("li");
        li.className = "list-group-item list-group-item-action text-capitalize";
        li.textContent = name;
        li.style.cursor = "pointer";
        li.addEventListener("click", () => {
            window.location.href = `/Pokemon/BySpecies?name=${encodeURIComponent(name)}`;
        });
        speciesList.appendChild(li);
    });
}

function renderLoading() {
    speciesList.innerHTML = `<li class="list-group-item text-muted small">Cargando...</li>`;
}

function renderError() {
    speciesList.innerHTML = `<li class="list-group-item text-danger small">Error al cargar</li>`;
}

function getPageSize() {
    return new URLSearchParams(window.location.search).get("pageSize") || 20;
}

// Enviar Excel por correo
async function enviarExcel(btn) {
    const tabla    = document.querySelector("table");
    const workbook = XLSX.utils.table_to_book(tabla, { sheet: "Pokédex" });

    const pagina = btn.dataset.page || 1;
    const search = btn.dataset.search;
    const nombre = search
        ? `pokedex_${search}.xlsx`
        : `pokedex_pagina_${pagina}.xlsx`;

    // Generar el archivo como base64 en lugar de descargarlo
    const base64 = XLSX.write(workbook, { bookType: "xlsx", type: "base64" });

    btn.disabled    = true;
    btn.textContent = "Enviando...";

    try {
        const res  = await fetch("/Pokemon/EnviarExcel", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ fileBase64: base64, fileName: nombre })
        });
        const data = await res.json();
        console.log("Respuesta:", data);

        btn.textContent = data.success ? "¡Enviado!" : "Error al enviar";
        if (!data.success) console.error("Error detallado:", data.detail);
        setTimeout(() => {
            btn.disabled    = false;
            btn.textContent = "Enviar por correo";
        }, 3000);
    } catch {
        btn.textContent = "Error al enviar";
        btn.disabled    = false;
    }
}
