using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using ClassHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassHome.Controllers
{

    public class UserController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ClassHomedbContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserController(UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager, ClassHomedbContext context, RoleManager<IdentityRole<int>> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
            this._roleManager = roleManager;
        }

        [Authorize(Roles = "administrador")]
        [HttpGet]
        public async Task<IActionResult> Cadastrar(string id)
        {
            var roles = _context.Roles.OrderBy(r => r.Name).Where(r => r.Name != "administrador").ToList();

                var rolesSelectList = new SelectList(roles);


            ViewBag.TipoUser = rolesSelectList;

            if (!string.IsNullOrEmpty(id))
            {

                var userBD = await _userManager.FindByIdAsync(id);
                if (userBD == null)
                {
                    this.MostrarMensagem("Perfil não encontrado.", true);
                    return RedirectToAction("Index", "Home");
                }
                var userVM = new CadastrarUserViewModel
                {
                    Id = userBD.Id,
                    NomeCompleto = userBD.NomeCompleto,
                    Email = userBD.Email,
                    Telefone = userBD.PhoneNumber,
                    DataNascimento = userBD.DataNascimento
                };
                return View(userVM);
            }
            return View(new CadastrarUserViewModel());
        }

        private bool EntidadeExiste(int id)
        {
            return (_userManager.Users.AsNoTracking().Any(u => u.Id == id));
        }

        private static void MapearCadastrarUserViewModel(CadastrarUserViewModel entidadeOrigem, UserModel entidadeDestino)
        {
            entidadeDestino.NomeCompleto = entidadeOrigem.NomeCompleto;
            entidadeDestino.DataNascimento = entidadeOrigem.DataNascimento;
            entidadeDestino.NormalizedUserName = entidadeOrigem.Email.ToUpper().Trim();
            entidadeDestino.UserName = entidadeOrigem.Email;
            entidadeDestino.Email = entidadeOrigem.Email;
            entidadeDestino.NormalizedEmail = entidadeOrigem.Email.ToUpper().Trim();
            entidadeDestino.PhoneNumber = entidadeOrigem.Telefone;
            entidadeDestino.TUsers = entidadeOrigem.TUsers;
        }

        [Authorize(Roles = "administrador")]
        [HttpPost]
        public async Task<IActionResult> Cadastrar(
        [FromForm] CadastrarUserViewModel userVM)
        {

            //se for alteração, não tem senha e confirmação de senha
            if (userVM.Id > 0)
            {
                ModelState.Remove("Senha");
                ModelState.Remove("ConfSenha");
            }

            if (ModelState.IsValid)
            {
                if (EntidadeExiste(userVM.Id))
                {
                    var userBD = await _userManager.FindByIdAsync(userVM.Id.ToString());
                    if ((userVM.Email != userBD.Email) &&
                        (_userManager.Users.Any(u => u.NormalizedEmail == userVM.Email.ToUpper().Trim())))
                    {
                        ModelState.AddModelError("Email",
                            "Já existe um perfil cadastrado com este e-mail.");
                        return View(userVM);
                    }
                    MapearCadastrarUserViewModel(userVM, userBD);

                    var resultado = await _userManager.UpdateAsync(userBD);
                    if (resultado.Succeeded)
                    {
                        this.MostrarMensagem("Perfil alterado com sucesso.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        this.MostrarMensagem("Não foi possível alterar o Perfil.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(userVM);
                    }
                }
                else
                {
                    var userBD = await _userManager.FindByEmailAsync(userVM.Email);
                    if (userBD != null)
                    {
                        ModelState.AddModelError("Email",
                            "Já existe um Perfil cadastrado com este e-mail.");
                        return View(userBD);
                    }

                    userBD = new UserModel();
                    MapearCadastrarUserViewModel(userVM, userBD);

                    var resultado = await _userManager.CreateAsync(userBD, userVM.Senha);
                    if (resultado.Succeeded)
                    {
                        var role = userBD.TUsers;

                        if(role == "aluno")
                        {
                            var aluno = new AlunoModel(){
                                UserId = userBD.Id,
                            };
                            _context.Alunos.Add(aluno);
                            _context.SaveChanges();
                            _context.SaveChanges();
                            var res = await _userManager.UpdateAsync(userBD);
                        }
                        else if(role == "professor")
                        {
                            var professor = new ProfessorModel(){
                                UserId = userBD.Id,
                            };
                            _context.Professores.Add(professor);
                            _context.SaveChanges();
                            var res = await _userManager.UpdateAsync(userBD);
                        }

                        var result = await _userManager.AddToRoleAsync(userBD, role);
                        if (result.Succeeded)
                        {
                            this.MostrarMensagem("Perfil cadastrado com sucesso. Use suas credenciais para entrar no sistema.");
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        this.MostrarMensagem("Erro ao cadastrar Perfil.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(userVM);
                    }
                }
            }
            else
            {
                return View(userVM);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(login.User, login.Senha, login.Lembrar, false);
                if (resultado.Succeeded)
                {
                    login.ReturnUrl = login.ReturnUrl ?? "~/";
                    return LocalRedirect(login.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Tentativa de login inválida. Reveja seus dados de acesso e tente novamente.");
                    return View(login);
                }
            }
            else
            {
                return View(login);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Where(x => x.Id != 1).AsNoTracking().ToListAsync();

            return View(users);
        }

        [Authorize(Roles = "administrador")]
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                this.MostrarMensagem("Perfil não informado.", true);
                return RedirectToAction(nameof(Index));
            }

            if (!EntidadeExiste(id.Value))
            {
                this.MostrarMensagem("Perfil não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            return View(user);
        }

        [Authorize(Roles = "administrador")]
        [HttpPost]
        public async Task<IActionResult> ExcluirPost(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var resultado = await _userManager.DeleteAsync(user);
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem("Perfil excluído com sucesso.");
                }
                else
                {
                    this.MostrarMensagem("Não foi possível excluir o perfil.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Perfil não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult AcessoRestrito([FromQuery] string returnUrl)
        {
            return View(model: returnUrl);
        }

        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> AddProfessor(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var proff = new UserModel()
                {
                    NomeCompleto = user.NomeCompleto,
                    Email = user.Email,
                    DataNascimento = user.DataNascimento,
                    NormalizedEmail = user.NormalizedEmail,
                    PasswordHash = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                };


                var resultado = await _userManager.AddToRoleAsync(user, "coordenador");
                if (resultado.Succeeded)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, "user");

                    this.MostrarMensagem(
                        $"<b>{user.NomeCompleto}</b> agora é um Coordenador.");
                }
                else
                {
                    this.MostrarMensagem(
                        $"Não foi possível remover <b>{user.UserName}</b> de Coordenador.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Perfil não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> RemCoordenador(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var resultado = await _userManager.RemoveFromRoleAsync(user, "coordenador");
                if (resultado.Succeeded)
                {
                    var result = await _userManager.AddToRoleAsync(user, "user");
                    if (result.Succeeded)
                    {
                        this.MostrarMensagem(
                    $"<b>{user.NomeCompleto}</b> agora é um User.");
                    }
                }
                else
                {
                    this.MostrarMensagem(
                        $"Não foi possível remover <b>{user.UserName}</b> de Coordenador.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Perfil não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Perfil(int id)
        {
            var utilizador = await _context.Useres.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }
    }
}