using Microsoft.AspNetCore.Mvc;
using OptimalEquipment.Entities.Business;
using OptimalEquipment.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateEquipmentController : ControllerBase
    {
        // GET api/<CalculateEquipmentController>/GetMountainClimbingCalculation
        [HttpPost("GetMountainClimbingCalculation")]
        public MountainWrapper GetMountainClimbingCalculation([FromBody]MountainWrapper wr)
        {
            var maximumWeight = wr.MaximumWeight;
            var maximumCalories = wr.MaximumCalories;
            var equipments = wr.Equipments;

            // Filtrar los elementos que tienen un peso menor o igual al peso máximo
            var viableEquipments = equipments.Where(e => e.Weight <= maximumWeight).ToList();

            // Inicializar las variables para almacenar la combinación óptima de elementos
            var bestWeight = 0;
            var bestCalories = 0;
            var bestCombination = new List<Equipment>();

            // Calcular todas las combinaciones posibles de elementos
            foreach (var combination in GetCombinations(viableEquipments))
            {
                // Calcular el total de calorías y peso de la combinación actual
                var totalCalories = combination.Sum(e => e.Calories);
                var totalWeight = combination.Sum(e => e.Weight);

                // Verificar si la combinación actual cumple con el mínimo de calorías requeridas
                // y lleva el menor peso posible
                if (totalCalories >= maximumCalories && (bestCombination.Count == 0 || totalWeight < bestWeight))
                {
                    bestCombination = combination.ToList();
                    bestCalories = totalCalories;
                    bestWeight = totalWeight;
                }
            }

            // Crear un nuevo objeto MountainWrapper con la combinación óptima de elementos
            var newWr = new MountainWrapper
            {
                MaximumWeight = bestWeight,
                MaximumCalories = bestCalories,
                Equipments = bestCombination
            };

            return newWr;
        }

        // Método para generar todas las combinaciones posibles de elementos
        private IEnumerable<IEnumerable<Equipment>> GetCombinations(List<Equipment> equipments)
        {
            // Comienza un bucle para generar todas las combinaciones posibles de elementos.
            for (int i = 0; i < (1 << equipments.Count); i++)
            {
                // Usa LINQ para generar una secuencia de combinaciones para el índice actual.
                yield return from j in Enumerable.Range(0, equipments.Count)

                             // Filtra los elementos que deben estar presentes en la combinación actual.
                             where (i & (1 << j)) != 0

                             // Selecciona los elementos correspondientes a los índices que cumplen la condición.
                             select equipments[j];
            }
        }
    }
}