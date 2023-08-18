document.addEventListener("DOMContentLoaded", function () {
    const monthlyBtn = document.getElementById("monthlyBtn");
    const hourlyBtn = document.getElementById("hourlyBtn");
    const monthlyTable = document.getElementById("monthlyTable");
    const hourlyTable = document.getElementById("hourlyTable");

    monthlyBtn.addEventListener("click", function () {
        monthlyTable.style.display = "table";
        hourlyTable.style.display = "none";
    });

    hourlyBtn.addEventListener("click", function () {
        monthlyTable.style.display = "none";
        hourlyTable.style.display = "table";
    });
});