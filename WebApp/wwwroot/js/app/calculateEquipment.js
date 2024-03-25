console.log("Ready");

let countList = 0;      // Cuenta para los tr creados y eliminados
let countId = 0;        // Cuenta para los ID
let listEquipment = {}; // Lista de equipamento

function addEquipment() {

    // Capturar el valor del elemento por su ID
    let equipmentName = document.getElementById("equipmentName").value;
    let equipmentCalories = parseFloat(document.getElementById("equipmentCalories").value);
    let equipmentWeight = parseFloat(document.getElementById("equipmentWeight").value);

    // Validar NaN
    equipmentCalories = !isNaN(equipmentCalories) ? equipmentCalories : 0;
    equipmentWeight   = !isNaN(equipmentWeight)   ? equipmentWeight   : 0;

    // Capturar el elemento por su ID
    const equipmentNameAlert = document.getElementById("equipmentNameAlert");
    const equipmentCaloriesAlert = document.getElementById("equipmentCaloriesAlert");
    const equipmentWeightAlert = document.getElementById("equipmentWeightAlert");

    // Arreglos de datos
    let listValues = [equipmentName, equipmentCalories, equipmentWeight];               // Valores
    let listAlert = [equipmentNameAlert, equipmentCaloriesAlert, equipmentWeightAlert]; // Ubicación de sus alertas
    let listCase = [0, 1, 1];                                                           // Tipo de caso

    const isValid = validateInputs(listValues, listAlert, listCase);

    if (isValid) {

        // Equipamento
        let equipment = {
            name: equipmentName,
            calories: equipmentCalories,
            weight: equipmentWeight,
            isSelected: false,
        };

        // Inserta el objeto equipment en listEquipment, usando el id como clave
        listEquipment[countId] = equipment;

        // Capturar el elemento por su ID
        const listTr = document.getElementById('listTr');

        // Elemento tr
        let tr = 
            `<tr id="tr_${countId}" class="text-center">
            <td>${equipmentName}</td>
            <td>${equipmentCalories}</td>
            <td>${equipmentWeight}</td>
            <td>
                <button title='Eliminar' class="btn btn-danger" onclick="deleteEquipment(${countId})">X</button>
            </td>
        </tr>`;

        // Insertar HTML dentro del elemento
        listTr.innerHTML += tr;

        countList++; // Sumo a la cuenta de tr creados
        countId++;   // Sumo a la cuenta ID

        viewTable(); // Validar visibilidad de la tabla
    }
}

function deleteEquipment(number) {

    // Capturar el elemento por su ID
    const tr = document.getElementById(`tr_${number}`);

    // Eliminar el elemento del DOM
    tr.remove();

    // Eliminar el equipo con el id
    delete listEquipment[number];

    countList--; // Restar a la cuenta de tr creados

    viewTable(); // Validar visibilidad de la tabla
}

function viewTable() {

    // Capturar el elemento por su ID
    const tableEquipment = document.getElementById("tableEquipment");

    // Valido número de elementos tr existentes para hacer Visible o Invisible la tabla
    tableEquipment.style.display = countList > 0 ? "table" : "none";
}

function validateInputs(values, alerts, cases) {
    let status = true; // Estado del formulario

    // Itero sobre mis valores
    for (var i = 0; i < values.length; i++) {

        // Valido segun el caso, Tipo 0 para textos
        if (cases[i] === 0) {

            // Imprimir alerta
            alerts[i].style.display = values[i].length === 0 ? "block" : "none";
            status = values[i].length === 0 ? false : true;

        // Tipo 1 para números
        } else {

            // Imprimir alerta
            alerts[i].style.display = values[i] <= 0 ? "block" : "none";
            status = values[i] <= 0 ? false : true;
        }

        // Si se encontro un error, salgo del ciclo
        if (status === false) {
            break;
        }
    }

    return status;
}

function validateListEquipment(listEquipment) {

    // Contar cuántos objetos hay en listEquipment
    let count = Object.keys(listEquipment).length;

    // Capturar el elemento por su ID
    let listEquipmentAlert = document.getElementById("listEquipmentAlert");

    // ? Igual o más de 3 objetos en listEquipment : Hay menos de 3 objetos en listEquipment
    listEquipmentAlert.style.display = count >= 3 ? "none" : "block";

    return count >= 3 ? true : false;
}

function sendData() {
    let isValid = true;

    // Capturar el valor del elemento por su ID
    let maximumCalories = parseFloat(document.getElementById("maximumCalories").value);
    let maximumWeight = parseFloat(document.getElementById("maximumWeight").value);

    // Validar NaN
    maximumCalories = !isNaN(maximumCalories) ? maximumCalories : 0;
    maximumWeight   = !isNaN(maximumWeight)   ? maximumWeight   : 0;

    // Capturar el elemento por su ID
    let maximumCaloriesAlert = document.getElementById("maximumCaloriesAlert");
    let maximumWeightAlert = document.getElementById("maximumWeightAlert");

    // Arreglos de datos
    let listValues = [maximumCalories, maximumWeight];          // Valores
    let listAlert = [maximumCaloriesAlert, maximumWeightAlert]; // Ubicación de sus alertas
    let listCase = [1, 1];                                      // Tipo de caso

    isValid = validateInputs(listValues, listAlert, listCase)
    isValid = isValid ? validateListEquipment(listEquipment) : isValid;

    if (isValid) {

        // Convertir el objeto en un arreglo
        let listEquipmentArray = Object.values(listEquipment);

        listEquipmentArray = listEquipmentArray.map(equipment => ({
            name:         equipment.name,
            calories:     equipment.calories,
            weight:       equipment.weight,
            isSelected:   equipment.isSelected,
        }));

        let data = {
            maximumCalories: maximumCalories,
            maximumWeight: maximumWeight,
            equipments: listEquipmentArray,
        };

        //data = getDataStatic();

        fetch('api/CalculateEquipment/GetMountainClimbingCalculation', {
            method: 'POST', // Método HTTP utilizado para la solicitud
            headers: {
                'Content-Type': 'application/json', // Tipo de contenido que se enviará
            },
            body: JSON.stringify(data), // Convertimos en formato JSON
        })
        .then(response => {
            if (response.ok) {
                return response.json(); // Convertimos la respuesta a JSON si fue exitosa
            }
            throw new Error('La solicitud no fue exitosa.'); // Arrojamos un error si la solicitud falló
        })
        .then(data => {
            // Respuesta del servidor
            const result = document.getElementById("result");
            let listTr = "";

            // Usando un bucle for
            for (let i = 0; i < data.equipments.length; i++) {
                let equipment = data.equipments[i];

                listTr += `<tr><td>${equipment.name}</td><td>${equipment.calories}</td><td>${equipment.weight}</td></tr>`
            }

            result.innerHTML =
                `<tbody class="text-center">
                    <tr>
                        <td>Calorias</td>
                        <td>${data.maximumCalories}</td>
                    </tr>
                    <tr>
                        <td>Peso</td>
                        <td>${data.maximumWeight}</td>
                    </tr>
                    <tr>
                        <td style="text-align: center; vertical-align: middle;">Equipamiento</td>
                        <td>
                            <table class="table table-striped-columns table-hover table-borderless table-sm">
                                <thead>
                                    <tr>
                                        <th>Nombre</th>
                                        <th>Calorias</th>
                                        <th>Peso</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    ${listTr}
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>`
        })
        .catch(error => {
            console.error('Hubo un problema con la solicitud fetch:', error); // Aquí manejas los errores que puedan ocurrir
        });
    }
}

//function getDataStatic() {
//    return data = {
//        "maximumCalories": 15,
//        "maximumWeight": 10,
//        "equipments": [
//            {
//                "name": "E1",
//                "calories": 3,
//                "weight": 5,
//                "isSelected": false,
//            },
//            {
//                "name": "E2",
//                "calories": 5,
//                "weight": 3,
//                "isSelected": false,
//            },
//            {
//                "name": "E3",
//                "calories": 2,
//                "weight": 5,
//                "isSelected": false,
//            },
//            {
//                "name": "E4",
//                "calories": 8,
//                "weight": 1,
//                "isSelected": false,
//            },
//            {
//                "name": "E5",
//                "calories": 3,
//                "weight": 2,
//                "isSelected": false,
//            }
//        ]
//    }
//}