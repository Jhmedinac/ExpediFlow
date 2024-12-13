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
    public class ExpedienteController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ExpedienteController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Expediente
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Expediente> registros;
            if (filter != null)
            {
                registros = await _context.Expedientes.Where(r => r.IdEntidadResponsableNavigation.Nombre.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.Expedientes.ToListAsync();
            }
            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var dBContext = _context.Expedientes.Include(e => e.IdDepartamentoNavigation).Include(e => e.IdEntidadRepresentanteNavigation).Include(e => e.IdEntidadResponsableNavigation).Include(e => e.IdEntidadSolicitanteNavigation).Include(e => e.IdEstadoActualNavigation).Include(e => e.IdMunicipioNavigation).Include(e => e.IdSubTramiteNavigation).Include(e => e.IdTramiteNavigation);
            var IdDepartamentoNavigation = await _context.Departamentos.ToListAsync();
            var IdEntidadRepresentanteNavigation = await _context.Entidads.ToListAsync();
            var IdEntidadResponsableNavigation = await _context.Entidads.ToListAsync();
            var IdEntidadSolicitanteNavigation = await _context.Entidads.ToListAsync();
            var IdEstadoActualNavigation = await _context.Estados.ToListAsync();
            var IdMunicipioNavigation = await _context.Municipios.ToListAsync();
            var IdSubTramiteNavigation = await _context.SubTramites.ToListAsync();
            var IdTramiteNavigation = await _context.Tramites.ToListAsync();

            return View(data);
        }
         public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Expediente>? data = null;
             if (data == null)
             {
                data = _context.Expedientes.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Expedientes.xlsx";
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
        // GET: Expediente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdEntidadRepresentanteNavigation)
                .Include(e => e.IdEntidadResponsableNavigation)
                .Include(e => e.IdEntidadSolicitanteNavigation)
                .Include(e => e.IdEstadoActualNavigation)
                .Include(e => e.IdMunicipioNavigation)
                .Include(e => e.IdSubTramiteNavigation)
                .Include(e => e.IdTramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdExpediente == id);
            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        // GET: Expediente/Create
        public IActionResult Create()
        {
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["IdEntidadRepresentante"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre");
            ViewData["IdEntidadResponsable"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre");
            ViewData["IdEntidadSolicitante"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre");
            ViewData["IdEstadoActual"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado");
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre");
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo");
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "Codigo");
            return View();
        }

        // POST: Expediente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExpediente,NumExpediente,IdTramite,IdSubTramite,IdEntidadSolicitante,IdEntidadResponsable,IdEntidadRepresentante,FechaIngreso,IdDepartamento,IdMunicipio,IdEstadoActual,FechaEstadoActual,IdUsuarioAsignado,IdResolucion,IdDictamen,IdArchivo,Observaciones,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(expediente, true);
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", expediente.IdDepartamento);
            ViewData["IdEntidadRepresentante"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadRepresentante);
            ViewData["IdEntidadResponsable"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadResponsable);
            ViewData["IdEntidadSolicitante"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadSolicitante);
            ViewData["IdEstadoActual"] = new SelectList(_context.Estados, "IdEstado", "CreadoPor", expediente.IdEstadoActual);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", expediente.IdMunicipio);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", expediente.IdSubTramite);
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "Codigo", expediente.IdTramite);
            return View(expediente);
        }

        // GET: Expediente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", expediente.IdDepartamento);
            ViewData["IdEntidadRepresentante"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre", expediente.IdEntidadRepresentante);
            ViewData["IdEntidadResponsable"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre", expediente.IdEntidadResponsable);
            ViewData["IdEntidadSolicitante"] = new SelectList(_context.Entidads, "IdEntidad", "Nombre", expediente.IdEntidadSolicitante);
            ViewData["IdEstadoActual"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", expediente.IdEstadoActual);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", expediente.IdMunicipio);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", expediente.IdSubTramite);
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "Codigo", expediente.IdTramite);
            return View(expediente);
        }

        // POST: Expediente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExpediente,NumExpediente,IdTramite,IdSubTramite,IdEntidadSolicitante,IdEntidadResponsable,IdEntidadRepresentante,FechaIngreso,IdDepartamento,IdMunicipio,IdEstadoActual,FechaEstadoActual,IdUsuarioAsignado,IdResolucion,IdDictamen,IdArchivo,Observaciones,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Expediente expediente)
        {
            if (id != expediente.IdExpediente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(expediente, false);
                    _context.Update(expediente);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpedienteExists(expediente.IdExpediente))
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
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", expediente.IdDepartamento);
            ViewData["IdEntidadRepresentante"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadRepresentante);
            ViewData["IdEntidadResponsable"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadResponsable);
            ViewData["IdEntidadSolicitante"] = new SelectList(_context.Entidads, "IdEntidad", "CreadoPor", expediente.IdEntidadSolicitante);
            ViewData["IdEstadoActual"] = new SelectList(_context.Estados, "IdEstado", "CreadoPor", expediente.IdEstadoActual);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipios, "IdMunicipio", "Nombre", expediente.IdMunicipio);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", expediente.IdSubTramite);
            ViewData["IdTramite"] = new SelectList(_context.Tramites, "IdTramite", "Codigo", expediente.IdTramite);
            return View(expediente);
        }

        // GET: Expediente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes
                .Include(e => e.IdDepartamentoNavigation)
                .Include(e => e.IdEntidadRepresentanteNavigation)
                .Include(e => e.IdEntidadResponsableNavigation)
                .Include(e => e.IdEntidadSolicitanteNavigation)
                .Include(e => e.IdEstadoActualNavigation)
                .Include(e => e.IdMunicipioNavigation)
                .Include(e => e.IdSubTramiteNavigation)
                .Include(e => e.IdTramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdExpediente == id);
            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        // POST: Expediente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var expediente = await _context.Expedientes.FindAsync(id);
            try
            {
               
                if (expediente != null)
                {
                    _context.Expedientes.Remove(expediente);
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
                return View(expediente);
            }

        }

        private bool ExpedienteExists(int id)
        {
            return _context.Expedientes.Any(e => e.IdExpediente == id);
        }
        
        private void SetCamposAuditoria(Expediente record, bool bNewRecord)
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
