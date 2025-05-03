function getPlants() {
    fetch('https://localhost:7107/api/noveny')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            console.log("Fetched plants data:", data);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('plant-form');
    const carePlanTable = document.getElementById('care-plan-table');

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        const plantData = document.getElementById('plantData').value.split('\n').map(line => {
            const [name, type, dailyWater, frequency] = line.split(',').map(item => item.trim());
            return { name, type, dailyWater: parseFloat(dailyWater), frequency: parseInt(frequency) };
        });
        displayCarePlan(plantData);
    });

    function displayCarePlan(plants) {
        const tbody = carePlanTable.querySelector('tbody');
        tbody.innerHTML = '';
        plants.forEach(plant => {
            const totalWater = plant.dailyWater * (7 / plant.frequency);
            const row = `<tr>
                <td>${plant.name}</td>
                <td>${plant.type}</td>
                <td>${plant.dailyWater}</td>
                <td>${plant.frequency}</td>
                <td>${totalWater.toFixed(2)}</td>
            </tr>`;
            tbody.innerHTML += row;
        });
    }
});