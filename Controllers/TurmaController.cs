using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Models;
using ClassHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
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

        [Authorize]
        public async Task<IActionResult> Index(int id)
        {
            var idTurma = _context.Turmas.FirstOrDefault(x => x.TurmaId == id);
            var ProffInDisciplina = _context.ProfessorDisciplina.OrderBy(x => x.ProfessorId).AsNoTracking();
            var alunoInTurma = _context.Matriculas.OrderBy(x => x.AlunoId).AsNoTracking();

            ViewBag.AlunoInTurma = alunoInTurma;
            ViewBag.ProffInDisciplina = ProffInDisciplina;
            ViewBag.TurmaId = idTurma;
            return View(await _context.Disciplinas.OrderBy(x => x.Nome).Include(x => x.Turma)
            .Where(x => x.TurmaId == id).AsNoTracking().ToListAsync());

        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar(int? id, [FromForm] DisciplinaModel disciplina)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
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
                else
                {
                    var disciplinaG = _context.Disciplinas.OrderBy(x => x.Nome).Where(x => x.Nome == disciplina.Nome);
                    if (disciplinaG.Count() > 0)
                    {
                        foreach (var dis in disciplinaG)
                        {
                            if (disciplina.TurmaId == dis.TurmaId)
                            {
                                this.MostrarMensagem("Esta disciplina já existe.", true);
                                return RedirectToAction("Index", new RouteValueDictionary(new { action = "Index", Id = disciplina.TurmaId }));
                            }
                        }
                    }


                    _context.Disciplinas.Add(disciplina);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var dp = new ProfessorDisciplinaModel();
                        dp.DisciplinaId = disciplina.DisciplinaId;
                        dp.ProfessorId = disciplina.CriadorId;

                        _context.ProfessorDisciplina.Add(dp);
                        _context.SaveChanges();

                        ViewBag.DisciplinaId = disciplina.TurmaId;
                        this.MostrarMensagem("Nova disciplina criada.");
                    }
                    else
                    {
                        this.MostrarMensagem("Erro ao criar disciplina.", true);
                    }

                }
                return RedirectToAction("Index", new RouteValueDictionary(new { action = "Index", Id = disciplina.TurmaId }));
            }
            else
            {
                return View(disciplina);
            }
        }


        [Authorize]
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

        [Authorize]
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

        [Authorize(Roles = "Professor")]
        [HttpGet]
        public IActionResult AddProff(int? id)
        {

            var disciplina = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == id);
            var ProffInDisciplina = _context.ProfessorDisciplina.OrderBy(x => x.ProfessorId).Where(x => x.DisciplinaId == disciplina.DisciplinaId).AsNoTracking();
            var ProffInTurma = _context.TurmaUser.OrderBy(x => x.UserId).Where(x => x.TurmaId == disciplina.TurmaId).AsNoTracking();

            var prf = _context.Useres.Where(x => x.Id != disciplina.CriadorId).OrderBy(x => x.NomeCompleto).Where(p => p.TUsers == "Professor").AsNoTracking().ToList();
            var idsTurma = new List<int>();
            var PApv = new List<UserModel>();
            var inTurma = false;
            var user = new List<UserModel>();

            foreach (var n in prf)
            {
                foreach (var PinT in ProffInTurma)
                {
                    if (n.Id == PinT.UserId) //Está na turma
                    {
                        inTurma = true;
                    }

                    if (inTurma) //Está na turma
                    {
                        idsTurma.Add(n.Id);
                        foreach (var p in ProffInDisciplina)
                        {
                            if (n.Id == p.ProfessorId) // Está na disciplina
                            {
                                idsTurma.Remove(n.Id);
                            }
                        }
                        if (prf.Count <= 1)
                            break;

                        inTurma = false;
                    }

                }
                if (prf.Count <= 1)
                    break;
            }
            foreach (var us in prf)
            {
                if (idsTurma.Contains(us.Id))
                {
                    PApv.Add(us);
                }
            }

            var prfSelectList = new SelectList(PApv,
                nameof(UserModel.Id), nameof(UserModel.NomeCompleto));

            ViewBag.ProffInDisciplina = ProffInDisciplina;
            ViewBag.DCP = prfSelectList;
            ViewBag.DisciplinaId = disciplina;

            return View(new ProfessorDisciplinaModel() { DisciplinaId = id.Value });
        }

        [Authorize(Roles = "Professor")]
        [HttpPost]
        public IActionResult AddProffPost([FromForm] ProfessorDisciplinaModel proffDis)
        {
            if (proffDis.ProfessorId != 0)
            {
                var disciplina = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == proffDis.DisciplinaId);

                var dp = new ProfessorDisciplinaModel();
                dp.DisciplinaId = proffDis.DisciplinaId;
                dp.ProfessorId = proffDis.ProfessorId;

                _context.ProfessorDisciplina.Add(dp);
                _context.SaveChanges();

                this.MostrarMensagem("Professor adicionado à disciplina.");

                return RedirectToAction("Index", new RouteValueDictionary(new { Id = disciplina.TurmaId }));
            }
            else
            {
                var disciplina = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == proffDis.DisciplinaId);
                this.MostrarMensagem("Não selecionou nenhum professor.", true);

                return RedirectToAction("Index", new RouteValueDictionary(new { Id = disciplina.TurmaId }));


            }

        }

        [Authorize]
        public IActionResult Utilizadores(int turmaId)
        {
            var tur = _context.Turmas.FirstOrDefault(x => x.TurmaId == turmaId);
            var ProffInTurma = _context.TurmaUser.OrderBy(x => x.UserId).Where(x => x.TurmaId == turmaId).AsNoTracking().ToList();
            var professores = new List<UserModel>();
            var Alunos = _context.Useres.OrderBy(x => x.NomeCompleto).Where(x => x.TUsers == "Aluno").AsNoTracking().ToList();
            var AlunosInTurma = new List<UserModel>();
            var MatriculasTurma = _context.Matriculas.Where(x => x.TurmaId == turmaId).AsNoTracking().ToList();
            var uId = 0;

            foreach (var matricula in MatriculasTurma)
            {
                foreach (var aluno in Alunos)
                {
                    {
                        if (matricula.AlunoId == aluno.Id)
                        {
                            if (aluno.Id != uId)
                            {
                                AlunosInTurma.Add(aluno);
                            }
                            uId = aluno.Id;
                        }
                    }
                }
            }



            foreach (var turma in ProffInTurma)
            {
                var us = _context.Useres.FirstOrDefault(x => x.Id == turma.UserId);
                professores.Add(us);
            }

            ViewBag.Turma = tur;
            ViewBag.Professores = professores;
            return View(AlunosInTurma);
        }

        [Authorize(Roles = "Professor")]
        [HttpGet]
        public IActionResult AddAluno(int turmaId)
        {
            var tur = _context.Turmas.FirstOrDefault(x => x.TurmaId == turmaId);
            var disciplinas = _context.Disciplinas.OrderBy(x => x.Nome).Where(x => x.TurmaId == turmaId).AsNoTracking().ToList();
            var alunos = _context.Useres.OrderBy(x => x.NomeCompleto).Where(x => x.TUsers == "Aluno").AsNoTracking().ToList();

            var AlunoSelectList = new SelectList(alunos,
                nameof(UserModel.Id), nameof(UserModel.NomeCompleto));
            var DisciplinaSelectList = new SelectList(disciplinas,
                nameof(DisciplinaModel.DisciplinaId), nameof(DisciplinaModel.Nome));

            ViewBag.Turma = tur;
            ViewBag.AIS = AlunoSelectList;
            ViewBag.Disciplinas = DisciplinaSelectList;

            return View(new MatriculaModel());
        }

        [Authorize(Roles = "Professor")]
        [HttpPost]
        public IActionResult AddAlunoPost([FromForm] MatriculaModel matricula)
        {
            if (matricula.AlunoId == 0 || matricula.DisciplinaId == 0)
            {
                if (matricula.AlunoId == 0)
                    this.MostrarMensagem("Não selecionou nenhum aluno.", true);

                if (matricula.DisciplinaId == 0)
                    this.MostrarMensagem("Não selecionou nenhuma disciplina.", true);

                return RedirectToAction("AddAluno", new RouteValueDictionary(new { matricula.TurmaId }));

            }
            else
            {
                var matric = _context.Matriculas.Where(m => m.DisciplinaId == matricula.DisciplinaId).ToList();

                foreach (var m in matric)
                {
                    if (matricula.DisciplinaId == m.DisciplinaId && m.AlunoId == matricula.AlunoId)
                    {
                        var al = _context.Useres.FirstOrDefault(x => x.Id == m.AlunoId);
                        var dc = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == m.DisciplinaId);

                        this.MostrarMensagem("" + al.NomeCompleto + " já está associado à " + dc.Nome + ".", true);
                        return RedirectToAction("AddAluno", new RouteValueDictionary(new { matricula.TurmaId }));
                    }

                }
                var disciplina = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == matricula.DisciplinaId);
                var tur = _context.Turmas.FirstOrDefault(x => x.TurmaId == disciplina.TurmaId);

                var mt = new MatriculaModel();
                mt.DisciplinaId = matricula.DisciplinaId;
                mt.AlunoId = matricula.AlunoId;
                mt.TurmaId = matricula.TurmaId;

                _context.Matriculas.Add(mt);
                _context.SaveChanges();

                this.MostrarMensagem("Aluno adicionado à turma " + tur.NomeCurso + " e a disciplina de " + disciplina.Nome + ".");

                return RedirectToAction("Index", new RouteValueDictionary(new { action = "Index", Id = disciplina.TurmaId }));

            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemProff(int idProff, int idTurm)
        {
            var turm = _context.Turmas.FirstOrDefault(d => d.TurmaId == idTurm);
            var proff = _context.Useres.FirstOrDefault(x => x.Id == idProff);

            ViewBag.Proff = proff;

            return View(turm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RemProffPost(int idProff, int idTur)
        {
            var pf = _context.Useres.FirstOrDefault(x => x.Id == idProff);
            var turm = _context.Turmas.FirstOrDefault(d => d.TurmaId == idTur);
            var dp = _context.TurmaUser.Where(x => x.UserId == idProff).FirstOrDefault(x => x.TurmaId == idTur);

            if (dp != null)
            {
                _context.TurmaUser.Remove(dp);
                _context.SaveChanges();

                this.MostrarMensagem("" + pf.NomeCompleto + " removido da Turma.");
            }

            return RedirectToAction("Index", new RouteValueDictionary(new { Id = turm.TurmaId }));
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemAluno(int idAluno, int idTurm)
        {
            var turm = _context.Turmas.FirstOrDefault(d => d.TurmaId == idTurm);
            var aluno = _context.Useres.FirstOrDefault(x => x.Id == idAluno);

            ViewBag.Aluno = aluno;

            return View(turm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RemAlunoPost(int idAluno, int idTur)
        {
            var tur = _context.Turmas.FirstOrDefault(d => d.TurmaId == idTur);
            var mt = _context.Matriculas.Where(x => x.AlunoId == idAluno).ToList().Where(x => x.TurmaId == idTur);
            var us = _context.Useres.FirstOrDefault(x => x.Id == idAluno);

            if (mt != null)
            {
                foreach (var matric in mt)
                {
                    _context.Matriculas.Remove(matric);
                }
                _context.SaveChanges();

                this.MostrarMensagem("" + us.NomeCompleto + " removido da Turma.");
            }

            return RedirectToAction("Index", new RouteValueDictionary(new { Id = tur.TurmaId }));
        }
    }
}