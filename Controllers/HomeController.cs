using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClassHomedbContext _context;

        public HomeController(ClassHomedbContext context)
        {
            this._context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var ProffInTurma = _context.TurmaUser.OrderBy(x => x.UserId).AsNoTracking();
            var alunoInTurma = _context.Matriculas.OrderBy(x => x.AlunoId).AsNoTracking();

            ViewBag.AlunoInTurma = alunoInTurma;
            ViewBag.ProffInTurma = ProffInTurma;

            return View(await _context.Turmas.OrderBy(x => x.NomeCurso).AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "Professor")]
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

        [Authorize(Roles = "Professor")]
        [HttpPost]
        public async Task<IActionResult> Criar(int? id, [FromForm] TurmaModel turma, int? idCriador)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (TurmaExiste(id.Value))
                    {
                        if (turma.CriadorId != idCriador.Value)
                            turma.CriadorId = idCriador.Value;

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
                            var tp = new TurmaUserModel();
                            tp.UserId = turma.CriadorId;
                            tp.TurmaId = turma.TurmaId;
                            tp.User = _context.Useres.FirstOrDefault(x => x.Id == turma.CriadorId);

                            _context.TurmaUser.Add(tp);
                            _context.SaveChanges();

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

        [Authorize(Roles = "Professor")]
        [HttpGet]
        public IActionResult AddProff(int? id)
        {

            var turma = _context.Turmas.FirstOrDefault(x => x.TurmaId == id);
            var ProffInTurma = _context.TurmaUser.OrderBy(x => x.UserId).Where(x => x.TurmaId == turma.TurmaId).AsNoTracking();

            var prf = _context.Useres.Where(x => x.Id != turma.CriadorId).OrderBy(x => x.NomeCompleto).Where(p => p.TUsers == "Professor").AsNoTracking().ToList();
            var ppp = new List<UserModel>();
            var count = 0;
            var user = new UserModel();

            foreach (var n in prf)
            {
                foreach (var p in ProffInTurma)
                {
                    if (n.Id == p.UserId)
                    {
                        count++;
                        user = n;
                        ppp.Add(n);
                    }
                }
            }

            if (count > 0)
            {
                foreach (var us in ppp)
                {
                    prf.Remove(us);
                }
            }

            var prfSelectList = new SelectList(prf,
                nameof(UserModel.Id), nameof(UserModel.NomeCompleto));

            ViewBag.ProffInTurma = ProffInTurma;
            ViewBag.PRF = prfSelectList;
            ViewBag.TurmaId = turma;

            return View(new TurmaUserModel() { TurmaId = id.Value });
        }

        [Authorize(Roles = "Professor")]
        [HttpPost]
        public IActionResult AddProffPost([FromForm] TurmaUserModel turmauser)
        {
            if (turmauser.UserId != 0)
            {
                var tp = new TurmaUserModel();
                tp.UserId = turmauser.UserId;
                tp.TurmaId = turmauser.TurmaId;
                tp.User = _context.Useres.FirstOrDefault(x => x.Id == turmauser.UserId);

                _context.TurmaUser.Add(tp);
                _context.SaveChanges();

                this.MostrarMensagem("Professor adicionado à turma.");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Certifique-se de que selecionou o professor.", true);
            }

            return View("AddProff", turmauser.TurmaId);

            

        }

        [Authorize(Roles = "Professor")]
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

        [Authorize(Roles = "Professor")]
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