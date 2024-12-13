using System.Data;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static ExpediFlow.cGeneralFun;
using Microsoft.AspNetCore.Identity;
using ExpediFlow.ViewModel;

using ExpediFlow.Models;

namespace ExpediFlow.Controllers
{
    public class SubTramiteRequisitoController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public SubTramiteRequisitoController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SubTramiteRequisito
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<SubTramiteRequisito> registros;
            if (filter != null)
            {
                registros = await _context.SubTramiteRequisitos.Where(r => r.IdSubtramiteNavigation.NombreSubTramite.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.SubTramiteRequisitos.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.SubTramiteRequisitos.Include(s => s.IdRequisitoNavigation).Include(s => s.IdSubtramiteNavigation);
            var IdRequisitoNavigation = await _context.Requisitos.ToListAsync();
            var IdSubtramiteNavigation = await _context.SubTramites.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<SubTramiteRequisito>? data = null;
             if (data == null)
             {
                data = _context.SubTramiteRequisitos.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "SubTramiteRequisitos.xlsx";
             using (XLWorkbook wb = new XLWorkbook())
             {
                 wb.Worksheets.Add(table);
                 using (MemoryStream stream = new MemoryStream())
                 {
                     wb.SaveAs(stream);
                     return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                 }
             }
        }    
        // GET: SubTramiteRequisito/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramiteRequisito = await _context.SubTramiteRequisitos
                .Include(s => s.IdRequisitoNavigation)
                .Include(s => s.IdSubtramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdRequisitoSubTramite == id);
            if (subTramiteRequisito == null)
            {
                return NotFound();
            }

            return View(subTramiteRequisito);
        }

        // GET: SubTramiteRequisito/Create
        public IActionResult Create()
        {
            ViewData["IdRequisito"] = new SelectList(_context.Requisitos, "IdRequisito", "NombreRequisito");
            ViewData["IdSubtramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo");
            return View();
        }

        // POST: SubTramiteRequisito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRequisitoSubTramite,IdSubtramite,IdRequisito,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] SubTramiteRequisito subTramiteRequisito)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(subTramiteRequisito, true);
                _context.Add(subTramiteRequisito);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdRequisito"] = new SelectList(_context.Requisitos, "IdRequisito", "NombreRequisito", subTramiteRequisito.IdRequisito);
            ViewData["IdSubtramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", subTramiteRequisito.IdSubtramite);
            return View(subTramiteRequisito);
        }

        // GET: SubTramiteRequisito/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramiteRequisito = await _context.SubTramiteRequisitos.FindAsync(id);
            if (subTramiteRequisito == null)
            {
                return NotFound();
            }
            ViewData["IdRequisito"] = new SelectList(_context.Requisitos, "IdRequisito", "NombreRequisito", subTramiteRequisito.IdRequisito);
            ViewData["IdSubtramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", subTramiteRequisito.IdSubtramite);
            return View(subTramiteRequisito);
        }

        // POST: SubTramiteRequisito/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRequisitoSubTramite,IdSubtramite,IdRequisito,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] SubTramiteRequisito subTramiteRequisito)
        {
            if (id != subTramiteRequisito.IdRequisitoSubTramite)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(subTramiteRequisito, false);
                    _context.Update(subTramiteRequisito);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubTramiteRequisitoExists(subTramiteRequisito.IdRequisitoSubTramite))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }            
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = "Error: " + message;
            }
            ViewData["IdRequisito"] = new SelectList(_context.Requisitos, "IdRequisito", "NombreRequisito", subTramiteRequisito.IdRequisito);
            ViewData["IdSubtramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", subTramiteRequisito.IdSubtramite);
            return View(subTramiteRequisito);
        }

        // GET: SubTramiteRequisito/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramiteRequisito = await _context.SubTramiteRequisitos
                .Include(s => s.IdRequisitoNavigation)
                .Include(s => s.IdSubtramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdRequisitoSubTramite == id);
            if (subTramiteRequisito == null)
            {
                return NotFound();
            }

            return View(subTramiteRequisito);
        }

        // POST: SubTramiteRequisito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var subTramiteRequisito = await _context.SubTramiteRequisitos.FindAsync(id);
            try
            {
               
                if (subTramiteRequisito != null)
                {
                    _context.SubTramiteRequisitos.Remove(subTramiteRequisito);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha sido eliminado exitosamente.";
                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                    TempData["Error"] = "Hubo un error al intentar eliminar el Empleado Contacto. Por favor, verifica la información e intenta nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
                {
                    TempData["error"] = "Error: No puede elimiar el registro actual ya que se encuentra relacionado a otro Registro.";
                }
                else
                {
                    var message = ex.InnerException;
                    TempData["error"] = "Error: " + message;
                }
                return View(subTramiteRequisito);
            }

        }

        private bool SubTramiteRequisitoExists(int id)
        {
            return _context.SubTramiteRequisitos.Any(e => e.IdRequisitoSubTramite == id);
        }
        
        private void SetCamposAuditoria(SubTramiteRequisito record, bool bNewRecord)
        {
            var now = DateTime.Now;
            var CurrentUser =  _userManager.GetUserName(User);
           
            if (bNewRecord)
            {
                record.FechaCreacion = now;
                record.CreadoPor = CurrentUser;
                record.FechaModificacion = now;
                record.ModificadoPor = CurrentUser;
                record.Activo = true;
            }
            else
            {
                record.FechaModificacion = now;
                record.ModificadoPor = CurrentUser;
            }
        }        
    }
}
