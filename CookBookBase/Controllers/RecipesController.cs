using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CookBookBase;
using CookBookBase.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace CookBookBase.Controllers
{
  
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly CookBookDbContext _context;
        IConfiguration _configuration;

        public RecipesController(CookBookDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Recipes
        [HttpGet]
        [Route("api/GetRecipes")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes([FromBody] PaginationQuery query)
        {
            var recipes = await _context.Recipes.ToListAsync();
            if (query.Offset >= recipes.Count())
            {
                return BadRequest("Offset is out of range");
            }
            var RemainingCount = recipes.Count() - query.Offset;
            if (query.Count > RemainingCount)
            {
                query.Count = RemainingCount;
            }
            var RedactedRecipes = recipes.Skip(query.Offset).Take(query.Count);
            return Ok(RedactedRecipes);
        }

        // GET: api/Recipes/5
        [HttpGet("api/GetRecipe/{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
        
            var IngridientToRecipe = _context.Recipetoingridients.Where(e => e.RecId == id).ToList();
            var RecipeToQauntities = new List<Recipetoqauntity>();
            var IngridientsToQauntities = new List<Ingridienttoqauntity>();
            var RecipeToTag = _context.Recipetotags.Where(e => e.RecId == id).ToList();
            for (int i = 0; i< IngridientToRecipe.Count(); i++)
            {
                RecipeToQauntities.AddRange(_context.Recipetoqauntities.Where(e => e.RtoiId == IngridientToRecipe[i].Id).ToList());
            }
            for(int i = 0; i < RecipeToQauntities.Count();i++)
            {
                IngridientsToQauntities.AddRange(_context.Ingridienttoqauntities.Where(e => e.Id == RecipeToQauntities[i].ItoqId).ToList());
            
            }
            var Ingridients = new List<Ingridient>();
            var Qauntities = new List<Qauntity>();
            var Tags = new List<Tag>();
            var Steps = _context.Steps.Where(e => e.RecId == id).ToList();

            for(int i = 0; i< Steps.Count();i++)
            {
                Steps[i].Rec = null;
            }

            for(int i = 0; i< RecipeToTag.Count(); i++)
            {
                Tags.Add(_context.Tags.Find(RecipeToTag[i].TagId));
                RecipeToTag[i].Rec = null;
                Tags[i].Recipetotags = null;
            }
            for(int i = 0; i< IngridientToRecipe.Count();i++)
            {
                Ingridients.Add(_context.Ingridients.Find(IngridientToRecipe[i].IngId));
                Qauntities.Add(_context.Qauntitys.Find(IngridientsToQauntities[i].QauId));
                IngridientToRecipe[i].Rec = null;
                Ingridients[i].Recipetoingridients = null;
                IngridientsToQauntities[i].Ing = null;
                IngridientsToQauntities[i].Recipetoqauntities = null;
                IngridientToRecipe[i].Recipetoqauntities = null;
                Qauntities[i].Ingridienttoqauntities = null;
            }
            //recipe.Recipetoingridients = IngridientToRecipe;

            return recipe;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("api/ChangeRecipe/{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        [Route("api/AddRecipe")]
        public async Task<ActionResult<Recipe>> PostRecipe(RedactedRecipe RedactedRecipe)
        {
            for(int i = 0;i<RedactedRecipe.Qauntities.Count(); i++)
            {
                var qauntity = new Qauntity()
                {
                    Type = RedactedRecipe.Qauntities[i]
                };
                var resultCheck = _context.Qauntitys
                  .Where(e => e.Type == qauntity.Type).FirstOrDefault();
                if (resultCheck == null)
                {
                    _context.Qauntitys.Add(qauntity);
                }
            }

            for(int i = 0;i<RedactedRecipe.Ingridients.Count();i++)
            {
                var ingridient = new Ingridient()
                {
                    Ingridienname = RedactedRecipe.Ingridients[i],
                    Ingridientcalories = RedactedRecipe.IngridientCalories[i]
                };
                var resultCheck = _context.Ingridients
                  .Where(e => e.Ingridienname == ingridient.Ingridienname).FirstOrDefault();
                if (resultCheck == null)
                {
                    _context.Ingridients.Add(ingridient);
                }
            }
            await _context.SaveChangesAsync();

            for (int i = 0;i<RedactedRecipe.Ingridients.Count();i++)
            {
                var qauntity = _context.Qauntitys
                  .Where(e => e.Type == RedactedRecipe.Qauntities[i]).FirstOrDefault();
                var ingridient = _context.Ingridients
                  .Where(e => e.Ingridienname == RedactedRecipe.Ingridients[i]).FirstOrDefault();

                var QauntityToIngridient = new Ingridienttoqauntity()
                {
                    IngId = ingridient.Id,
                    QauId = qauntity.Id
                };
                _context.Ingridienttoqauntities.Add(QauntityToIngridient);
            }

            for (int i = 0; i < RedactedRecipe.Tags.Count(); i++)
            {
                var tag = new Tag()
                {
                    Tagname = RedactedRecipe.Tags[i]
                };
                var resultCheck = _context.Tags
                  .Where(e => e.Tagname == tag.Tagname).FirstOrDefault();
                if (resultCheck == null)
                {
                    _context.Tags.Add(tag);
                }
            }
            var FileName = "\\images\\" + Guid.NewGuid().ToString() + ".jpg";
            System.IO.File.WriteAllBytes("wwwroot\\" + FileName, RedactedRecipe.MainImage);
            var recipe = new Recipe() 
            {
                Cokingtime = 0,
                Likes = 0,
                Recipecalories = RedactedRecipe.Recipecalories,
                Recipeimagepath = FileName,
                Recipename = RedactedRecipe.Recipename,
                Reportsnum = 0,
                UseId = RedactedRecipe.UserId
            };
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            var recipeId = _context.Recipes.Where(e => e.Recipeimagepath == FileName).FirstOrDefault().Id;

            for(int i = 0; i< RedactedRecipe.StepText.Count();  i++)
            {
                FileName = "\\images\\" + Guid.NewGuid().ToString() + ".jpg";
                System.IO.File.WriteAllBytes("wwwroot\\" + FileName, RedactedRecipe.StepImages[i]);
                var step = new Step()
                {
                    RecId = recipeId,
                    Stepimagepath = FileName,
                    Steptext = RedactedRecipe.StepText[i],
                    Steptime = 0
                };
                _context.Steps.Add(step);
            }

            for (int i = 0; i < RedactedRecipe.Tags.Count(); i++)
            {
                var tag = _context.Tags
                  .Where(e => e.Tagname == RedactedRecipe.Tags[i]).FirstOrDefault();
                var RecipeToTag = new Recipetotag()
                {
                    RecId = recipeId,
                    TagId = tag.Id
                };
                _context.Recipetotags.Add(RecipeToTag);
            }

            for (int i = 0; i < RedactedRecipe.Ingridients.Count(); i++)
            {
                var ingridient = _context.Ingridients
                  .Where(e => e.Ingridienname == RedactedRecipe.Ingridients[i]).FirstOrDefault();
                var RecipeToIngridient = new Recipetoingridient()
                {
                    RecId = recipeId,
                    IngId = ingridient.Id
                };
                _context.Recipetoingridients.Add(RecipeToIngridient);
            }
            await _context.SaveChangesAsync();

            for (int i = 0; i < RedactedRecipe.Ingridients.Count(); i++)
            {
                var qauntity = _context.Qauntitys
                 .Where(e => e.Type == RedactedRecipe.Qauntities[i]).FirstOrDefault();
                var ingridient = _context.Ingridients
                  .Where(e => e.Ingridienname == RedactedRecipe.Ingridients[i]).FirstOrDefault();

                var IngToQId = _context.Ingridienttoqauntities
                  .Where(e => e.IngId == ingridient.Id && e.QauId == qauntity.Id).FirstOrDefault().Id;

                var RecToIngId = _context.Recipetoingridients
                    .Where(e => e.IngId == ingridient.Id && e.RecId == recipeId).FirstOrDefault().Id;

                var RecipeToQauntities = new Recipetoqauntity()
                {
                   ItoqId = IngToQId,
                   RtoiId = RecToIngId
                };
                _context.Recipetoqauntities.Add(RecipeToQauntities);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [Authorize]
        [HttpDelete("api/DeleteRecipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
