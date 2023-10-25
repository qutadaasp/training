using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Dtos;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDBcontext _contextAccessor;

        public GenresController(ApplicationDBcontext contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _contextAccessor.Genres.OrderBy(g => g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> creates(Genresdtoes dto)
        {
            var dtosd = new Genre { Name = dto.name };
            await _contextAccessor.Genres.AddAsync(dtosd);
            _contextAccessor.SaveChanges();
            return Ok(dtosd);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateResult(int id, [FromBody] Genresdtoes dtosf)
        {
            var genre = await _contextAccessor.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if(genre == null)
            {
                return NotFound("not found in id "+ id);
            }
            genre.Name = dtosf.name;
            _contextAccessor.SaveChanges();
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletedata(int id)
        {
            var genre = await _contextAccessor.Genres.SingleOrDefaultAsync(g =>g.Id == id);
            if(genre == null)
            {
                return NotFound($" not found in {id}");
            }
            _contextAccessor.Genres.Remove(genre);
            _contextAccessor.SaveChanges();
            return Ok(genre);
        }
    }
}
