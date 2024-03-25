using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalEquipment.Entities.Entities
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public int Weight { get; set; }
        public bool IsSelected { get; set; }
        public Guid IdClimbing { get; set; }

        // Constructor que establece los valores predeterminados
        public Equipment()
        {
            Id = Guid.NewGuid(); // Genera un nuevo GUID como valor predeterminado para Id
            CreationDate = DateTime.Now; // Establece la fecha y hora actual como valor predeterminado para CreationDate
            IdClimbing = Guid.NewGuid(); // Genera un nuevo GUID como valor predeterminado para IdClimbing
        }
    }
}
