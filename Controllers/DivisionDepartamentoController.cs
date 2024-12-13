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
    public class DivisionDepartamentoController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public DivisionDepartamentoController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DivisionDepartamento
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<DivisionDepartamento> registros;
            if (filter != null)
            {
                registros = await _context.DivisionDepartamentos.Where(r => r.NombreDepartamento.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.DivisionDepartamentos.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.DivisionDepartamentos.Include(d => d.IdDivisionNavigation);
            var IdDivisionNavigation = await _context.Divisions.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<DivisionDepartamento>? data = null;
             if (data == null)
             {
                data = _context.DivisionDepartamentos.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "DivisionDepartamentos.xlsx";
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
        // GET: DivisionDepartamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var divisionDepartamento = await _context.DivisionDepartamentos
                .Include(d => d.IdDivisionNavigation)
                .FirstOrDefaultAsync(m => m.IdDivisionDepartamento == id);
            if (divisionDepartamento == null)
            {
                return NotFound();
            }

            return View(divisionDepartamento);
        }

        // GET: DivisionDepartamento/Create
        public IActionResult Create()
        {
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "NombreDivision");
            return View();
        }

        // POST: DivisionDepartamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDivisionDepartamento,NombreDepartamento,IdDivision,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] DivisionDepartamento divisionDepartamento)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(divisionDepartamento, true);
                _context.Add(divisionDepartamento);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "CreadoPor", divisionDepartamento.IdDivision);
            return View(divisionDepartamento);
        }

        // GET: DivisionDepartamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var divisionDepartamento = await _context.DivisionDepartamentos.FindAsync(id);
            if (divisionDepartamento == null)
            {
                return NotFound();
            }
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "NombreDivision", divisionDepartamento.IdDivision);
            return View(divisionDepartamento);
        }

        // POST: DivisionDepartamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDivisionDepartamento,NombreDepartamento,IdDivision,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] DivisionDepartamento divisionDepartamento)
        {
            if (id != divisionDepartamento.IdDivisionDepartamento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(divisionDepartamento, false);
                    _context.Update(divisionDepartamento);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionDepartamentoExists(divisionDepartamento.IdDivisionDepartamento))
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
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "NombreDivision", divisionDepartamento.IdDivision);
           
            return View(divisionDepartamento);
        }

        // GET: DivisionDepartamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var divisionDepartamento = await _context.DivisionDepartamentos
                .Include(d => d.IdDivisionNavigation)
                .FirstOrDefaultAsync(m => m.IdDivisionDepartamento == id);
            if (divisionDepartamento == null)
            {
                return NotFound();
            }

            return View(divisionDepartamento);
        }

        // POST: DivisionDepartamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var divisionDepartamento = await _context.DivisionDepartamentos.FindAsync(id);
            try
            {
               
                if (divisionDepartamento != null)
                {
                    _context.DivisionDepartamentos.Remove(divisionDepartamento);
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
                return View(divisionDepartamento);
            }

        }

        private bool DivisionDepartamentoExists(int id)
        {
            return _context.DivisionDepartamentos.Any(e => e.IdDivisionDepartamento == id);
        }
        
        private void SetCamposAuditoria(DivisionDepartamento record, bool bNewRecord)
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
