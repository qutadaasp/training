
namespace WebApplication6.Models
{
    public class Movies
    {
        public int id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string storeline { get; set; }
        public byte[] poster { get; set; }
        public byte Genreid { get; set; }
        public Genre Genre { get; set; }

       
    }
}
