using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;

namespace POSUNO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext context;

        public ProductsController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await this.context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await this.context.Products.FindAsync(id);

            if (product == null)
            {
                return this.NotFound();
            }

            return product;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(product).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ProductExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            User user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == product.User.Id);

            if(user == null)
            {
                return BadRequest("Usuario no existe.");
            }

            product.User = user;

            this.context.Products.Add(product);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await this.context.Products.FindAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            this.context.Products.Remove(product);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ProductExists(int id)
        {
            return this.context.Products.Any(e => e.Id == id);
        }
    }
}
