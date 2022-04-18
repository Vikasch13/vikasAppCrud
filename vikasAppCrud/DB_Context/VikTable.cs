using System;
using System.Collections.Generic;

#nullable disable

namespace vikasAppCrud.DB_Context
{
    public partial class VikTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? Mobileno { get; set; }
        public string Salary { get; set; }
    }
}
