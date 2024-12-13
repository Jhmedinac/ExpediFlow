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
    public class MunicipioController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public MunicipioController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Municipio
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Municipio> registros;
            if (filter != null)
            {
                registros = await _context.Municipios.Where(r => r.Nombre.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Municipios.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.Municipios.Include(m => m.IdDepartamentoNavigation);
            var IdDepartamentoNavigation = await _context.Departamentos.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Municipio>? data = null;
             if (data == null)
             {
                data = _context.Municipios.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Municipios.xlsx";
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
        // GET: Municipio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios
                .Include(m => m.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.IdMunicipio == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // GET: Municipio/Create
        public IActionResult Create()
        {
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            return View();
        }

        // POST: Municipio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMunicipio,Nombre,IdDepartamento")] Municipio municipio)
        {
            if (ModelState.IsValid)
            {
               // SetCamposAuditoria(municipio, false);
                _context.Add(municipio);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", municipio.IdDepartamento);
            return View(municipio);
        }

        // GET: Municipio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios.FindAsync(id);
            if (municipio == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", municipio.IdDepartamento);
            return View(municipio);
        }

        // POST: Municipio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMunicipio,Nombre,IdDepartamento")] Municipio municipio)
        {
            if (id != municipio.IdMunicipio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  //  SetCamposAuditoria(municipio, false);
                    _context.Update(municipio);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MunicipioExists(municipio.IdMunicipio))
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
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", municipio.IdDepartamento);
            return View(municipio);
        }

        // GET: Municipio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipios
                .Include(m => m.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.IdMunicipio == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // POST: Municipio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var municipio = await _context.Municipios.FindAsync(id);
            try
            {
               
                if (municipio != null)
                {
                    _context.Municipios.Remove(municipio);
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
                return View(municipio);
            }

        }

        private bool MunicipioExists(int id)
        {
            return _context.Municipios.Any(e => e.IdMunicipio == id);
        }
        
        //private void SetCamposAuditoria(Municipio record, bool bNewRecord)
        //{
        //    var now = DateTime.Now;
        //    var CurrentUser =  _userManager.GetUserName(User);
           
        //    if (bNewRecord)
        //    {
        //        record.FechaCreacion = now;
        //        record.CreadoPor = CurrentUser;
        //        record.FechaModificacion = now;
        //        record.ModificadoPor = CurrentUser;
        //        record.Activo = true;
        //    }
        //    else
        //    {
        //        record.FechaModificacion = now;
        //        record.ModificadoPor = CurrentUser;
        //    }
        //}        
    }
}
