﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema.Contexts;
using sistema.Models;

namespace sistema.Controllers
{
    public class ProfessorController : Controller
    {

        private readonly SistemaContext _context;

        public ProfessorController(SistemaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            int? professorId = HttpContext.Session.GetInt32("ProfessorId");

            if (professorId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Professor professor = _context.Professors.Find(professorId)!;

            var turmas = _context.Turmas.Where(x => x.ProfessorId == professorId).ToList();

            ViewBag.NomeProfessor = professor.Nome;

            return View(turmas);
        }

        public IActionResult CadastrarTurma(string nomeTurma)
        {
            int? professorId = HttpContext.Session.GetInt32("ProfessorId");

            Turma turma = new()
            {
                Nome = nomeTurma,
                ProfessorId = professorId
            };

            _context.Turmas.Add(turma);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AbrirModal(int turmaId)
        {
            ViewBag.TurmaId = turmaId;

            return View();
        }

        [HttpPost]
        public IActionResult ExcluirTurma(int turmaId)
        {

            Turma turma = _context.Turmas.Include(x => x.Atividades).FirstOrDefault(x => x.TurmaId == turmaId)!;

            if (turma == null)
            {
                TempData["Mensagem"] = "Turma não encontrada!";

                return RedirectToAction("Index");
            }

            if (turma.Atividades.Count > 0)
            {
                TempData["Mensagem"] = "Você não pode excluir uma turma que possui atividades!";

                return RedirectToAction("Index");
            }
            _context.Turmas.Remove(turma);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
