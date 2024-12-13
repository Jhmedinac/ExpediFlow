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
    public class TipoEntidadController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TipoEntidadController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TipoEntidad
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<TipoEntidad> registros;
            if (filter != null)
            {
                registros = await _context.TipoEntidads.Where(r => r.NombreTipoEntidad.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.TipoEntidads.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<TipoEntidad>? data = null;
             if (data == null)
             {
                data = _context.TipoEntidads.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "TipoEntidads.xlsx";
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
        // GET: TipoEntidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEntidad = await _context.TipoEntidads
                .FirstOrDefaultAsync(m => m.IdTipoEntidad == id);
            if (tipoEntidad == null)
            {
                return NotFound();
            }

            return View(tipoEntidad);
        }

        // GET: TipoEntidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoEntidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoEntidad,NombreTipoEntidad,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] TipoEntidad tipoEntidad)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(tipoEntidad, true);
                _context.Add(tipoEntidad);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            return View(tipoEntidad);
        }

        // GET: TipoEntidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEntidad = await _context.TipoEntidads.FindAsync(id);
            if (tipoEntidad == null)
            {
                return NotFound();
            }
            return View(tipoEntidad);
        }

        // POST: TipoEntidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoEntidad,NombreTipoEntidad,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] TipoEntidad tipoEntidad)
        {
            if (id != tipoEntidad.IdTipoEntidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(tipoEntidad, false);
                    _context.Update(tipoEntidad);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoEntidadExists(tipoEntidad.IdTipoEntidad))
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
            return View(tipoEntidad);
        }

        // GET: TipoEntidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEntidad = await _context.TipoEntidads
                .FirstOrDefaultAsync(m => m.IdTipoEntidad == id);
            if (tipoEntidad == null)
            {
                return NotFound();
            }

            return View(tipoEntidad);
        }

        // POST: TipoEntidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var tipoEntidad = await _context.TipoEntidads.FindAsync(id);
            try
            {
               
                if (tipoEntidad != null)
                {
                    _context.TipoEntidads.Remove(tipoEntidad);
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
                return View(tipoEntidad);
            }

        }

        private bool TipoEntidadExists(int id)
        {
            return _context.TipoEntidads.Any(e => e.IdTipoEntidad == id);
        }
        
        private void SetCamposAuditoria(TipoEntidad record, bool bNewRecord)
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
