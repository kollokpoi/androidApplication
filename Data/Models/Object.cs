using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomApi.Data.Models
{
    public class Object
    {
        public int Id { get; set; }
        public virtual List<Apartament>? Apartaments { get; set; }
        public string Name { get; set; }

        public virtual List<ObjectImage> Images { get; set; } 
        public virtual Chat Chat { get; set; }

        [InverseProperty("Objects")]
        public virtual ICollection<User> Users { get; set; }
    }
}
