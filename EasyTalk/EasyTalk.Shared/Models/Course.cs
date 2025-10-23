using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int DurationHours { get; set; }
        public int TrainerId { get; set; }

        //navigation property
        public Trainer? Trainer { get; set; }

        //  display helper
        public string? TrainerName { get; set; }

        
    }
}
