using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDBcontext dbcontext;
        private List<string> allowedmov = new List<string> { ".jpg", ".png" };
        private long maxposter = 1048576;
        public MoviesController(ApplicationDBcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpPost]
        public async Task<IActionResult> createMovie([FromForm] MovieDTo dto)
        {
            if(dto.poster == null)
            {
                return BadRequest("poster is required");
            }
            if (!allowedmov.Contains(Path.GetExtension(dto.poster.FileName).ToLower())) {
                return BadRequest("only .png or .jpg");
            }
            if (dto.poster.Length > maxposter)
            {
                return BadRequest($"max length : {maxposter}");
            }
            var isvalidgenre = await dbcontext.Genres.AnyAsync(g => g.Id == dto.Genreid);

            if (!isvalidgenre)
            {
                return BadRequest($"invalid id {dto.Genreid}");
            }

            if (dto == null || dto.poster == null || dto.poster.Length == 0)
            {
                return BadRequest("Invalid data");
            }

            using (var datastream = new MemoryStream())
            {
                await dto.poster.CopyToAsync(datastream);
                //var pos = datastream.ToArray();
                var movie = _mapper.Map<Movies>(dto);
                movie.poster = datastream.ToArray();
                //var movie = new Movies
                //{
                //    Genreid = dto.Genreid,
                //    Title = dto.Title,
                //    poster = pos,
                //    storeline = dto.storeline,
                //    year = dto.year,
                //    Rate = dto.Rate
                //};
                await dbcontext.AddAsync(movie);
                dbcontext.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> getmovies()
        {
            var mo = await dbcontext.Movies.ToListAsync();
            var data = _mapper.Map<IEnumerable<MovieDTo>>(mo);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getmovieid(int id)
        {
            var invalidid = await dbcontext.Genres.AnyAsync(g => g.Id == id);
            if (!invalidid)
            {
                return BadRequest($"invalid id : {id}");
            }

            var mov = await dbcontext.Movies.FindAsync(id);
            var dto = _mapper.Map<MovieDTo>(mov);
            //var dto = new Moviedto1
            //{
            //    id = mov.id,
            //    Genreid = mov.Genreid,
            //    Title = mov.Title,
            //    Genre = mov.Genre,
            //    //poster = mov.poster,
            //    storeline = mov.storeline,
            //    Rate = mov.Rate,
            //    year = mov.year
            //};
            return Ok(dto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> deletemovieasync(int id)
        {
            var invalidid = await dbcontext.Movies.FindAsync(id);
            if (invalidid == null)
            {
                return BadRequest("invalid id "+ id);
            }
            dbcontext.Remove(invalidid);
            dbcontext.SaveChanges();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updatemovies(int id, [FromForm] MovieDTo dto)
        {
            var isvalidgenre = await dbcontext.Genres.AnyAsync(g => g.Id == dto.Genreid);

            if (!isvalidgenre)
            {
                return BadRequest($"invalid id {dto.Genreid}");
            }
            var mov = await dbcontext.Movies.FindAsync(id);
            var invalidid = await dbcontext.Movies.AnyAsync(g => g.id == id);
            if (invalidid == null)
            {
                return BadRequest("");
            }
            if(dto.poster != null)
            {
                if (!allowedmov.Contains(Path.GetExtension(dto.poster.FileName).ToLower()))
                {
                    return BadRequest("only .png or .jpg");
                }
                if (dto.poster.Length > maxposter)
                {
                    return BadRequest($"max length : {maxposter}");
                }
            }
            using (var datastream = new MemoryStream())
            {
                await dto.poster.CopyToAsync(datastream);
                var pos = datastream.ToArray();

                var movie = new Movies
                {

                    Genreid = dto.Genreid,
                    Title = dto.Title,
                    poster = pos,
                    storeline = dto.storeline,
                    year = dto.year,
                    Rate = dto.Rate
                };
                await dbcontext.AddAsync(movie);
                dbcontext.SaveChanges();
            }
            //mov.id = id;
            //mov.Rate = dto.Rate;
            //mov.storeline = dto.storeline;
            //mov.year = dto.year;
            //mov.Genreid = dto.Genreid;
            //mov.Title = dto.Title;
            //mov.Genreid = dto.Genreid;
            return Ok();
        }
    }
}
