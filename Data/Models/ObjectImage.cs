namespace DiplomApi.Data.Models
{
    public class ObjectImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public virtual Object Object { get; set; }
    }
}
