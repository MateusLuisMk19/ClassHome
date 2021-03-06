using System.Collections.Generic;
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
        
        [Authorize]
        public IActionResult Index(int id)
        {
            var idDis = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == id);
            var publicacoes = _context.Publicacoes.Where(p => p.DisciplinaId == id).OrderByDescending(x => x.DataPublicacao).AsNoTracking();


            ViewBag.Publicacoes = publicacoes;
            ViewBag.DisciplinaId = idDis;


            return View(new DInnerViewModel());

        }

        [Authorize]
        [HttpGet]
        public IActionResult Publicar(int id, int type)
        {


            if (type == 1)
            {
                var pub = _context.Publicacoes.FirstOrDefault(x => x.PublicacaoId == id);

                var publi = new DInnerViewModel();

                publi.PublicacaoTexto = pub.Texto;
                publi.DisciplinaId = pub.DisciplinaId;
                publi.UserId = pub.UserId;
                publi.PublicacaoId = pub.PublicacaoId;

                var idDis = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == publi.DisciplinaId);
                ViewBag.DisciplinaId = idDis;

                return View("EditarPublicacao", publi);
            }
            else if (type == 2)
            {
                var comen = _context.Comentarios.FirstOrDefault(x => x.ComentarioId == id);

                var com = new DInnerViewModel();

                com.ComentarioTexto = comen.Texto;
                com.PublicacaoId = comen.PublicacaoId;
                com.UserId = comen.UserId;
                com.ComentarioId = comen.ComentarioId;


                var idDis = _context.Disciplinas
                .FirstOrDefault(x => x.DisciplinaId == _context.Publicacoes
                .FirstOrDefault(n => n.PublicacaoId == com.PublicacaoId).DisciplinaId);

                ViewBag.DisciplinaId = idDis;

                return View("EditarComentario", com);
            }

            return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Disciplina" }));
        }

        private bool PublicacaoExiste(int id)
        {
            return _context.Publicacoes.Any(x => x.PublicacaoId == id);
        }

        private bool ComentarioExiste(int id)
        {
            return _context.Comentarios.Any(x => x.ComentarioId == id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Publicar(int? id, [FromForm] DInnerViewModel dinnerVM, int type)
        {
            var DisId = dinnerVM.DisciplinaId;

            if (id.HasValue) //edi????o
            {
                if (type == 1)
                {
                    var pub = new Publicacao();

                    pub.Texto = dinnerVM.PublicacaoTexto;
                    pub.DisciplinaId = dinnerVM.DisciplinaId;
                    pub.UserId = dinnerVM.UserId;
                    pub.PublicacaoId = dinnerVM.PublicacaoId;


                    if (PublicacaoExiste(id.Value))
                    {
                        _context.Publicacoes.Update(pub);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            this.MostrarMensagem("Publica????o editada.");
                        }
                        else
                        {
                            this.MostrarMensagem("Erro ao editar publica????o.", true);
                        }
                    }
                    else
                    {
                        this.MostrarMensagem("Publica????o n??o encontrada.", true);
                    }
                }
                else if (type == 2)
                {
                    var com = new Comentario();

                    com.Texto = dinnerVM.ComentarioTexto;
                    com.PublicacaoId = dinnerVM.PublicacaoId;
                    com.UserId = dinnerVM.UserId;
                    com.ComentarioId = dinnerVM.ComentarioId;

                    if (ComentarioExiste(id.Value))
                    {
                        _context.Comentarios.Update(com);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            this.MostrarMensagem("Coment??rio editado.");
                        }
                        else
                        {
                            this.MostrarMensagem("Erro ao editar coment??rio.", true);
                        }
                    }
                    else
                    {
                        this.MostrarMensagem("coment??rio n??o encontrado.", true);
                    }
                }

            }
            else //cria????o
            {
                if (type == 1)
                {
                    var pub = new Publicacao();

                    pub.Texto = dinnerVM.PublicacaoTexto;
                    pub.DisciplinaId = dinnerVM.DisciplinaId;
                    pub.UserId = dinnerVM.UserId;

                    if (pub.Texto != "" || pub.Texto != null)
                    {
                        _context.Publicacoes.Add(pub);
                    }
                }
                else if (type == 2)
                {
                    var com = new Comentario();

                    com.Texto = dinnerVM.ComentarioTexto;
                    com.PublicacaoId = dinnerVM.PublicacaoId;
                    com.UserId = dinnerVM.UserId;

                    if (com.Texto != "" || com.Texto != null)
                    {
                        _context.Comentarios.Add(com);
                    }
                }

                _context.SaveChanges();

            }

            return RedirectToAction("Index", new RouteValueDictionary(new { id = DisId }));
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id, int type)
        {
            if (type == 1)
            {
                var pub = _context.Publicacoes.FirstOrDefault(d => d.PublicacaoId == id);
                var idDisc = _context.Disciplinas.FirstOrDefault(x => x.DisciplinaId == pub.DisciplinaId);

                ViewBag.Disciplina = idDisc;

                if (!id.HasValue)
                {
                    this.MostrarMensagem("Publica????o n??o informada.", true);
                    return RedirectToAction(nameof(Index));
                }

                var publicacao = await _context.Publicacoes.FindAsync(id);
                if (publicacao == null)
                {
                    this.MostrarMensagem("Publica????o n??o encontrada.", true);
                    return RedirectToAction(nameof(Index));
                }

                return View("ExcluirPub", publicacao);
            }
            else if (type == 2)
            {
                var com = _context.Comentarios.FirstOrDefault(d => d.ComentarioId == id);
                var idDis = _context.Disciplinas
                .FirstOrDefault(x => x.DisciplinaId == _context.Publicacoes
                .FirstOrDefault(n => n.PublicacaoId == com.PublicacaoId).DisciplinaId);

                ViewBag.Disciplina = idDis;

                if (!id.HasValue)
                {
                    this.MostrarMensagem("Publica????o n??o informada.", true);
                    return RedirectToAction(nameof(Index));
                }

                var comentario = await _context.Comentarios.FindAsync(id);
                if (comentario == null)
                {
                    this.MostrarMensagem("Publica????o n??o encontrada.", true);
                    return RedirectToAction(nameof(Index));
                }

                return View("ExcluirCom", comentario);
            }
            return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Disciplina" }));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Excluir(int id, int type)
        {
            if (type == 1)
            {
                var pub = await _context.Publicacoes.FindAsync(id);
                if (pub != null)
                {
                    _context.Publicacoes.Remove(pub);
                    if (await _context.SaveChangesAsync() > 0)
                        this.MostrarMensagem("Publica????o exclu??da.");
                    else
                        this.MostrarMensagem("N??o foi poss??vel excluir a Publica????o.", true);

                    return RedirectToAction("Index", new RouteValueDictionary(new { Id = pub.DisciplinaId }));
                }
                else
                {
                    this.MostrarMensagem("Publica????o n??o encontrada.", true);
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (type == 2)
            {
                var com = await _context.Comentarios.FindAsync(id);
                if (com != null)
                {
                    var idDis = _context.Disciplinas
                    .FirstOrDefault(x => x.DisciplinaId == _context.Publicacoes
                    .FirstOrDefault(n => n.PublicacaoId == com.PublicacaoId).DisciplinaId);

                    _context.Comentarios.Remove(com);
                    if (await _context.SaveChangesAsync() > 0)
                        this.MostrarMensagem("Coment??rio exclu??do.");
                    else
                        this.MostrarMensagem("N??o foi poss??vel excluir o Coment??rio.", true);

                    return RedirectToAction("Index", new RouteValueDictionary(new { Id = idDis.DisciplinaId }));
                }
                else
                {
                    this.MostrarMensagem("Coment??rio n??o encontrado.", true);
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Disciplina" }));
        }
    }
}