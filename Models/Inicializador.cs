using System;
using ClassHome.Models;
using Microsoft.AspNetCore.Identity;


namespace ClassHome.Models
{
    public static class Inicializador
    {
        private static void InicializarPerfis(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("administrador").Result)
            {
                var perfil1 = new IdentityRole<int>();
                perfil1.Name = "administrador";
                roleManager.CreateAsync(perfil1).Wait();
            }

            if (!roleManager.RoleExistsAsync("Professor").Result)
            {
                var perfil2 = new IdentityRole<int>();
                perfil2.Name = "Professor";
                roleManager.CreateAsync(perfil2).Wait();
            }

            if (!roleManager.RoleExistsAsync("Aluno").Result)
            {
                var perfil3 = new IdentityRole<int>();
                perfil3.Name = "Aluno";
                roleManager.CreateAsync(perfil3).Wait();
            }
        }


        private static void InicializarDados(UserManager<UserModel> userManager, ClassHomedbContext _context)
        {
            if (userManager.FindByNameAsync("admin@email.com").Result == null)
            {
                //Perfis
                var adm = new UserModel();
                adm.Id = 1;
                adm.UserName = "admin@email.com";
                adm.Email = "admin@email.com";
                adm.NomeCompleto = "Administrador do Sistema";
                adm.DataNascimento = new DateTime(1980, 1, 1);
                adm.PhoneNumber = "999999991";
                var resultado = userManager.CreateAsync(adm, "Admin@123").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(adm, "administrador").Wait();
                }

                _context.SaveChanges();
            }
        }

        public static void InicializarIdentity(UserManager<UserModel> userManager,
            RoleManager<IdentityRole<int>> roleManager, ClassHomedbContext _context)
        {
            InicializarPerfis(roleManager);
            InicializarDados(userManager, _context);
        }
    }
}