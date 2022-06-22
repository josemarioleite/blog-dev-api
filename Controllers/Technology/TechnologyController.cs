using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Broken;
using blog_api_dev.Broken.Services.Technology;
using blog_api_dev.Models.Tech;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Controllers.Technology
{
    [Route("api/v1/technology")]
    [ApiController]
    public class TechnologyController : ControllerBase
    {
        private readonly TechnologyService techService;

        public TechnologyController(DbContextDatabase database, IMapper mapper)
        {
            techService = new TechnologyService(database, mapper);
        }

        [HttpGet]
        public async Task<JsonResult> ListTechs([FromQuery]int tech_id = 0)
        {
            if (ModelState.IsValid)
            {
                return await techService.ListTechnologies(tech_id);
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewTech([FromBody]TechnologyPost tech)
        {
            if (ModelState.IsValid)
            {
                return await techService.NewTechnology(tech);
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }
    }
}