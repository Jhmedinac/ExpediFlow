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
    public class TipoEstadoController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public TipoEstadoController(DBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TipoEstado
        public async Task<IActionResult> Index(int pg, string? filter)
        {
            List<TipoEstado> registros;
            if (filter != null)
            {
                registros = await _context.TipoEstados.Where(r => r.NombreTipoEstado.ToLower().Contains(filter.ToLower())).ToListAsync();
            }
            else
            {
                registros = await _context.TipoEstados.ToListAsync();
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
             List<TipoEstado>? data = null;
             if (data == null)
             {
                data = _context.TipoEstados.ToList();
             }
             DataTable table = converter.ToDataTable(data);
             string fileName = "TipoEstados.xlsx";
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
        // GET: TipoEstado/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEstado = await _context.TipoEstados
                .FirstOrDefaultAsync(m => m.IdTipoEstado == id);
            if (tipoEstado == null)
            {
                return NotFound();
            }

            return View(tipoEstado);
        }

        // GET: TipoEstado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoEstado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoEstado,NombreTipoEstado,Notificar,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] TipoEstado tipoEstado)
        {
            if (ModelState.IsValid)
            {
                SetCamposAuditoria(tipoEstado, true);
                _context.Add(tipoEstado);
                await _context.SaveChangesAsync();
                TempData["success"] = "El registro ha sido creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["error"] = "Error: " + message;
            }
            return View(tipoEstado);
        }

        // GET: TipoEstado/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEstado = await _context.TipoEstados.FindAsync(id);
            if (tipoEstado == null)
            {
                return NotFound();
            }
            return View(tipoEstado);
        }

        // POST: TipoEstado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdTipoEstado,NombreTipoEstado,Notificar,Activo,FechaCreacion,FechaModificacion,CreadoPor,ModificadoPor")] TipoEstado tipoEstado)
        {
            if (id != tipoEstado.IdTipoEstado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetCamposAuditoria(tipoEstado, false);
                    _context.Update(tipoEstado);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "El registro ha actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoEstadoExists(tipoEstado.IdTipoEstado))
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
            return View(tipoEstado);
        }

        // GET: TipoEstado/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEstado = await _context.TipoEstados
                .FirstOrDefaultAsync(m => m.IdTipoEstado == id);
            if (tipoEstado == null)
            {
                return NotFound();
            }

            return View(tipoEstado);
        }

        // POST: TipoEstado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
             var tipoEstado = await _context.TipoEstados.FindAsync(id);
            try
            {
               
                if (tipoEstado != null)
                {
                    _context.TipoEstados.Remove(tipoEstado);
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
                return View(tipoEstado);
            }

        }

        private bool TipoEstadoExists(short id)
        {
            return _context.TipoEstados.Any(e => e.IdTipoEstado == id);
        }
        
        private void SetCamposAuditoria(TipoEstado record, bool bNewRecord)
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
