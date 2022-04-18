using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vikasAppCrud.Models
{
    public class VikasModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? Mobileno { get; set; }
        public string Salary { get; set; }
    }
}
