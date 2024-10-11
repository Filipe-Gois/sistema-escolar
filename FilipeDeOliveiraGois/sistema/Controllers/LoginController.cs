using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema.Contexts;
using sistema.Models;

namespace sistema.Controllers
{
    public class LoginController : Controller
    {
        private readonly SistemaContext _context;


        public LoginController(SistemaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logar(string email, string senha)
        {
            try
            {
                Professor professor = _context.Professors.FirstOrDefault(x => x.Email == email && x.Senha == senha)!;

                if (professor != null)
                {
                    HttpContext.Session.SetInt32("ProfessorId", professor.ProfessorId);
                    HttpContext.Session.SetString("Nome", professor.Nome!);

                    return RedirectToAction("Index", "Professor");
                }

                TempData["Mensagem"] = "Email ou senha incorretos!";

                //já retorna pra index do login
                return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
