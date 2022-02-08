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
                var resultado = userManager.CreateAsync(adm, "Admin@123").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(adm, "administrador").Wait();
                }

                var pfr = new UserModel();
                pfr.Id = 2;
                pfr.UserName = "professor@email.com";
                pfr.Email = "professor@email.com";
                pfr.NomeCompleto = "Professor Um";
                pfr.TUsers = "Professor";
                pfr.DataNascimento = new DateTime(1980, 1, 1);
                var resul = userManager.CreateAsync(pfr, "Proff@123").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(pfr, "Professor").Wait();
                }

                var pfr1 = new UserModel();
                pfr1.Id = 3;
                pfr1.UserName = "professor2@email.com";
                pfr1.Email = "professor2@email.com";
                pfr1.NomeCompleto = "Professor Dois";
                pfr1.TUsers = "Professor";
                pfr1.DataNascimento = new DateTime(1980, 1, 1);
                var res = userManager.CreateAsync(pfr1, "Proff2@123").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(pfr1, "Professor").Wait();
                }

                var pfr2 = new UserModel();
                pfr2.Id = 4;
                pfr2.UserName = "professor3@email.com";
                pfr2.Email = "professor3@email.com";
                pfr2.NomeCompleto = "Professor Três";
                pfr2.TUsers = "Professor";
                pfr2.DataNascimento = new DateTime(1980, 1, 1);
                var re = userManager.CreateAsync(pfr2, "Proff3@123").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(pfr2, "Professor").Wait();
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