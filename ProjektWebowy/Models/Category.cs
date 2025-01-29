namespace ProjektWebowy.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Serial> Serials { get; set; }
    }
}
