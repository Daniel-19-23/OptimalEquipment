using OptimalEquipment.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalEquipment.Entities.Business
{
    public class MountainWrapper
    {
        public int MaximumWeight { get; set; }
        public int MaximumCalories { get; set; }
        public List<Equipment> Equipments { get; set; }
    }
}
