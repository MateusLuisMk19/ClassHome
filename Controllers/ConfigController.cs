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
    public class ConfigController : Controller
    {
        private readonly ClassHomedbContext _context;

        public ConfigController(ClassHomedbContext context)
        {
            this._context = context;
        }

        [Authorize]
        public async Task<IActionResult> Config()
        {
            return View(await _context.Configs.OrderBy(x => x.Id).AsNoTracking().ToListAsync());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Criar(int? id)
        {
            if (id > 0)
            {
                var conf = await _context.Configs.FindAsync(id);
                if (conf == null)
                {
                    return NotFound();
                }
                return View(conf);
            }
            return View(new Config());
        }

        private bool ConfigExiste(int id)
        {
            return _context.Configs.Any(x => x.Id == id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar(int? id, [FromForm] Config config)
        {

            if (id > 0)
            {
                if (ConfigExiste(id.Value))
                {
                    var conf = _context.Configs.FirstOrDefault(x => x.Id == id.Value);

                    conf.Fundo = config.Fundo;
                    conf.Texto = config.Texto;
                    conf.CardStyle = config.CardStyle;
                    conf.CardStyleCab = config.CardStyleCab;
                    conf.BTNin = config.BTNin;
                    conf.BTNoff = config.BTNoff;
                    conf.Menu = config.Menu;
                    conf.Nome = config.Nome;
                    conf.Tabela = config.Tabela;
                    conf.TabelaText = config.TabelaText;
                    conf.Link = config.Link;
                    conf.TitleText = config.TitleText;
                    conf.CardCab = config.CardCab;
                    conf.DCPBannerStyle = config.DCPBannerStyle;
                    conf.DCPBoxColor = config.DCPBoxColor;
                    conf.DCPBoxText = config.DCPBoxText;

                    _context.Configs.Update(conf);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        this.MostrarMensagem("Config editada.");
                    }
                    else
                    {
                        this.MostrarMensagem("Erro ao editar config.", true);
                    }
                }
            }
            else
            {
                _context.Configs.Add(config);
                if (await _context.SaveChangesAsync() > 0)
                {
                    this.MostrarMensagem("Nova config criada.");
                }
                else
                {
                    this.MostrarMensagem("Erro ao criar config.", true);
                }
            }
            return RedirectToAction(nameof(Config));

        }

        [Authorize]
        public IActionResult Ativar(int id)
        {
            var config = _context.Configs.FirstOrDefault(x => x.Id == id);
            var CList = _context.Configs.OrderBy(x => x.Id).Where(x => x.Id != config.Id).AsNoTracking().ToList();

            foreach (var conf in CList)
            {
                conf.Estado = Estado.Desativado;
                _context.Configs.Update(conf);
                _context.SaveChangesAsync();
            }

            config.Estado = Estado.Ativado;
            _context.Configs.Update(config);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(Config));

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!(id > 0))
            {
                this.MostrarMensagem("Config não informada.", true);
                return RedirectToAction(nameof(Config));
            }

            var config = await _context.Configs.FindAsync(id);
            if (config == null)
            {
                this.MostrarMensagem("Config não encontrada.", true);
                return RedirectToAction(nameof(Config));
            }

            return View(config);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var config = await _context.Configs.FindAsync(id);
            if (config != null)
            {
                _context.Configs.Remove(config);
                if (await _context.SaveChangesAsync() > 0)
                    this.MostrarMensagem("Config excluída.");
                else
                    this.MostrarMensagem("Não foi possível excluir a config.", true);

                return RedirectToAction(nameof(Config));

            }
            else
            {
                this.MostrarMensagem("Config não encontrada.", true);
                return RedirectToAction(nameof(Config));
            }
        }
    }
}