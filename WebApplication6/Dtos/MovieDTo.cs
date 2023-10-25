namespace WebApplication6.Dtos
{
    public class MovieDTo
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string storeline { get; set; }
        public IFormFile? poster { get; set; }
        public byte Genreid { get; set; }
    }
}
