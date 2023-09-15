using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TP3.Models;

namespace TP3.Data
{
    public class GalerieService
    {
        readonly TP3Context _context;

        public GalerieService(TP3Context context){
            _context = context;
        }

        public bool IsEmpty()
        {
            return _context.Galerie == null;
        }

        public async Task CreateGalerie(User user, Galerie galerie)
        {
            galerie.User = new List<User>();
            galerie.User.Add(user);
            user.Galeries = new List<Galerie>();
            user.Galeries.Add(galerie);
            _context.Galerie.Add(galerie);
            await _context.SaveChangesAsync();
        }

        public async Task<Galerie?> GetGalerie(int id)
        {
            return await _context.Galerie.FindAsync(id);
        }

        public async Task<ActionResult<IEnumerable<Galerie>>> GetPublicGaleries()
        {
            
            var galerie = await _context.Galerie.Where(g => g.IsPublic).ToListAsync();
            return galerie;
        }

        //public async Task<ActionResult<IEnumerable<Galerie>>> GetMyGaleries()
        //{
        //    string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    return await _context.Users.SingleAsync(u => u.Id == userid);

        //}

        public async Task DeleteGalerie(Galerie galerie)
        {
            _context.Remove(galerie);
            await _context.SaveChangesAsync();
        }

        public async Task<Galerie?> UpdateGalerie(int id, Galerie galerie)
        {
            _context.Entry(galerie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Galerie.AnyAsync(x => x.Id == id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return galerie;
        }

        public async Task<Galerie?> ShareGalerie(Galerie galerie, string username)
        {
            User? userO = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            galerie.User.Add(userO);
            _context.Entry(galerie).State = EntityState.Modified;
            return galerie;
        }

        public async Task PorPGalerie(Galerie galerie)
        {
            galerie.IsPublic = !galerie.IsPublic;
            await _context.SaveChangesAsync();
        }
    }
}
