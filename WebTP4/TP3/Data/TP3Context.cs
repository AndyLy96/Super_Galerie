using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP3.Models;

namespace TP3.Data
{
    public class TP3Context : IdentityDbContext<User>
    {
        public TP3Context (DbContextOptions<TP3Context> options)
            : base(options)
        {
        }

        public DbSet<TP3.Models.Galerie> Galerie { get; set; } = default!;
        public DbSet<TP3.Models.PhotoImage> Photo { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User u1 = new User
            {
                Id = "11111111-1111-1111-1111-111111111111",
                UserName = "Andy",
                Email = "a@a.a",
                NormalizedEmail = "A@A.A",
                NormalizedUserName = "ANDY"
            };
            u1.PasswordHash = hasher.HashPassword(u1, "Salut1!");
            User u2 = new User
            {
                Id = "11111111-1111-1111-1111-111111111112",
                UserName = "Victor",
                Email = "v@v.v",
                NormalizedEmail = "V@V.V",
                NormalizedUserName = "VICTOR"

            };
            u2.PasswordHash = hasher.HashPassword(u2, "Salut1!");
            builder.Entity<User>().HasData(u1, u2);

            builder.Entity<Galerie>().HasData(
                new Galerie(){ Id = 1, Name = "Andy", IsPublic = true, FileName = "11111111-1111-1111-1111-111111111111.png",  MimeType = "image/png"},
               new Galerie() { Id = 2, Name = "Victor", IsPublic = false }
                );

            builder.Entity<Galerie>().HasMany(u1 => u1.User).WithMany(v => v.Galeries).UsingEntity(e =>
            {
                e.HasData(new { UserId = u1.Id, GaleriesId = 1 });
                e.HasData(new { UserId = u2.Id, GaleriesId = 2 });


            });

            var p1 = new 
            {
                Id = 1,
                FileName = "11111111-1111-1111-1111-111111111111.png",
                MimeType = "image/png",
                GalerieId = 1
            };
            builder.Entity<PhotoImage>().HasData(p1);

            byte[] file = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/original/" + p1.FileName);
            Image image = Image.Load(file);
            image.Mutate(i =>
                i.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Min,
                    Size = new Size() { Width = 320}
                })
            );
            image.Save(Directory.GetCurrentDirectory() + "/images/miniature/" + p1.FileName);
        }
    }
}
