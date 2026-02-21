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

if (speciesToggle) {

    // Abrir / cerrar
    speciesToggle.addEventListener("click", () => {
        const isOpen = speciesPanel.style.display === "block";
        speciesPanel.style.display = isOpen ? "none" : "block";
        if (!isOpen) {
            speciesSearch.value = "";
            filterSpeciesOptions("");
            speciesSearch.focus();
        }
    });

    // Cerrar al hacer click fuera
    document.addEventListener("click", (e) => {
        if (!speciesToggle.contains(e.target) && !speciesPanel.contains(e.target))
            speciesPanel.style.display = "none";
    });

    // Filtrar opciones mientras escribe
    speciesSearch.addEventListener("input", () => {
        filterSpeciesOptions(speciesSearch.value.trim().toLowerCase());
    });

    // Navegar al seleccionar
    document.querySelectorAll(".species-option").forEach(li => {
        li.addEventListener("click", () => {
            const name = li.dataset.name;
            window.location.href = `?species=${encodeURIComponent(name)}&pageSize=${getPageSize()}`;
        });
    });
}

function filterSpeciesOptions(q) {
    document.querySelectorAll(".species-option").forEach(li => {
        const match = li.dataset.name.includes(q);
        li.style.display = match ? "block" : "none";
    });
}

function getPageSize() {
    return new URLSearchParams(window.location.search).get("pageSize") || 20;
}
