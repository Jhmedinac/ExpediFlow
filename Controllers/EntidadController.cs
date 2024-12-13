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
    public class EntidadController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public EntidadController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entidad
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Entidad> registros;
            if (filter != null)
            {
                registros = await _context.Entidads.Where(r => r.Nombre.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Entidads.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.Entidads.Include(e => e.IdDepartamentoNavigation).Include(e => e.IdMunicipioNavigation).Include(e => e.IdTipoEntidadNavigation);
            var IdDepartamentoNavigation = await _context.Departamentos.ToListAsync();
            var IdMunicipioNavigation = await _context.Municipios.ToListAsync();
            var IdTipoEntidadNavigation = await _context.TipoEntidads.ToListAsync();
            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Entidad>? data = null;
             if (data == null)
             {
                data = _context.Entidads.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Entidads.xlsx";
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
        // GET: Entidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entidad = await _context.Entidads
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdMunicipioNavigation)
                .Include(e => e.IdTipoEntidadNavigation)
                .FirstOrDefaultAsync(m => m.IdEntidad == id);
            if (entidad == null)
            {
                return NotFound();
            }

            return View(entidad);
        }

        // GET: Entidad/Create
        public IActionResult Create()
        {
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre");
            ViewData["IdTipoEntidad"] = new SelectList(_context.TipoEntidads, "IdTipoEntidad", "NombreTipoEntidad");
            return View();
        }

        // POST: Entidad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEntidad,IdTipoEntidad,Dni,Rtn,Codigo,Nombre,Direccion,Telefono,Email,IdDepartamento,IdMunicipio,Observaciones,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Entidad entidad)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(entidad, true);
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", entidad.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", entidad.IdMunicipio);
            ViewData["IdTipoEntidad"] = new SelectList(_context.TipoEntidads, "IdTipoEntidad", "NombreTipoEntidad", entidad.IdTipoEntidad);
            return View(entidad);
        }

        // GET: Entidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entidad = await _context.Entidads.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", entidad.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", entidad.IdMunicipio);
            ViewData["IdTipoEntidad"] = new SelectList(_context.TipoEntidads, "IdTipoEntidad", "NombreTipoEntidad", entidad.IdTipoEntidad);
            return View(entidad);
        }

        // POST: Entidad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEntidad,IdTipoEntidad,Dni,Rtn,Codigo,Nombre,Direccion,Telefono,Email,IdDepartamento,IdMunicipio,Observaciones,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Entidad entidad)
        {
            if (id != entidad.IdEntidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(entidad, false);
                    _context.Update(entidad);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntidadExists(entidad.IdEntidad))
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
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", entidad.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", entidad.IdMunicipio);
            ViewData["IdTipoEntidad"] = new SelectList(_context.TipoEntidads, "IdTipoEntidad", "NombreTipoEntidad", entidad.IdTipoEntidad);
            return View(entidad);
        }

        // GET: Entidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entidad = await _context.Entidads
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdMunicipioNavigation)
                .Include(e => e.IdTipoEntidadNavigation)
                .FirstOrDefaultAsync(m => m.IdEntidad == id);
            if (entidad == null)
            {
                return NotFound();
            }

            return View(entidad);
        }

        // POST: Entidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var entidad = await _context.Entidads.FindAsync(id);
            try
            {
               
                if (entidad != null)
                {
                    _context.Entidads.Remove(entidad);
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
                return View(entidad);
            }

        }

        private bool EntidadExists(int id)
        {
            return _context.Entidads.Any(e => e.IdEntidad == id);
        }
        
        private void SetCamposAuditoria(Entidad record, bool bNewRecord)
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
