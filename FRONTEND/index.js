let url = 'https://localhost:7107/api/noveny';

function refreshPlants() {
    fetch(url)
        .then(response => response.json())
        .then(result => {
            console.log("plants:", result);
            displayPlants(result);
        })
        .catch(error => console.error(error));

    const requestOptions = {
        method: "GET",
        redirect: "follow"
    };

    fetch(`${url}/getweeklyplan`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            const dailyPlants = JSON.parse(result);
            console.log("dailyPlants:", dailyPlants);
            displayWeeklyPlan(dailyPlants);
        })
        .catch((error) => console.error(error));
}

function displayPlants(plants) {
    const tbody = document.getElementById("plant-table").querySelector('tbody');
    tbody.innerHTML = '';
    plants.forEach(p => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td style="font-weight: 600;">${p.nev}</td>
            <td>${p.kategoria}</td>
            <td>${p.napiVizigeny}</td>
            <td>${p.ontozesiGyakorisag}</td>
            <td>
                <button class="btn btn-warning btn-sm edit-btn">✏️</button>
                <button class="btn btn-danger btn-sm delete-btn">🗑️</button>
            </td>
        `;

        const editButton = row.getElementsByClassName('edit-btn')[0];
        const deleteButton = row.getElementsByClassName('delete-btn')[0];

        editButton.addEventListener('click', () => {
            editRow(row, p);
        });

        deleteButton.addEventListener('click', () => {
            deletePlant(p.id);
        });

        tbody.appendChild(row);
    });
}

function displayWeeklyPlan(dailyPlants) {
    const tableBody = document.getElementById("weekly-plan-table").querySelector('tbody');
    tableBody.innerHTML = "";

    dailyPlants.forEach((dailyPlan, index) => {
        if (index % 7 === 0) {
            row = document.createElement("tr");
            tableBody.appendChild(row);
        }

        const cell = document.createElement("td");
        cell.style = ``;

        cell.innerHTML = `<p class="cellDayNumber p-1 px-2 rounded" style="font-weight: 600; ${dailyPlan.napiOntozendoNovenyek.length >= 3 ? "background-color: rgb(255, 156, 156);" : ""}">${index + 1}</p>`;

        const plantList = dailyPlan.napiOntozendoNovenyek.map(plant =>
            `<div class="d-flex justify-content-between">
                <span class="fw-medium">${plant.nev}</span>
                <span class="text-info">${plant.napiVizigeny} mL</span>
            </div>`
        ).join("");

        const dayDiv = document.createElement("div");

        dayDiv.innerHTML = `
            <div>${plantList}</div>
            <hr class="${dailyPlan.napiVizigenySzum > 0 ? "d-flex" : "d-none"} my-1">
            <div class="${dailyPlan.napiVizigenySzum > 0 ? "d-flex" : "d-none"} justify-content-between">
                <span class="text-info">Összesen:</span>
                <span class="text-info font-weight-bold">${dailyPlan.napiVizigenySzum} mL</span>
            </div>
        `;

        cell.appendChild(dayDiv);
        row.appendChild(cell);
    });
}

function uploadMatrix() {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify(
        document.getElementById("matrix-input").value
    );

    const requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
    };

    fetch(`${url}/uploadmatrix`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            console.log(result);
            refreshPlants();
        })
        .catch((error) => console.error(error));
}


function editRow(row, plant) {
    row.innerHTML = `
        <td><input type="text" class="form-control plant-name" value="${plant.nev}"></td>
        <td>
            <select id="editCategoryDropdown" class="form-control" required>
                <option value="Virag" ${plant.kategoria === "Virag" ? "selected" : ""}>Virág</option>
                <option value="Szukkulens" ${plant.kategoria === "Szukkulens" ? "selected" : ""}>Szukkulens</option>
                <option value="FuszerNoveny" ${plant.kategoria === "Fuszernoveny" ? "selected" : ""}>Fűszernövény</option>
                <option value="Kertinoveny" ${plant.kategoria === "Kertinoveny" ? "selected" : ""}>Kertinövény</option>
                <option value="Szobanoveny" ${plant.kategoria === "Szobanoveny" ? "selected" : ""}>Szobanövény</option>
            </select>
        </td>
        <td><input type="number" class="form-control plant-daily-water" value="${plant.napiVizigeny}"></td>
        <td><input type="number" class="form-control plant-watering-frequency" value="${plant.ontozesiGyakorisag}"></td>
        <td>
            <button class="btn btn-success btn-sm save-btn">💾</button>
            <button class="btn btn-secondary btn-sm cancel-btn">✖️</button>
        </td>
    `;

    const saveButton = row.getElementsByClassName('save-btn')[0];
    const cancelButton = row.getElementsByClassName('cancel-btn')[0];

    saveButton.addEventListener('click', () => {
        updatePlant({
            id: plant.id,
            nev: row.querySelector('.plant-name').value,
            kategoria: row.querySelector('#editCategoryDropdown').value,
            napiVizigeny: parseFloat(row.querySelector('.plant-daily-water').value),
            ontozesiGyakorisag: parseInt(row.querySelector('.plant-watering-frequency').value),
        });
    });

    cancelButton.addEventListener('click', () => {
        refreshPlants();
    });
}

function deletePlant(id) {
    const requestOptions = {
        method: "DELETE",
        redirect: "follow"
    };

    fetch(`${url}/${id}`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            console.log(result);
            refreshPlants();
        })
        .catch((error) => console.error(error));
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

    fetch(`${url}/${plant.id}`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            console.log(result);
            refreshPlants();
        })
        .catch((error) => console.error(error));
}

function createPlant(event) {
    event.preventDefault();

    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
        nev: document.getElementById('name').value,
        kategoria: document.getElementById('category').value,
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
        .then((response) => response.text())
        .then((result) => {
            console.log(result);
            refreshPlants();
        })
        .catch((error) => console.error(error));
}

function getFunFact() {
    const requestOptions = {
        method: "GET",
        redirect: "follow"
    };

    fetch(`${url}/getfunfact`, requestOptions)
        .then((response) => response.text())
        .then((result) => {console.log(result);displayFunFact(result)})
        .catch((error) => console.error(error));
}

function displayFunFact(funFact) {
    const funFactElement = document.getElementById("fun-fact");
    funFactElement.innerText = funFact;
}