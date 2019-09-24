using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTelegramBot.Data;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDataRepository _repo;

        public WebController(IDataRepository repo, DataContext context) => 
            (_repo, _context) = (repo, context);

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesFromRepo = _context.Categories;

            return await Task.Run( () => 
            {
                return Ok(categoriesFromRepo);
            });
        }
    }
}