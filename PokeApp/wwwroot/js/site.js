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


// Colores por tipo de pokémon
const tipoColores = {
    fire: "#F08030",     water: "#6890F0",   grass: "#78C850",
    electric: "#F8D030", psychic: "#F85888", ice: "#98D8D8",
    dragon: "#7038F8",   dark: "#705848",    fairy: "#EE99AC",
    normal: "#A8A878",   fighting: "#C03028", flying: "#A890F0",
    poison: "#A040A0",   ground: "#E0C068",  rock: "#B8A038",
    bug: "#A8B820",      ghost: "#705898",   steel: "#B8B8D0"
};

const statNombres = {
    "hp": "HP", "attack": "Ataque", "defense": "Defensa",
    "special-attack": "Sp. Ataque", "special-defense": "Sp. Defensa",
    "speed": "Velocidad"
};

async function abrirModal(id) {
    const modal = new bootstrap.Modal(document.getElementById("pokemonModal"));
    
    // Limpiar contenido anterior y mostrar cargando
    document.getElementById("modalNombre").textContent  = "Cargando...";
    document.getElementById("modalSprite").src          = "";
    document.getElementById("modalTipos").innerHTML     = "";
    document.getElementById("modalStats").innerHTML     = "";
    modal.show();

    try {
        const res  = await fetch(`/Pokemon/GetStats?id=${id}`);
        const data = await res.json();

        // Nombre
        document.getElementById("modalNombre").textContent = data.name;

        // Sprite
        document.getElementById("modalSprite").src = data.sprite;
        document.getElementById("modalSprite").alt = data.name;

        // Color del header según tipo principal
        const colorPrincipal = tipoColores[data.types[0]] || "#6c757d";
        document.getElementById("modalHeader").style.backgroundColor = colorPrincipal;
        document.getElementById("modalHeader").style.color = "#fff";

        // Tipos como badges
        const tiposDiv = document.getElementById("modalTipos");
        data.types.forEach(tipo => {
            const badge = document.createElement("span");
            badge.className   = "badge me-1 text-capitalize";
            badge.textContent = tipo;
            badge.style.backgroundColor = tipoColores[tipo] || "#6c757d";
            tiposDiv.appendChild(badge);
        });

        // Barras de estadísticas
        const statsDiv = document.getElementById("modalStats");
        data.stats.forEach(s => {
            const pct  = Math.min(Math.round((s.value / 255) * 100), 100);
            const nombre = statNombres[s.name] || s.name;
            statsDiv.innerHTML += `
                <div class="d-flex align-items-center mb-2 gap-2">
                    <span style="width:100px; text-align:right; font-size:.85rem">${nombre}</span>
                    <span style="width:35px; text-align:right; font-size:.85rem">${s.value}</span>
                    <div class="progress flex-grow-1" style="height:10px">
                        <div class="progress-bar"
                             style="width:${pct}%; background-color:${colorPrincipal}">
                        </div>
                    </div>
                </div>`;
        });

    } catch {
        document.getElementById("modalNombre").textContent = "Error al cargar";
    }
}