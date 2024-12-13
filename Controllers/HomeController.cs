using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExpediFlow.Models;
using ExpediFlow.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;




public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Microsoft.AspNetCore.Identity.UserManager<Usuario> _userManager;
    private readonly DBContext _context;

    public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Identity.UserManager<Usuario> userManager, DBContext context)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Bienvenido()
    {
        return View();
    }
    public async Task<IActionResult> IndexAsync()
    {

        var userName = User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(userName);
        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || !roles.Any())
        {

            return RedirectToAction("Bienvenido");
        }


        try
        {

            var viewModel = new InicioFiltros
            {
                Profile = new ProfileViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return NotFound();
        }
    }



    public IActionResult NoPermissionAccess()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}

