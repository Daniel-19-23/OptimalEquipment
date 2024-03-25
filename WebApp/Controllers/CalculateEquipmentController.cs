using Microsoft.AspNetCore.Mvc;
using OptimalEquipment.Entities.Business;
using OptimalEquipment.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Contexts;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateEquipmentController : ControllerBase
    {
        private readonly ConnectionSQLServer _context;

        public CalculateEquipmentController(ConnectionSQLServer context)
        {
            _context = context;
        }

        // GET api/<CalculateEquipmentController>/GetMountainClimbingCalculation
        [HttpPost("GetMountainClimbingCalculation")]
        public ClimbingWrapper GetMountainClimbingCalculation([FromBody]ClimbingWrapper wr)
        {
            // Repositorios
            var repoEquipments = _context.Equipments;
            var repoClimbings = _context.Climbings;

            // Variables
            var maximumWeight = wr.MaximumWeight;
            var maximumCalories = wr.MaximumCalories;
            var equipments = wr.Equipments;

            // Objetos
            Climbing climbing = new()
            {
                Id = Guid.NewGuid(),
                MyKey = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                MaximumCalories = maximumCalories,
                MaximumWeight = maximumWeight,
            };

            repoClimbings.Add(climbing);

            // Bucle para el quipamiento
            foreach (var item in equipments)
            {
                item.Id = Guid.NewGuid();
                item.CreationDate = DateTime.Now;
                item.Name = item.Name;
                item.Calories = item.Calories;
                item.Weight = item.Weight;
                item.IsSelected = false;
                item.IdClimbing = climbing.Id;
            }

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
            var newWr = new ClimbingWrapper
            {
                MaximumWeight = bestWeight,
                MaximumCalories = bestCalories,
                Equipments = bestCombination
            };

            // Iterar sobre los equipamientos
            foreach (var item in equipments)
            {
                // Verificar si el equipamiento está presente en bestCombination
                if (bestCombination.Any(e => e.Id == item.Id))
                {
                    // Si está presente, establecer IsSelected en true
                    item.IsSelected = true;
                }

                repoEquipments.Add(item);
            }

            // Guardo los datos
            _context.SaveChanges();

            // Retorno el mejor resultado
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