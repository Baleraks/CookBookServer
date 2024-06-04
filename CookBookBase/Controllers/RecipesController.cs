using CookBookBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        

        // POST: api/GetRecipes
        [HttpPost]
        [Route("api/GetRecipes")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByTags(PaginationQuery query)
        {
            List<Recipe> recipes = new();
            recipes = await _context.Recipes.ToListAsync();
            recipes = recipes.OrderByDescending(e => e.Id).ToList();

            if (query != null && query.Tags.Any())
            {
                List<Tag> tags = new();
                foreach (var item in query.Tags)
                {
                    tags.AddRange(_context.Tags.Where(x => x.Tagname == item));
                }
                List<Recipetotag> recipetotags = new();
                foreach (var item in tags)
                {
                    recipetotags.AddRange(_context.Recipetotags.Where(x => x.TagId == item.Id));
                }

                recipes.Clear();
                var a = new List<Recipetotag>();
                for (int i = 0; i < recipetotags.Count; i++)
                {
                    int count = 1;
                    for (int j = i + 1; j < recipetotags.Count; j++)
                    {
                        if (recipetotags[i].RecId == recipetotags[j].RecId
                            && recipetotags[i].TagId != recipetotags[j].TagId)
                        {
                            count++;
                        }
                    }

                    if (count == query.Tags.Count)
                    {
                        a.Add(recipetotags[i]);
                    }
                }

                foreach (var item in a)
                {
                    recipes.Add(item.Rec);
                }

                foreach (var item in recipes)
                {
                    item.Recipetotags = null;
                }
            }

            if(!string.IsNullOrEmpty(query.RecipeName))
            {
                recipes = recipes.Where((e => e.Recipename.Contains(query.RecipeName))).ToList();
            }

            if (query.Offset >= recipes.Count())
            {
                return BadRequest("Offset is out of range");
            }
            var RemainingCount = recipes.Count() - query.Offset;
            if (query.Count > RemainingCount)
            {
                query.Count = RemainingCount;
            }
            var RedactedRecipes = recipes.OrderByDescending(e => e.Id).ToList();
            RedactedRecipes = RedactedRecipes.Skip(query.Offset).Take(query.Count).ToList();

            return Ok(RedactedRecipes);
        }

        [HttpPost]
        [Route("api/GetRecipesByLikes")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByLikes(PaginationQuery query)
        {
            List<Recipe> recipes = new();
            recipes = await _context.Recipes.ToListAsync();
            recipes = recipes.OrderByDescending(e => e.Likes).ToList();

            if (query != null && query.Tags.Any())
            {
                List<Tag> tags = new();
                foreach (var item in query.Tags)
                {
                    tags.AddRange(_context.Tags.Where(x => x.Tagname == item));
                }
                List<Recipetotag> recipetotags = new();
                foreach (var item in tags)
                {
                    recipetotags.AddRange(_context.Recipetotags.Where(x => x.TagId == item.Id));
                }

                recipes.Clear();
                var a = new List<Recipetotag>();
                for (int i = 0; i < recipetotags.Count; i++)
                {
                    int count = 1;
                    for (int j = i + 1; j < recipetotags.Count; j++)
                    {
                        if (recipetotags[i].RecId == recipetotags[j].RecId
                            && recipetotags[i].TagId != recipetotags[j].TagId)
                        {
                            count++;
                        }
                    }

                    if (count == query.Tags.Count)
                    {
                        a.Add(recipetotags[i]);
                    }
                }

                foreach (var item in a)
                {
                    recipes.Add(item.Rec);
                }

                foreach (var item in recipes)
                {
                    item.Recipetotags = null;
                }
            }

            if (!string.IsNullOrEmpty(query.RecipeName))
            {
                recipes = recipes.Where((e => e.Recipename.Contains(query.RecipeName))).ToList();
            }

            if (query.Offset >= recipes.Count())
            {
                return BadRequest("Offset is out of range");
            }
            var RemainingCount = recipes.Count() - query.Offset;
            if (query.Count > RemainingCount)
            {
                query.Count = RemainingCount;
            }
            var RedactedRecipes = recipes.OrderByDescending(e => e.Likes).ToList();
            RedactedRecipes = RedactedRecipes.Skip(query.Offset).Take(query.Count).ToList();
            return Ok(RedactedRecipes);
        }

        [HttpPost]
        [Route("api/GetRecipesByReports")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByReports(PaginationQuery query)
        {
            List<Recipe> recipes = new();
            recipes = await _context.Recipes.ToListAsync();
            recipes = recipes.OrderByDescending(e => e.Reportsnum).ToList();

            if (query != null && query.Tags.Any())
            {
                List<Tag> tags = new();
                foreach (var item in query.Tags)
                {
                    tags.AddRange(_context.Tags.Where(x => x.Tagname == item));
                }
                List<Recipetotag> recipetotags = new();
                foreach (var item in tags)
                {
                    recipetotags.AddRange(_context.Recipetotags.Where(x => x.TagId == item.Id));
                }

                recipes.Clear();
                var a = new List<Recipetotag>();
                for (int i = 0; i < recipetotags.Count; i++)
                {
                    int count = 1;
                    for (int j = i + 1; j < recipetotags.Count; j++)
                    {
                        if (recipetotags[i].RecId == recipetotags[j].RecId
                            && recipetotags[i].TagId != recipetotags[j].TagId)
                        {
                            count++;
                        }
                    }

                    if (count == query.Tags.Count)
                    {
                        a.Add(recipetotags[i]);
                    }
                }

                foreach (var item in a)
                {
                    recipes.Add(item.Rec);
                }

                foreach (var item in recipes)
                {
                    item.Recipetotags = null;
                }
            }

            if (!string.IsNullOrEmpty(query.RecipeName))
            {
                recipes = recipes.Where((e => e.Recipename.Contains(query.RecipeName))).ToList();
            }

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
            RedactedRecipes = RedactedRecipes.OrderByDescending(e => e.Reportsnum).ToList();

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
            var Likes = _context.Likes.Where(e => e.RecId == id).ToList();
           

            for (int i = 0; i < Likes.Count(); i++)
            {
                Likes[i].Rec = null;
            }

            for (int i = 0; i< Steps.Count();i++)
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
            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe.Id);
        }

        // DELETE: api/Recipes/5
        [Authorize]
        [HttpDelete("api/DeleteRecipe")]
        public async Task<IActionResult> DeleteRecipe(DeleteModel model)
        {
            var recipeId = model.Id;
            var userId = model.UserId;
            var recipe = _context.Recipes.Find(recipeId);
            var User = _context.Users.Find(userId);
            if (recipe == null || User == null)
            {
                return NotFound();
            }
            if (model.UserId == recipe.UseId || User.Isadmin == true)
            {
                if (User.Isadmin == true)
                {
                    using (StreamWriter writer = new StreamWriter("wwwroot\\Ban.txt", true))
                    {
                        writer.WriteLine($"{recipe.UseId}/{recipe.Recipename}/{recipe.Id}");
                    }
                }
                var RecipeToIngridients = _context.Recipetoingridients.Where(e => e.RecId == model.Id).ToList();
                var RecipeToQauntities = new List<Recipetoqauntity>();
                var IngridientsToQauntities = new List<Ingridienttoqauntity>();

                for (int i = 0; i < RecipeToIngridients.Count(); i++)
                {
                    RecipeToQauntities.AddRange(_context.Recipetoqauntities.Where(e => e.RtoiId == RecipeToIngridients[i].Id).ToList());
                }
                for (int i = 0; i < RecipeToQauntities.Count; i++)
                {
                    _context.Recipetoqauntities.Remove(RecipeToQauntities[i]);
                }
                await _context.SaveChangesAsync();


                for (int i = 0; i < RecipeToIngridients.Count; i++)
                {
                    _context.Recipetoingridients.Remove(RecipeToIngridients[i]);
                }
                var RecipeToTag = _context.Recipetotags.Where(e => e.RecId == model.Id).ToList();
                for (int i = 0; i < RecipeToTag.Count; i++)
                {
                    _context.Recipetotags.Remove(RecipeToTag[i]);
                }
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                var Steps = _context.Steps.Where(e => e.RecId == model.Id).ToList();
                for (int i = 0; i < Steps.Count; i++)
                {
                    _context.Steps.Remove(Steps[i]);
                }

                var Comments = _context.Comments.Where(e => e.RecId == model.Id).ToList();
                for (int i = 0; i < Comments.Count; i++)
                {
                    _context.Comments.Remove(Comments[i]);
                }

                var Likes = _context.Likes.Where(e => e.RecId == model.Id).ToList();
                for (int i = 0; i < Likes.Count; i++)
                {
                    _context.Likes.Remove(Likes[i]);
                }

                var Reports = _context.Reports.Where(e => e.RecId == model.Id).ToList();
                for (int i = 0; i < Reports.Count; i++)
                {
                    _context.Reports.Remove(Reports[i]);
                }

                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else 
            { 
                return BadRequest("Permission denied"); 
            }
           
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
