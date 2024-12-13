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
    public class UnidadController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public UnidadController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Unidad
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Unidad> registros;
            if (filter != null)
            {
                registros = await _context.Unidads.Where(r => r.NombreUnidad.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Unidads.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.Unidads.Include(u => u.IdDivisionDepartamentoNavigation);
            var IdDivisionDepartamentoNavigation = await _context.DivisionDepartamentos.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Unidad>? data = null;
             if (data == null)
             {
                data = _context.Unidads.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Unidads.xlsx";
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
        // GET: Unidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidads
                .Include(u => u.IdDivisionDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.IdUnidad == id);
            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // GET: Unidad/Create
        public IActionResult Create()
        {
            ViewData["IdDivisionDepartamento"] = new SelectList(_context.DivisionDepartamentos, "IdDivisionDepartamento", "NombreDepartamento");
            return View();
        }

        // POST: Unidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUnidad,NombreUnidad,IdDivisionDepartamento,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Unidad unidad)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(unidad, true);
                _context.Add(unidad);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdDivisionDepartamento"] = new SelectList(_context.DivisionDepartamentos, "IdDivisionDepartamento", "DivisionDepartamento", unidad.IdDivisionDepartamento);
            return View(unidad);
        }

        // GET: Unidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidads.FindAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }
            ViewData["IdDivisionDepartamento"] = new SelectList(_context.DivisionDepartamentos, "IdDivisionDepartamento", "NombreDepartamento", unidad.IdDivisionDepartamento);
            return View(unidad);
        }

        // POST: Unidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUnidad,NombreUnidad,IdDivisionDepartamento,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Unidad unidad)
        {
            if (id != unidad.IdUnidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(unidad, false);
                    _context.Update(unidad);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadExists(unidad.IdUnidad))
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
            ViewData["IdDivisionDepartamento"] = new SelectList(_context.DivisionDepartamentos, "IdDivisionDepartamento", "CreadoPor", unidad.IdDivisionDepartamento);
            return View(unidad);
        }

        // GET: Unidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidads
                .Include(u => u.IdDivisionDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.IdUnidad == id);
            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // POST: Unidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var unidad = await _context.Unidads.FindAsync(id);
            try
            {
               
                if (unidad != null)
                {
                    _context.Unidads.Remove(unidad);
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
                return View(unidad);
            }

        }

        private bool UnidadExists(int id)
        {
            return _context.Unidads.Any(e => e.IdUnidad == id);
        }
        
        private void SetCamposAuditoria(Unidad record, bool bNewRecord)
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
