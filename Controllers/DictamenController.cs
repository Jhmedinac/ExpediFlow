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
    public class DictamenController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public DictamenController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dictamen
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Dictaman> registros;
            if (filter != null)
            {
                registros = await _context.Dictamen.Where(r => r.NumeroDictamen.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Dictamen.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.Dictamen.Include(d => d.IdExpedienteNavigation);
            var IdExpediente = _context.Expedientes.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Dictaman>? data = null;
             if (data == null)
             {
                data = _context.Dictamen.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Dictamen.xlsx";
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
        // GET: Dictamen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dictaman = await _context.Dictamen
                .Include(d => d.IdExpedienteNavigation)
                .FirstOrDefaultAsync(m => m.IdDictamen == id);
            if (dictaman == null)
            {
                return NotFound();
            }

            return View(dictaman);
        }

        // GET: Dictamen/Create
        public IActionResult Create()
        {
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "NumExpediente");
            return View();
        }

        // POST: Dictamen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDictamen,IdExpediente,NumeroDictamen,FechaDictamen,Justificacion,IdUsuarioDictamen,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Dictaman dictaman)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(dictaman, true);
                _context.Add(dictaman);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "NumExpediente", dictaman.IdExpediente);
            return View(dictaman);
        }

        // GET: Dictamen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dictaman = await _context.Dictamen.FindAsync(id);
            if (dictaman == null)
            {
                return NotFound();
            }
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "NumExpediente", dictaman.IdExpediente);
            return View(dictaman);
        }

        // POST: Dictamen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDictamen,IdExpediente,NumeroDictamen,FechaDictamen,Justificacion,IdUsuarioDictamen,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Dictaman dictaman)
        {
            if (id != dictaman.IdDictamen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(dictaman, false);
                    _context.Update(dictaman);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DictamanExists(dictaman.IdDictamen))
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
            ViewData["IdExpediente"] = new SelectList(_context.Expedientes, "IdExpediente", "CreadoPor", dictaman.IdExpediente);
            return View(dictaman);
        }

        // GET: Dictamen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dictaman = await _context.Dictamen
                .Include(d => d.IdExpedienteNavigation)
                .FirstOrDefaultAsync(m => m.IdDictamen == id);
            if (dictaman == null)
            {
                return NotFound();
            }

            return View(dictaman);
        }

        // POST: Dictamen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var dictaman = await _context.Dictamen.FindAsync(id);
            try
            {
               
                if (dictaman != null)
                {
                    _context.Dictamen.Remove(dictaman);
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
                return View(dictaman);
            }

        }

        private bool DictamanExists(int id)
        {
            return _context.Dictamen.Any(e => e.IdDictamen == id);
        }
        
        private void SetCamposAuditoria(Dictaman record, bool bNewRecord)
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
