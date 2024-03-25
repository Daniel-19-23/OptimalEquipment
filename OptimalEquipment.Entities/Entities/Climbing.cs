using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalEquipment.Entities.Entities
{
    public class Climbing
    {
        public Guid Id { get; set; }
        public Guid MyKey { get; set; }
        public DateTime CreationDate { get; set; }
        public int MaximumCalories { get; set; }
        public int MaximumWeight { get; set; }
    }
}
