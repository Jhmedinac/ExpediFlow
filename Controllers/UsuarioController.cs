﻿using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using ExpediFlow.Models;
using ExpediFlow.Servicio;
using ExpediFlow.Servicio;
using ExpediFlow.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Packaging;

namespace ExpediFlow.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly DBContext context;
        private readonly EmailService emailService;
        private IWebHostEnvironment Environment;


        public UsuarioController(UserManager<Usuario> userManager,
         SignInManager<Usuario> signInManager,
         DBContext context,
         EmailService emailService,
         IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.emailService = emailService;
            Environment = environment;
        }

        [AllowAnonymous]
        public IActionResult RegistrarseConfirmación()
        {
            return View();
        }



        [AllowAnonymous]
        public IActionResult DiaFestivo()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Registro(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }


            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(Registro modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Verificar si las contraseñas coinciden
            if (modelo.Password != modelo.RetypePassword)
            {
                ViewData["mensaje"] = "Las contraseñas no coinciden.";
                return View(modelo);
            }

            var usuarioExistente = await context.Users.FirstOrDefaultAsync(u => u.Email == modelo.Email);
            if (usuarioExistente != null)
            {
                ViewData["mensaje"] = "El correo electrónico ya está registrado.";
                return View(modelo);
            }


            var usuario = new Usuario()
            {
                Email = modelo.Email,
                UserName = modelo.Email,
                PhoneNumber = modelo.PhoneNumber,
                EmailConfirmed = true
            };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: false);
                ViewData["mensaje"] = "Registro exitoso. Has iniciado sesión automáticamente.";

                // Obtener los roles del usuario
                var userName = usuario.UserName;
                var usermenu = await userManager.FindByNameAsync(userName);
                var rolesmenu = await userManager.GetRolesAsync(usermenu);

                if (rolesmenu == null || !rolesmenu.Any())
                {
                    return RedirectToAction("Bienvenido", "Home");
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ViewData["Error"] = "Error al registrarse";
            }

            return View(modelo);
        }


        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Registro(Registro modelo)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(modelo);
        //    }

        //    // Verificar si las contraseñas coinciden
        //    if (modelo.Password != modelo.RetypePassword)
        //    {
        //        ViewData["mensaje"] = "Las contraseñas no coinciden.";
        //        return View(modelo);
        //    }

        //    // Verificar si el correo ya existe
        //    var usuarioExistente = await userManager.FindByEmailAsync(modelo.Email);
        //    if (usuarioExistente != null)
        //    {
        //        ViewData["mensaje"] = "El correo electrónico ya está registrado.";
        //        return View(modelo);
        //    }

        //    var usuario = new Usuario() { Email = modelo.Email, UserName = modelo.Email, PhoneNumber = modelo.PhoneNumber, EmailConfirmed = true };
        //    var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

        //    if (resultado.Succeeded)
        //    {
        //        var userId = await userManager.GetUserIdAsync(usuario);
        //        var code = await userManager.GenerateEmailConfirmationTokenAsync(usuario);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //        // URL de confirmación personalizada
        //        var callbackUrl = Url.Action("ConfirmarEmail", "Usuario", new { userId = userId, code = code }, protocol: Request.Scheme);


        //        // Verificar que el valor de 'modelo.Email' no sea null antes de enviar el correo electrónico
        //        if (modelo.Email != null)
        //        {
        //            try
        //            {
        //                // Enviar correo electrónico de confirmación
        //                await emailService.EnviarEmail(modelo.Email, "Confirmación de correo electrónico", $"Por favor, confirma tu correo electrónico haciendo clic <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a>.");

        //                if (userManager.Options.SignIn.RequireConfirmedAccount)
        //                {
        //                    ViewData["mensaje"] = "Registro exitoso. Por favor, confirma tu correo electrónico para iniciar sesión";
        //                    return RedirectToAction("RegistrarseConfirmación", "Usuario");

        //                }

        //               else
        //                {
        //                    await signInManager.SignInAsync(usuario, isPersistent: false);
        //                    ViewData["mensaje"] = "Registro exitoso. Has iniciado sesión automáticamente.";
        //                    return RedirectToAction("Index", "Home"); // Ajusta la ruta según tus necesidades
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                // Registra el error o imprímelo en la consola para depuración
        //                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");



        //                ViewData["mensaje"] = "Usuario no registrado. Hubo un problema al enviar el correo electrónico. Por favor, intenta registrarte de nuevo o contacta al soporte.";
        //                return View(modelo);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var error in resultado.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }  
        //    }

        //    return View(modelo);
        //}


        [AllowAnonymous]
        public async Task<IActionResult> ConfirmarEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                // Manejar el caso en el que userId o code son nulos
                ViewData["mensaje"] = "El userId o code son nulos";
                return View("Error");
            }

            var usuario = await userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
                // Manejar el caso en el que el usuario no se encuentra
                ViewData["mensaje"] = "El usuario no se encuentra";
                return View("Error");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var resultado = await userManager.ConfirmEmailAsync(usuario, code);

            if (resultado.Succeeded)
            {
                // Actualizar la propiedad [EmailConfirmed] del usuario en la base de datos
                usuario.EmailConfirmed = true;
                await userManager.UpdateAsync(usuario);

                ViewData["mensaje"] = "Correo electrónico confirmado con éxito. ¡Bienvenido!";
                return View("ConfirmarEmail"); // O tu vista de confirmación personalizada
            }
            else
            {
                // Manejar el caso en el que la confirmación falla
                ViewData["mensaje"] = "La confirmación falló";
                return View("Error");
            }
        }



        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Models.Login modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await userManager.FindByEmailAsync(modelo.Email);

            if (usuario != null && !await userManager.IsEmailConfirmedAsync(usuario))
            {
                // El correo electrónico del usuario no está verificado
                ViewData["mensaje"] = "Tu correo electrónico aún no ha sido verificado. Por favor, verifica tu correo electrónico y vuelve a intentarlo.";
                return View(modelo);
            }
            try
            {
                var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    // Inicio de sesión exitoso
                    return RedirectToAction("Index", "Home");
                }
                else if (resultado.IsLockedOut)
                {
                    // Usuario bloqueado
                    ViewData["mensaje"] = "La cuenta está bloqueada. Por favor, inténtalo nuevamente más tarde.";
                    return View(modelo);
                }
                else
                {
                    // Inicio de sesión fallido
                    ViewData["mensaje"] = "Nombre de usuario o contraseña incorrectos." + resultado.ToString();
                    return View(modelo);
                }
            }
            catch (Exception ex)
            {
                ViewData["error"] = "Error: " + ex.Message;
                return View(modelo);
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string sEmail = model.Email;
                var normalizedEmail = userManager.NormalizeEmail(sEmail);
                //var user = await userManager.FindByEmailAsync(normalizedEmail);
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == sEmail);
                if (user == null || !await userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View(model);
                }

                // Generar el token de restablecimiento de contraseña
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Construir la URL de restablecimiento de contraseña
                var callbackUrl = Url.Action("ResetPassword", "Usuario", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

                // Enviar el correo electrónico con el enlace de restablecimiento de contraseña
                await emailService.EnviarEmail(model.Email, "Restablecer contraseña",
                    $"Por favor, restablece tu contraseña haciendo clic <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a>.");

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("Código de restablecimiento de contraseña debe ser suministrado.");
            }

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // No revelar si el usuario no existe
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                // Decodificar y usar el token de restablecimiento de contraseña
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
                var result = await userManager.ResetPasswordAsync(user, code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager
                .ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null,
            string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");

            var mensaje = "";

            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                mensaje = "Error cargando la data de login externo";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            // Ya la cuenta existe
            if (resultadoLoginExterno.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }

            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var usuario = new Usuario { Email = email, UserName = email };

            var resultadoCrearUsuario = await userManager.CreateAsync(usuario);

            if (!resultadoCrearUsuario.Succeeded)
            {
                mensaje = resultadoCrearUsuario.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);

            if (resultadoAgregarLogin.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);
                return LocalRedirect(urlRetorno);
            }

            mensaje = "Ha ocurrido un error agregando el login";
            return RedirectToAction("login", routeValues: new { mensaje });
        }

        [HttpGet]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await context.Users.Select(u => new Usuario
            {
                Email = u.Email
            }).ToListAsync();

            var modelo = new UsuarioListado();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;
            return View(modelo);

        }


        [HttpGet]
        public string Photo()
        {
            Usuario user = userManager.GetUserAsync(User).Result; // Espera a que la tarea se complete
            if (user != null && user.AvatarPath != null)
            {
                return user.AvatarPath.ToString();
            }
            else
            {
                return "/images/Employee.png";
            }
        }

        //public string result que devuelve el Rol del usuario actual.
        [HttpGet]
        public string GetRole()
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var roles = userManager.GetRolesAsync(user).Result;
                if (roles.Count > 0)
                {
                    return roles[0];
                }
            }
            return "";
        }


        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (user.AvatarName == null)
            {
                user.AvatarPath = Url.Content("~/images/Employee.png");
            }
            var model = new PersonalDataViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarName = user.AvatarName,
                AvatarPath = user.AvatarPath
            };
            return View(model);
        }
        public IActionResult ObtenerMenuDinamico()
        {
            var gen = new cGeneralFun();
            var menu = gen.ObtenerMenu("Perfil"); // Obtener el menú para el perfil
            return PartialView("_MenuDinamico", menu); // Retornar el partial con la lista de menús
        }
        [HttpGet]
        public async Task<IActionResult> Email(string mensaje = null)
        {

            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EmailViewModel
            {
                Email = user.Email,
                IsEmailConfirmed = user.EmailConfirmed,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Email(EmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Email", model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Cambiar UserName y NormalizedUserName por el correo electrónico
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email.ToUpperInvariant();

            user.Email = model.Email;
            user.EmailConfirmed = false;
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // URL de confirmación personalizada
                var callbackUrl = Url.Action("ConfirmarEmail", "Usuario", new { userId = user.Id, code }, protocol: Request.Scheme);


                // Aquí puedes enviar el correo electrónico con el enlace de confirmación
                // Usa la URL 'callbackUrl' para construir el enlace en el correo electrónico
                await emailService.EnviarEmail(model.Email, "Confirmación de correo electrónico", $"Por favor, confirma tu correo electrónico haciendo clic <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a>.");

                ViewData["mensaje"] = "Se ha enviado un correo electrónico de confirmación. Por favor, verifica tu nuevo correo electrónico.";
                return View("Email", model);
            }

            foreach (var error in result.Errors)
            {
                ViewData["Error"] = "Ocurrió un error al actualizar la información";
                //ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Email", model);
        }


        [HttpGet]
        public IActionResult ChangePassword(string mensaje = null)
        {

            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                ViewData["mensaje"] = "Contraseña actualizada con éxito.";
                return View(model);
            }

            foreach (var error in changePasswordResult.Errors)
            {
                ViewData["Error"] = "Ocurrió un error al actualizar la información";
                //ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalData(PersonalDataViewModel model, IFormFile avatar)
        {
            if (!ModelState.IsValid)
            {
                return View("PersonalData", model);
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            string previous_path = "";

            if (user.AvatarName != null)
            {
                previous_path = System.IO.Path.Combine(Environment.WebRootPath, "images", user.AvatarName);
            }
            if (avatar != null && avatar.Length > 0)
            {
                // Genera un nombre único para el archivo de imagen
                var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(avatar.FileName);
                // Obtiene la ruta del directorio wwwroot/images
                var imagePath = System.IO.Path.Combine(Environment.WebRootPath, "images", fileName);
                // Copia el contenido del archivo a la ubicación en el servidor
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }
                user.AvatarName = fileName;
                user.AvatarPath = "/images/" + fileName;
                //elimina la imagen anterior
                if (!string.IsNullOrEmpty(previous_path))
                {
                    if (System.IO.File.Exists(previous_path))
                    {
                        System.IO.File.Delete(previous_path);
                    }
                }
                model.AvatarPath = user.AvatarPath;
                model.AvatarName = user.AvatarName;
            }
            else
            {
                user.AvatarName = model.AvatarName;
                user.AvatarPath = model.AvatarPath;
            }
            if (model.AvatarPath == null)
            {
                model.AvatarPath = "/Images/Employee.png";
            }



            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                ViewData["mensaje"] = "Datos personales actualizados con éxito.";
                return View("PersonalData", model);
            }

            foreach (var error in result.Errors)
            {
                ViewData["Error"] = "Ocurrió un error al actualizar la información";
                //ModelState.AddModelError(string.Empty, error.Description);

            }

            return View("PersonalData", model);
        }

    }
}
