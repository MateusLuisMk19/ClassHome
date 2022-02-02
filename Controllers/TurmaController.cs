using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassHome.Controllers
{
    public class TurmaController : Controller
    {
        private readonly ClassHomedbContext _context;

        public TurmaController(ClassHomedbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Turmas.OrderBy(x => x.NomeCurso).AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Criar(int? id)
        {
            if (id.HasValue)
            {
                var Turma = await _context.Turmas.FindAsync(id);
                if (Turma == null)
                {
                    return NotFound();
                }
                return View(Turma);
            }
            return View(new TurmaModel());
        }

        private bool TurmaExiste(int id)
        {
            return _context.Turmas.Any(x => x.TurmaId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(int? id, [FromForm] TurmaModel turma)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (TurmaExiste(id.Value))
                    {
                        _context.Turmas.Update(turma);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            this.MostrarMensagem("Turma editada.");
                        }
                        else
                        {
                            this.MostrarMensagem("Erro ao editar turma.", true);
                        }
                    }
                    else
                    {
                        this.MostrarMensagem("Turma não encontrada.", true);
                    }
                }
                else
                {
                    var turmaG = _context.Turmas.FirstOrDefault(x => x.NomeCurso == turma.NomeCurso);
                    if (turmaG != null && turma.Local == turmaG.Local)
                    {
                        this.MostrarMensagem("Esta turma já existe.", true);
                    }
                    else
                    {
                        _context.Turmas.Add(turma);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            ViewBag.Turmaid = turma.TurmaId;
                            this.MostrarMensagem("Nova turma criada.");
                        }
                        else
                        {
                            this.MostrarMensagem("Erro ao criar turma.", true);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(turma);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                this.MostrarMensagem("Turma não informada.", true);
                return RedirectToAction(nameof(Index));
            }

            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                this.MostrarMensagem("Turma não encontrada.", true);
                return RedirectToAction(nameof(Index));
            }

            return View(turma);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma != null)
            {
                var disciplinaRel = _context.Disciplinas.FirstOrDefault(x => x.TurmaId == turma.TurmaId);
                if (disciplinaRel == null)
                {
                    _context.Turmas.Remove(turma);
                    if (await _context.SaveChangesAsync() > 0)
                        this.MostrarMensagem("Turma excluída.");
                    else
                        this.MostrarMensagem("Não foi possível excluir a turma.", true);

                }
                else
                {
                    this.MostrarMensagem("Não Pode excluir a turma, porque tem disciplinas associadas", true);
                }
                return RedirectToAction(nameof(Index));


            }
            else
            {
                this.MostrarMensagem("Turma não encontrada.", true);
                return RedirectToAction(nameof(Index));
            }
        }

    }
}