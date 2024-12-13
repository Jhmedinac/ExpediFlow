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
    public class ResolucionController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ResolucionController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Resolucion
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Resolucion> registros;
            if (filter != null)
            {
                registros = await _context.Resolucions.Where(r => r.NumeroResolucion.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Resolucions.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var IdExpediente = _context.Expedientes.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Resolucion>? data = null;
             if (data == null)
             {
                data = _context.Resolucions.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Resolucions.xlsx";
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
        // GET: Resolucion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolucion = await _context.Resolucions
                .FirstOrDefaultAsync(m => m.IdResolucion == id);
            if (resolucion == null)
            {
                return NotFound();
            }

            return View(resolucion);
        }

        // GET: Resolucion/Create
        public IActionResult Create()
        {
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "NumExpediente");
            return View();
        }

        // POST: Resolucion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdResolucion,IdExpediente,NumeroResolucion,FechaResolucion,Justificacion,IdUsuarioResolucion,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Resolucion resolucion)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(resolucion, true);
                _context.Add(resolucion);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            return View(resolucion);
        }

        // GET: Resolucion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolucion = await _context.Resolucions.FindAsync(id);
            if (resolucion == null)
            {
                return NotFound();
            }
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "NumExpediente", resolucion.IdExpediente);
            return View(resolucion);
        }

        // POST: Resolucion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdResolucion,IdExpediente,NumeroResolucion,FechaResolucion,Justificacion,IdUsuarioResolucion,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Resolucion resolucion)
        {
            if (id != resolucion.IdResolucion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(resolucion, false);
                    _context.Update(resolucion);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResolucionExists(resolucion.IdResolucion))
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
            return View(resolucion);
        }

        // GET: Resolucion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolucion = await _context.Resolucions
                .FirstOrDefaultAsync(m => m.IdResolucion == id);
            if (resolucion == null)
            {
                return NotFound();
            }

            return View(resolucion);
        }

        // POST: Resolucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var resolucion = await _context.Resolucions.FindAsync(id);
            try
            {
               
                if (resolucion != null)
                {
                    _context.Resolucions.Remove(resolucion);
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
                return View(resolucion);
            }

        }

        private bool ResolucionExists(int id)
        {
            return _context.Resolucions.Any(e => e.IdResolucion == id);
        }
        
        private void SetCamposAuditoria(Resolucion record, bool bNewRecord)
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
