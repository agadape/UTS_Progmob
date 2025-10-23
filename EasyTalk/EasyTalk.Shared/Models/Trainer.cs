using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTalk.Shared.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Expertise { get; set; } = "";
        public string Email { get; set; } = "";

        public ICollection<Course>? Courses { get; set; }
    }
}
