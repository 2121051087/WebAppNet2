// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('DOMContentLoaded', event => {
    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            // Lưu trạng thái của sidebar toggle vào localStorage
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }
});





console.log("DOM Loaded");
document.addEventListener("DOMContentLoaded", function () {
    console.log("Script running");
    const button = document.getElementById("addColorSize");
    if (button) {
        console.log("Button found");
        button.addEventListener("click", function () {
            console.log("Button clicked");
        });
    } else {
        console.error("Button not found");
    }
});
