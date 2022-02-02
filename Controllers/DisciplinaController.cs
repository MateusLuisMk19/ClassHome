using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ClassHome.Controllers
{
    public class DisciplinaController : Controller
    {
        private readonly ClassHomedbContext _context;

        public DisciplinaController(ClassHomedbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var idTurma = _context.Turmas.FirstOrDefault(x => x.TurmaId == id);

            ViewBag.TurmaId = idTurma;
            return View(await _context.Disciplinas.OrderBy(x => x.Nome).Include(x => x.Turma).Where(x => x.TurmaId == id).AsNoTracking().ToListAsync());

        }

        [HttpGet]
        public async Task<IActionResult> Criar(int? id, int idTrel)
        {
            var idTurma = _context.Turmas.FirstOrDefault(x => x.TurmaId == idTrel);

            ViewBag.TurmaId = idTurma;

            if (id.HasValue)
            {
                var Disciplina = await _context.Disciplinas.FindAsync(id);
                if (Disciplina == null)
                {
                    return NotFound();
                }
                return View(Disciplina);
            }
            return View(new DisciplinaModel());
        }

        private bool DisciplinaExiste(int id)
        {
            return _context.Disciplinas.Any(x => x.DisciplinaId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(int? id, [FromForm] DisciplinaModel disciplina)
        {
            if (!ModelState.IsValid)
            {
                return View(disciplina);
            }
            else
            {
                if (id.HasValue) //edita
                {
                    if (DisciplinaExiste(id.Value))
                    {
                        _context.Disciplinas.Update(disciplina);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            this.MostrarMensagem("Disciplina editada.");
                        }
                        else
                        {
                            this.MostrarMensagem("Erro ao editar disciplina.", true);
                        }
                    }
                    else
                    {
                        this.MostrarMensagem("Disciplina não encontrada.", true);
                    }
                }
                else    //cria
                {
                    var disciplinaG = _context.Disciplinas.FirstOrDefault(x => x.Nome == disciplina.Nome);
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

                return RedirectToAction("Index", new RouteValueDictionary(new { action = "Index", Id = disciplina.TurmaId }));
            }
        }

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
        }
    }
}