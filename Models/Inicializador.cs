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

        private static void GerarPerfis(UserManager<UserModel> userManager, ClassHomedbContext _context)
        {
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

        private static void GerarThemas(ClassHomedbContext _context)
        {
            var them1 = new Config();
            var them2 = new Config();
            var them3 = new Config();
            var them4 = new Config();

            them1.Nome = "Claro Padrão";
            them1.Fundo = "bg-white";
            them1.Menu = "navbar-white bg-white ";
            
            them1.Texto = "text-white";
            them1.Link = "text-white";
            them1.TitleText = "text-dark";

            them1.CardCab = "card-header bg-white bg-opacity-25";
            them1.CardStyle = "border-radius: 3%; background: linear-gradient(45deg, rgb(12, 66, 80), rgb(135, 22, 24)); box-shadow: 1px 2px 15px  #222; border-radius: 10px;";
            them1.CardStyleCab = "border-bottom-left-radius: 12px; border-bottom-right-radius: 12px;";
            
            them1.BTNin = "btn btn-primary";
            them1.BTNoff = "btn btn-outline-dark";
                        
            them1.Tabela = "table tabela text-secondary";
            them1.TabelaText = "table text-secondary";

            them1.DCPBannerStyle = "border-radius: 3%; background: linear-gradient(45deg, rgb(12, 66, 80), rgb(135, 22, 24));";
            them1.DCPBoxColor = "bg-white";
            them1.DCPBoxText = "text-black";

            _context.Configs.Add(them1);
//----------------------------------------------------------------
            them2.Nome = "Grey Padrão";
            them2.Fundo = "bg-light";
            them2.Menu = "navbar-light bg-light";
            
            them2.Texto = "text-white";
            them2.Link = "link-light";
            them2.TitleText = "text-dark";

            them2.CardCab = "card-header bg-white bg-opacity-25";
            them2.CardStyle = "background: linear-gradient(35deg, rgb(5, 06, 55), rgb(106, 211, 230)); box-shadow: 1px 2px 15px #222; border-radius: 10px;";
            them2.CardStyleCab = "border-bottom-left-radius: 12px; border-bottom-right-radius: 12px; border-bottom: 1px solid grey; border-left: 1px solid grey; border-right: 1px solid grey;";
            
            them2.BTNin = "btn btn-primary";
            them2.BTNoff = "btn btn-outline-white";
                        
            them2.Tabela = "table tabela text-secondary";
            them2.TabelaText = "table text-secondary";

            them2.DCPBannerStyle = "border-radius: 3%; background: linear-gradient(25deg, rgb(5, 06, 55), rgb(106, 211, 230));";
            them2.DCPBoxColor = "bg-white";
            them2.DCPBoxText = "text-black";

            _context.Configs.Add(them2);
//----------------------------------------------------------------
            them3.Nome = "Branco Branco";
            them3.Fundo = "bg-white";
            them3.Menu = "navbar-white bg-white ";
            
            them3.Texto = "text-dark";
            them3.Link = "text-white";
            them3.TitleText = "text-dark";

            them3.CardCab = "card-header bg-white bg-opacity-25";
            them3.CardStyle = "background: linear-gradient(35deg, rgb(280, 280, 280),rgb(200, 200, 220)); box-shadow: 1px 2px 15px  #555; border-radius: 10px;";
            them3.CardStyleCab = "border-bottom-left-radius: 12px; border-bottom-right-radius: 12px;";
            
            them3.BTNin = "btn btn-primary";
            them3.BTNoff = "btn btn-outline-dark";
                        
            them3.Tabela = "table tabela text-secondary";
            them3.TabelaText = "table text-secondary";

            them3.DCPBannerStyle = "border-radius: 3%; background: linear-gradient(35deg,rgb(39, 39, 70),rgb(77, 125, 170));";
            them3.DCPBoxColor = "bg-white";
            them3.DCPBoxText = "text-black";

            them3.Estado = Estado.Ativado;

            _context.Configs.Add(them3);
//----------------------------------------------------------------
            them4.Nome = "Dark Rainbow";
            them4.Fundo = "bg-black";
            them4.Menu = "navbar-black bg-black";
            
            them4.Texto = "text-white";
            them4.Link = "text-black";
            them4.TitleText = "text-white";

            them4.CardCab = "card-header bg-white bg-opacity-25";
            them4.CardStyle = "background: linear-gradient(12deg,rgb(57, 105, 150), rgb(230, 230, 0), rgb(235, 14, 183)); box-shadow: 1px 2px 5px  #fff; border-radius: 10px;";
            them4.CardStyleCab = "border-bottom-left-radius: 12px; border-bottom-right-radius: 12px; border-bottom: 1px solid grey; border-left: 1px solid grey; border-right: 1px solid grey;";
            
            them4.BTNin = "btn btn-secondary";
            them4.BTNoff = "btn btn-outline-white";
                        
            them4.Tabela = "table tabela text-white";
            them4.TabelaText = "table text-white";

            them4.DCPBannerStyle = "background: linear-gradient(12deg,rgb(57, 105, 150), rgb(230, 230, 0), rgb(235, 14, 183));";
            them4.DCPBoxColor = "bg-light";
            them4.DCPBoxText = "text-white";

            _context.Configs.Add(them4);
//------------------------------
            _context.SaveChanges();
        }

        private static void InicializarDados(UserManager<UserModel> userManager, ClassHomedbContext _context)
        {
            if (userManager.FindByNameAsync("admin@email.com").Result == null)
            {
                //Perfis
                GerarPerfis(userManager, _context);

                //Temas
                GerarThemas(_context);

                
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