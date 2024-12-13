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
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;

namespace ExpediFlow.Controllers
{
    public class FlujoController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public FlujoController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<Flujo> registros;
            if (filter != null)
            {
                registros = await _context.Flujos
                    .Include(f => f.IdSubTramiteNavigation)  // Incluir datos de navegación
                    .Where(r => r.NombreFlujo.ToLower().Contains(filter.ToLower()))
                    .ToListAsync();
            }
            else
            {
                registros = await _context.Flujos
                    .Include(f => f.IdSubTramiteNavigation)  // Incluir datos de navegación
                    .ToListAsync();
            }

            const int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = registros.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = registros.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            var IdEstadoInicialNavigation = _context.Estados.ToListAsync();
               var IdEstadoFinalNavigation = _context.Estados.ToListAsync();
            return View(data);
        }

        public ActionResult Download()
         {
             ListtoDataTableConverter converter = new ListtoDataTableConverter();
             List<Flujo>? data = null;
             if (data == null)
             {
                data = _context.Flujos.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "Flujos.xlsx";
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
        // GET: Flujo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flujo = await _context.Flujos
                .Include(f => f.IdEstadoFinalNavigation)
                .Include(f => f.IdEstadoInicialNavigation)
                .Include(f => f.IdSubTramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdFlujo == id);
            if (flujo == null)
            {
                return NotFound();
            }

            return View(flujo);
        }
        public async Task<IActionResult> FlowDesigner(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //       var flujo = await _context.Flujos
            //.Include(f => f.IdEstadoFinalNavigation)
            //.Include(f => f.IdEstadoInicialNavigation)
            //.Include(f => f.IdSubTramiteNavigation)
            //.Include(f => f.FlujoBloques)
            //    .ThenInclude(b => b.FlujoTransicions)
            //        .ThenInclude(t => t.IdEstadoInicialNavigation) 
            //.Include(f => f.FlujoBloques)
            //    .ThenInclude(b => b.FlujoTransicions)
            //        .ThenInclude(t => t.IdEstadoFinalNavigation)   
            //.Include(f => f.FlujoBloques)
            //    .ThenInclude(b => b.IdEstadoInicialNavigation) 
            //.Include(f => f.FlujoBloques)
            //    .ThenInclude(b => b.IdEstadoFinalNavigation)   
            //.FirstOrDefaultAsync(m => m.IdFlujo == id);
            var flujo = await _context.Flujos
           .Include(f => f.IdEstadoFinalNavigation)
           .Include(f => f.IdEstadoInicialNavigation)
           .Include(f => f.IdSubTramiteNavigation)
           .Include(f => f.FlujoBloques)
               .ThenInclude(b => b.FlujoTransicions)
                   .ThenInclude(t => t.IdEstadoInicialNavigation)
           .Include(f => f.FlujoBloques)
               .ThenInclude(b => b.FlujoTransicions)
                   .ThenInclude(t => t.IdEstadoFinalNavigation)
           .Include(f => f.FlujoBloques)
               .ThenInclude(b => b.IdEstadoInicialNavigation)
           .Include(f => f.FlujoBloques)
               .ThenInclude(b => b.IdEstadoFinalNavigation)
           .Include(f => f.FlujoBloques)
               .ThenInclude(b => b.IdUnidadNavigation)  // Aquí se incluye la relación con IdUnidadNavigation
           .FirstOrDefaultAsync(m => m.IdFlujo == id);


            if (flujo == null)
            {
                return NotFound();
            } else
            {
                flujo.FlujoBloques = flujo.FlujoBloques.OrderBy(b => b.Orden).ToList();

            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado");
            ViewData["Unidades"] = new SelectList(_context.Unidads, "IdUnidad", "NombreUnidad");
            return View(flujo);
        }

        // GET: Flujo/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoFinal"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado");
            ViewData["IdEstadoInicial"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado");
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo");
            return View();
        }

        // POST: Flujo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFlujo,NombreFlujo,IdSubTramite,Descipcion,IdEstadoInicial,IdEstadoFinal,TiempoMin,TiempoMax,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Flujo flujo)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(flujo, true);
                _context.Add(flujo);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            ViewData["IdEstadoFinal"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoFinal);
            ViewData["IdEstadoInicial"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoInicial);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", flujo.IdSubTramite);
            return View(flujo);
        }

        // GET: Flujo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flujo = await _context.Flujos.FindAsync(id);
            if (flujo == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoFinal"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoFinal);
            ViewData["IdEstadoInicial"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoInicial);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", flujo.IdSubTramite);
            return View(flujo);
        }

        // POST: Flujo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFlujo,NombreFlujo,IdSubTramite,Descipcion,IdEstadoInicial,IdEstadoFinal,TiempoMin,TiempoMax,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] Flujo flujo)
        {
            if (id != flujo.IdFlujo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(flujo, false);
                    _context.Update(flujo);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlujoExists(flujo.IdFlujo))
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
            ViewData["IdEstadoFinal"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoFinal);
            ViewData["IdEstadoInicial"] = new SelectList(_context.Estados, "IdEstado", "NombreEstado", flujo.IdEstadoInicial);
            ViewData["IdSubTramite"] = new SelectList(_context.SubTramites, "IdSubTramite", "Codigo", flujo.IdSubTramite);
            return View(flujo);
        }

        // GET: Flujo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flujo = await _context.Flujos
                .Include(f => f.IdEstadoFinalNavigation)
                .Include(f => f.IdEstadoInicialNavigation)
                .Include(f => f.IdSubTramiteNavigation)
                .FirstOrDefaultAsync(m => m.IdFlujo == id);
            if (flujo == null)
            {
                return NotFound();
            }

            return View(flujo);
        }

        // POST: Flujo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             var flujo = await _context.Flujos.FindAsync(id);
            try
            {
               
                if (flujo != null)
                {
                    _context.Flujos.Remove(flujo);
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
                return View(flujo);
            }

        }

        private bool FlujoExists(int id)
        {
            return _context.Flujos.Any(e => e.IdFlujo == id);
        }
        
        private void SetCamposAuditoria(Flujo record, bool bNewRecord)
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
        public IActionResult CrearBloque(int flujoId, int? posicion)
        {
            int Orden = 1;
            if (posicion != null)
            {
                Orden += (int)posicion;
            }
            var model = new FlujoBloque
            {
                IdFlujo = flujoId,
                Orden = Orden
            };
            ViewBag.Unidades = new SelectList(_context.Unidads, "IdUnidad", "NombreUnidad");

            return PartialView("_CrearBloqueModal", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CrearBloque(FlujoBloque bloque)
        {
            if (ModelState.IsValid)
            {
                var ultimoBloque = await _context.FlujoBloques
                    .Where(b => b.IdFlujo == bloque.IdFlujo)
                    .OrderByDescending(b => b.Orden)
                    .FirstOrDefaultAsync();
                var flujo = await _context.Flujos.FindAsync(bloque.IdFlujo);

                bloque.Orden = (ultimoBloque?.Orden ?? 0) + 1;
                
                if (ultimoBloque != null)
                {
                    bloque.IdEstadoInicial = ultimoBloque?.IdEstadoFinal;
                    bloque.IdEstadoFinal = flujo.IdEstadoFinal;
                } else
                {
                    bloque.IdEstadoInicial = flujo.IdEstadoInicial;
                    bloque.IdEstadoFinal = flujo.IdEstadoFinal;
                }

                var CurrentUser = _userManager.GetUserName(User);
                bloque.CreadoPor = CurrentUser;
                bloque.ModificadoPor = CurrentUser;
                bloque.FechaCreacion = DateTime.Now;
                bloque.FechaModificacion = DateTime.Now;
                bloque.Activo = true;

                _context.FlujoBloques.Add(bloque);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // Método para mostrar el modal de Editar Bloque
        public async Task<IActionResult> EditarBloque(int id)
        {
            var bloque = await _context.FlujoBloques.FindAsync(id);
            if (bloque == null)
            {
                return NotFound();
            }
            ViewData["Unidades"] = new SelectList(_context.Unidads, "IdUnidad", "NombreUnidad");
            return PartialView("_EditarBloqueModal", bloque);
        }

        // Método para manejar la edición de un bloque
        [HttpPost]
        public async Task<IActionResult> EditarBloque(FlujoBloque bloque)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CurrentUser = _userManager.GetUserName(User);
                    bloque.FechaModificacion = DateTime.Now;
                    bloque.ModificadoPor = CurrentUser;
                    _context.Update(bloque);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
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
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBloque(int idBloque)
        {
            var bloque = await _context.FlujoBloques.FindAsync(idBloque);
            if (bloque == null)
            {
                return Json(new { success = false });
            }
            _context.FlujoBloques.Remove(bloque);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEstado(int idFlujoTransicion)
        {
            var flujoTransicion = await _context.FlujoTransicions.FindAsync(idFlujoTransicion);
            if (flujoTransicion == null)
                return NotFound();
          
            _context.FlujoTransicions.Remove(flujoTransicion);
            await _context.SaveChangesAsync();
            await SetEstadoFinal(flujoTransicion.IdBloque);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddEstado([FromBody] EstadoViewModel request)
        {
            var block = await _context.FlujoBloques
                .Include(b => b.FlujoTransicions)
                .FirstOrDefaultAsync(b => b.IdBloque == request.BlockId);

            if (block == null)
                return NotFound();

            var ultimoEstado = await _context.FlujoTransicions
                .Where(b => b.IdBloque == request.BlockId)
                .OrderByDescending(b => b.Orden)
                .FirstOrDefaultAsync();

            var CurrentUser = _userManager.GetUserName(User);

            var estado = new FlujoTransicion
            {
                IdBloque = request.BlockId,
                Orden = (ultimoEstado?.Orden ?? 0) + 1,
                IdEstadoInicial = (int)block.IdEstadoInicial,
                IdEstadoFinal = request.IdNuevoEstado,
                CreadoPor = CurrentUser,
                ModificadoPor = CurrentUser,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                Activo = true,
                Enviar = request.Enviar,
                Recibir = request.Recibir
            };
            block.IdEstadoFinal = request.IdNuevoEstado;
            block.FlujoTransicions.Add(estado);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        public async Task SetEstadoFinal(int blockId)
        {
            var bloque = await _context.FlujoBloques.FindAsync(blockId);
            if (bloque == null)
                return; // No se hace nada si no se encuentra el bloque.

            var estadoFinal = _context.FlujoTransicions.OrderByDescending(e => e.Orden).FirstOrDefault(e => e.IdBloque == blockId);
                
            if (estadoFinal != null)
            {
                bloque.IdEstadoFinal = estadoFinal.IdEstadoFinal;
            }

            await _context.SaveChangesAsync();
        }


    }
}
