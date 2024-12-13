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
    public class SubTramiteController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public SubTramiteController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SubTramite
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<SubTramite> registros;
            if (filter != null)
            {
                registros = await _context.SubTramites.Where(r => r.NombreSubTramite.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.SubTramites.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var IdTramites = await _context.Tramites.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<SubTramite>? data = null;
             if (data == null)
             {
                data = _context.SubTramites.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "SubTramites.xlsx";
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
        // GET: SubTramite/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramite = await _context.SubTramites
                .FirstOrDefaultAsync(m => m.IdSubTramite == id);
            if (subTramite == null)
            {
                return NotFound();
            }

            return View(subTramite);
        }

        // GET: SubTramite/Create
        public IActionResult Create()
        {
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "NombreTramite");
            return View();
        }

        // POST: SubTramite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubTramite,NombreSubTramite,IdTramite,Codigo,Costo,CodigoFinanzas,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] SubTramite subTramite)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(subTramite, true);
                _context.Add(subTramite);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "NombreTramite");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            return View(subTramite);
        }

        // GET: SubTramite/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramite = await _context.SubTramites.FindAsync(id);
            if (subTramite == null)
            {
                return NotFound();
            }
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "NombreTramite",subTramite.IdTramite);
            return View(subTramite);
        }

        // POST: SubTramite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSubTramite,NombreSubTramite,IdTramite,Codigo,Costo,CodigoFinanzas,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] SubTramite subTramite)
        {
            if (id != subTramite.IdSubTramite)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(subTramite, false);
                    _context.Update(subTramite);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubTramiteExists(subTramite.IdSubTramite))
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
            return View(subTramite);
        }

        // GET: SubTramite/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTramite = await _context.SubTramites
                .FirstOrDefaultAsync(m => m.IdSubTramite == id);
            if (subTramite == null)
            {
                return NotFound();
            }

            return View(subTramite);
        }

        // POST: SubTramite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var subTramite = await _context.SubTramites.FindAsync(id);
            try
            {
               
                if (subTramite != null)
                {
                    _context.SubTramites.Remove(subTramite);
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
                return View(subTramite);
            }

        }

        private bool SubTramiteExists(int id)
        {
            return _context.SubTramites.Any(e => e.IdSubTramite == id);
        }
        
        private void SetCamposAuditoria(SubTramite record, bool bNewRecord)
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
