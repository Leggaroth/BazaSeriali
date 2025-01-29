using System.ComponentModel.DataAnnotations;

namespace ProjektWebowy.Models
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(1,10)]
        public int Value { get; set; } 

        
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

       
        public int SerialId { get; set; }
        public virtual Serial Serial { get; set; }
    }
}
