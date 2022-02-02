using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using ClassHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ClassHome.Controllers
{
    public class DInnerController : Controller
    {
        private readonly ClassHomedbContext _context;
        private readonly UserManager<UserModel> _userManager;


        public DInnerController(UserManager<UserModel> userManager, ClassHomedbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }
        public IActionResult Index(int id)
        {
            var idDis = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == id);

            ViewBag.DisciplinaId = idDis;
            return View(new DInnerViewModel());

        }
/* 
        [HttpPost]
        public async Task<IActionResult> Publicar(
        [FromForm] DInnerViewModel dinnerVM)
        {
            if (ModelState.IsValid)
            {
                var pub = new Publicacao();

                pub.Texto = dinnerVM.PublicacaoTexto;
                pub.DisciplinaId = dinnerVM.DisciplinaId;
                pub.UserId = dinnerVM.UserId;


                if (disciplinaG != null)
                {
                    this.MostrarMensagem("Esta disciplina já existe.", true);
                }
                else
                {
                    _context.Disciplinas.Add(disciplina);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        this.MostrarMensagem("Nova disciplina criada.");
                    }
                    else
                    {
                        this.MostrarMensagem("Erro ao criar disciplina.", true);
                    }
                }

            }
            else
            {
                return View(dinnerVM);
            }
        } */

        /*
                [HttpGet]
                public async Task<IActionResult> Excluir(int? id)
                {
                    var disc = _context.Disciplinas.FirstOrDefault(d => d.DisciplinaId == id);
                    var idTurma = _context.Turmas.FirstOrDefault(x => x.TurmaId == disc.TurmaId);

                    ViewBag.TurmaId = idTurma;

                    if (!id.HasValue)
                    {
                        this.MostrarMensagem("Disciplina não informada.", true);
                        return RedirectToAction(nameof(Index));
                    }

                    var disciplina = await _context.Disciplinas.FindAsync(id);
                    if (disciplina == null)
                    {
                        this.MostrarMensagem("Disciplina não encontrada.", true);
                        return RedirectToAction(nameof(Index));
                    }

                    return View(disciplina);
                }

                [HttpPost]
                public async Task<IActionResult> Excluir(int id)
                {
                    var disciplina = await _context.Disciplinas.FindAsync(id);
                    if (disciplina != null)
                    {
                        _context.Disciplinas.Remove(disciplina);
                        if (await _context.SaveChangesAsync() > 0)
                            this.MostrarMensagem("Disciplina excluída.");
                        else
                            this.MostrarMensagem("Não foi possível excluir a disciplina.", true);

                        return RedirectToAction("Index", new RouteValueDictionary(new { action = "Index", Id = disciplina.TurmaId }));
                    }
                    else
                    {
                        this.MostrarMensagem("Disciplina não encontrada.", true);
                        return RedirectToAction(nameof(Index));
                    }
                } */
    }
}