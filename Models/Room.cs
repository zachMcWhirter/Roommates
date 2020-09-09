using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Models
{
    // C# representation of the Room table
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxOccupancy { get; set; }
    }
}
