using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektWebowy.Models
{
    public class Serial
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters long")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 2000 characters long")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2025, ErrorMessage = "Year must be between 1900 and the current year")]
        public int Year { get; set; }

        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        public double? AvgRating
        {
            get
            {
                return Ratings != null && Ratings.Any() ? Ratings.Average(r => r.Value) : 0;
            }
        }
    }
}
