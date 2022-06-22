using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Models.Tech;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_api_dev.Broken.Services.Technology
{
  public class TechnologyService
    {
        private readonly DbContextDatabase _database;
        private readonly IMapper _imapper;

        public TechnologyService(DbContextDatabase database, IMapper imapper = null)
        {
            _database = database;
            _imapper = imapper;
        }

        public async Task<JsonResult> ListTechnologies(int tech_id = 0)
        {
            if (tech_id == 0) {
                return new JsonResult(await _database.Technology.AsNoTracking().OrderBy(obj => obj.name).ToListAsync());
            } else {
                return new JsonResult(await _database.Technology.AsNoTracking().Where(obj => obj.id.Equals(tech_id)).ToListAsync());
            }
        }

        public async Task<ActionResult> NewTechnology(TechnologyPost tech)
        {
            if (await _database.Technology.FirstOrDefaultAsync(u => u.name.ToLower() == tech.name.ToLower()) != null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, $"{tech.name} já está cadastrado no sistema");
            }
            
            var newTech =_imapper.Map<Models.Tech.Technology>(tech);
            await _database.Technology.AddAsync(newTech);
            try {
                await _database.SaveChangesAsync();
                return TypeUtils.ReturnTypeResponseHTTP(true);
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }
    }
}