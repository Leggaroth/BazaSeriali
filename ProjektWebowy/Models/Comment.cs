namespace ProjektWebowy.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        
        public int SerialId { get; set; }
        public virtual Serial Serial { get; set; }
    }
}
