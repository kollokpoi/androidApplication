namespace DiplomApi.Data.Models
{
    public class Apartament
    {
        public int Id { get; set; }
        public virtual Object Object { get; set; }
        public int Floor { get; set; }
        public int ApartamentNumber { get; set; }
        public virtual ApartamentStatus Status { get; set; }

    }
}
