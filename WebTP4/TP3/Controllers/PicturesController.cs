using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TP3.Data;
using TP3.Models;

namespace TP3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly TP3Context _context;
        readonly UserManager<User> userManager;

        public PicturesController(TP3Context context, UserManager<User> _userManager)
        {
            _context = context;
            userManager = _userManager;

        }

        [HttpPost("{id}")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<PhotoImage>> PostPhoto(int id)
        {
            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile? file = formCollection.Files.GetFile("monImage");
                if(file != null)
                {
                    Image image = Image.Load(file.OpenReadStream());

                    PhotoImage photoImage = new PhotoImage()
                    {
                        Id = 0,
                        FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                        MimeType = file.ContentType
                    };
                    photoImage.FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    photoImage.MimeType = file.ContentType;

                    image.Save(Directory.GetCurrentDirectory() + "/images/original/" + photoImage.FileName);
                    image.Mutate(i =>
                        i.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Min,
                            Size = new Size()
                            {
                                Width = 320
                            }
                        })

                    );

                    image.Save(Directory.GetCurrentDirectory() + "/images/miniature/" + photoImage.FileName);

                    photoImage.Galerie = await _context.Galerie.FindAsync(id);

                    _context.Photo.Add(photoImage);
                    await _context.SaveChangesAsync();
                    return Ok(photoImage);

                }
                else
                {
                    return NotFound(new { Message = "Aucune image fournie" });
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpPost("{id}")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<PhotoImage>> PostGalPhoto(int id)
        {
            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile? file = formCollection.Files.GetFile("monImage");
                if (file != null)
                {
                    Image image = Image.Load(file.OpenReadStream());

                    Galerie? perm = await _context.Galerie.FindAsync(id);

                    if(perm == null)
                    {
                        NotFound();
                    }
                    
                    if(perm.FileName != null)
                    {
                        perm.FileName = "";
                    }
                    // si la galerie a déjà une photo de couverture, supprimer la photo du file system

                    perm.FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    perm.MimeType = file.ContentType;
                    
                

                    image.Save(Directory.GetCurrentDirectory() + "/images/original/" + perm.FileName);
                    image.Mutate(i =>
                        i.Resize(new ResizeOptions()
                        {
                            Mode = ResizeMode.Min,
                            Size = new Size()
                            {
                                Width = 320
                            }
                        })

                    );

                    image.Save(Directory.GetCurrentDirectory() + "/images/miniature/" + perm.FileName);

                    await _context.SaveChangesAsync();
                    return Ok(perm);

                }
                else
                {
                    return NotFound(new { Message = "Aucune image fournie" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{size}/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPhoto(string size, int id)
        {
            if(_context.Photo == null)
            {
                return NotFound();
            }

            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            PhotoImage? photo = await _context.Photo.FindAsync(id);
            if(photo == null || photo.FileName == null || photo.MimeType == null)
            {
                return NotFound(new { Message = "Cette photo n'existe pas ou n'a pas de photo"});
            }
            if(!Regex.Match(size, "original|miniature").Success)
            {
                return BadRequest(new { Message = "La taille demandée est inadéquate" });
            }
            byte[] bytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/" + size + "/" + photo.FileName);
            return File(bytes, photo.MimeType);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPhotoGalerie( int id)
        {
            if (_context.Galerie == null)
            {
                return NotFound();
            }

            User? user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Galerie? photo = await _context.Galerie.FindAsync(id);
            if (photo == null || photo.FileName == null || photo.MimeType == null)
            {
                return NotFound(new { Message = "Cette photo n'existe pas ou n'a pas de photo" });
            }
           
            byte[] bytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/miniature/" + photo.FileName);
            return File(bytes, photo.MimeType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBirb(int id)
        {
            if(_context.Photo == null)
            {
                return NotFound();
            }
            var photo = await _context.Photo.FindAsync(id);
            if(photo == null)
            {
                return NotFound(new { Message = "Cette photo n'existe pas" });
            }
            if(photo.MimeType != null && photo.FileName != null)
            {
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/miniature/" + photo.FileName);
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/original/" + photo.FileName);

            }

            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
