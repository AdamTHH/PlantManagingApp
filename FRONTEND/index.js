let url = 'https://localhost:7107/api/noveny';

function getPlants() {
    fetch(url)
        .then(response => response.json())
        .then(data => {
            console.log("plants:", data);
            displayPlants(data);
        })
        .catch(error => console.error(error));
}

function displayPlants(plants) {
    const tbody = document.getElementById("plant-table").querySelector('tbody');
    tbody.innerHTML = '';
    plants.forEach(p => {
        const opacity = Math.min(Math.max(p.napiVizigeny / 5, 0), 1);

        const row = document.createElement('tr');
        row.innerHTML = `
            <td style="font-weight: 600;">${p.nev}</td> <!-- Semibold for plant name -->
            <td>${p.kategoria}</td>
            <td style="background-color: rgba(0, 0, 255, ${opacity});">${p.napiVizigeny}</td> <!-- Blue background with dynamic opacity -->
            <td>${p.ontozesiGyakorisag}</td>
            <td>
                <button class="btn btn-warning btn-sm edit-btn">✏️</button>
                <button class="btn btn-danger btn-sm delete-btn">🗑️</button>
            </td>
        `;

        const editButton = row.getElementsByClassName('edit-btn')[0];
        const deleteButton = row.getElementsByClassName('delete-btn')[0];

        editButton.addEventListener('click', () => {
            editStart(row, p);
        });

        deleteButton.addEventListener('click', () => {
            deletePlant(p.id);
        });

        tbody.appendChild(row);
    });
}

function editStart(row, plant) {
    row.innerHTML = `
        <td><input type="text" class="form-control plant-name" value="${plant.nev}"></td>
        <td><input type="number" class="form-control plant-category" value="${plant.kategoria}"></td>
        <td><input type="number" class="form-control plant-daily-water" value="${plant.napiVizigeny}" step="0.1"></td>
        <td><input type="number" class="form-control plant-watering-frequency" value="${plant.ontozesiGyakorisag}"></td>
        <td>
            <button class="btn btn-success btn-sm save-btn">💾</button>
            <button class="btn btn-secondary btn-sm cancel-btn">✖️</button>
        </td>
    `;

    const saveButton = row.getElementsByClassName('save-btn')[0];
    const cancelButton = row.getElementsByClassName('cancel-btn')[0];

    saveButton.addEventListener('click', () => {
        const updatedPlant = {
            id: plant.id,
            nev: row.querySelector('.plant-name').value,
            kategoria: parseInt(row.querySelector('.plant-category').value),
            napiVizigeny: parseFloat(row.querySelector('.plant-daily-water').value),
            ontozesiGyakorisag: parseInt(row.querySelector('.plant-watering-frequency').value),
        };
        updatePlant(updatedPlant);
    });

    cancelButton.addEventListener('click', () => {
        getPlants();
    });
}

function deletePlant(id) {
    fetch(`${url}/${id}`, {
        method: 'DELETE',
    })
        .then(r => {
            console.log(`ID=${id} deleted`);
            getPlants();
        })
        .catch(error => console.error(error));
}

function updatePlant(plant) {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify(plant);

    const requestOptions = {
        method: "PUT",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
    };

    fetch("https://localhost:7107/api/noveny/0", requestOptions)
        .then((response) => response.text())
        .then((result) => {
            console.log('Plant updated successfully');
            getPlants();
        })
        .catch((error) => console.error(error));
}

function createPlant(event) {
    event.preventDefault();

    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
        nev: document.getElementById('name').value,
        kategoria: parseInt(document.getElementById('category').value),
        napiVizigeny: parseFloat(document.getElementById('dailyWater').value),
        ontozesiGyakorisag: parseInt(document.getElementById('wateringFrequency').value)
    });

    console.log("adding:", raw);

    const requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
    };

    fetch(url, requestOptions)
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw new Error(`Error ${response.status}: ${errorData.message || response.statusText}`);
                });
            }
            return response.json();
        })
        .then(data => {
            console.log("Plant added successfully:", data);
            getPlants();
        })
        .catch(error => {
            console.error('Failed to add plant:', error.message);
            alert(`Failed to add plant: ${error.message}`);
        });
}