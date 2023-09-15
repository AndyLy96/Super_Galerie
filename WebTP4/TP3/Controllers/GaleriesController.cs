using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP3.Data;
using TP3.Models;

namespace TP3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GaleriesController : ControllerBase
    {
        readonly GalerieService service;

        readonly UserManager<User> userManager;

        private readonly TP3Context _context;

        public GaleriesController(TP3Context context, GalerieService _service, UserManager<User> _userManager)
        {
            _context = context;
            service = _service;
            userManager = _userManager;
        }

        // GET: api/Galeries
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Galerie>>> PublicGetGalerie()
        {

            //var galleries = await _context.Galerie.Where(g => g.IsPublic).ToListAsync();
            //var galleries = await service.GetPublicGaleries();

            if(service.IsEmpty())
            {
                return NotFound();
            }

            return await service.GetPublicGaleries();
            
        }

        // GET: api/Galeries/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Galerie>>> MyGetGaleries()
        {
          if (service.IsEmpty())
          {
              return NotFound();
          }
           

            //string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User user = await _context.Users.SingleAsync(u => u.Id == userid);
            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return user.Galeries;
        }

        // PUT: api/Galeries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGalerie(int id, Galerie galerie)
        {
            //string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User? user = await _context.Users.FindAsync(userid);
            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id != galerie.Id)
            {
                return BadRequest();
            }

            //Galerie? oldGalerie = await _context.Galerie.FindAsync(id);
            Galerie? oldGalerie = await service.GetGalerie(id);

            if(user == null || service.IsEmpty() || oldGalerie == null)
            {
                return NotFound();
            }

            if(!user.Galeries.Contains(galerie))
            {
                return Unauthorized(new { Message = "Not cool man" });
            }


            Galerie? newGalerie = await service.UpdateGalerie(id, galerie);

            //_context.Entry(galerie).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!GalerieExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Ok(new { Message = "Galerie nice" });
        }

        // POST: api/Galeries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Galerie>> PostGalerie(Galerie galerie)
        {
          if (service.IsEmpty())
          {
              return Problem("Entity set 'TP3Context.Galerie'  is null.");
          }

            //string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User? user = await _context.Users.FindAsync(id);
            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (user != null)
            {
                //galerie.User = new List<User>();
                //galerie.User.Add(user);
                //user.Galeries = new List<Galerie>();
                //user.Galeries.Add(galerie);
                //_context.Galerie.Add(galerie);
                //await _context.SaveChangesAsync();

                await service.CreateGalerie(user, galerie);
            }
            
            return CreatedAtAction("PostGalerie", new { id = galerie.Id }, galerie);
        }

        // DELETE: api/Galeries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGalerie(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User user = _context.Users.Single(u => u.Id == userid);
            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //var gallery = await _context.Galerie.FindAsync(id);
            var gallery = await service.GetGalerie(id);
            if (gallery == null)
            {
                return NotFound();
            }
           
            if (!gallery.User.Contains(user))
            {
                return Unauthorized(new {Message = "No work man"});
            }

            //_context.Galerie.Remove(gallery);
            //await _context.SaveChangesAsync();
            await service.DeleteGalerie(gallery);

            return Ok(new { Message = "It work man" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PorPGalerie(int id, Galerie galerie)
        {
            //string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User? user = await _context.Users.FindAsync(userid);
            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id != galerie.Id)
            {
                return BadRequest();
            }

            //Galerie? oldGalerie = await _context.Galerie.FindAsync(galerie.Id);
            Galerie? oldGalerie = await service.GetGalerie(id);

            if (user == null || service.IsEmpty() || oldGalerie == null)
            {
                return NotFound();
            }

            if (!user.Galeries.Contains(oldGalerie))
            {
                return Unauthorized(new { Message = "Not cool man" });
            }


            //oldGalerie.IsPublic = !oldGalerie.IsPublic;
            //await _context.SaveChangesAsync();
            await service.PorPGalerie(oldGalerie);
            await service.UpdateGalerie(id, oldGalerie);

            return Ok(new { Message = "Galerie changed" });

        }

        [HttpPut("{id}/{username}")]
        public async Task<IActionResult> shareGalerie(int id, string username)
        {
            //string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //User? user = await _context.Users.FindAsync(userid);

            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));


            //Galerie? oldGalerie = await _context.Galerie.FindAsync(id);
            Galerie? oldGalerie = await service.GetGalerie(id);

            if (user == null || service.IsEmpty() || oldGalerie == null)
            {
                return NotFound();
            }

            if (!user.Galeries.Contains(oldGalerie))
            {
                return Unauthorized(new { Message = "Not cool man" });
            }


            //User? userO = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            //oldGalerie.User.Add(userO);

            await service.ShareGalerie(oldGalerie, username);


            await service.UpdateGalerie(id,oldGalerie);

            //_context.Entry(oldGalerie).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!GalerieExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Ok(new { Message = "Galerie nice" });
        }

        //private bool GalerieExists(int id)
        //{
        //    return (_context.Galerie?.Any(e => e.Id == id)).GetValueOrDefault();
        //}


    }
}
