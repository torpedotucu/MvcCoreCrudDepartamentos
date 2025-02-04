using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDepartamentos.Models;
using MvcCoreCrudDepartamentos.Repositories;

namespace MvcCoreCrudDepartamentos.Controllers
{
    public class DepartamentosController : Controller
    {

        RepositoryDepartamentos repo;

        public DepartamentosController()
        {
            this.repo=new RepositoryDepartamentos();
        }

        public async Task<IActionResult> Index()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string localidad)
        {
            await this.repo.InsertDepartament(nombre, localidad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Departamento dept = await this.repo.FindDepartamento(id);
            return View(dept);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(Departamento departamento)
        {
            await this.repo.UpdateDepartamentosAsync(departamento.idDepartamento, departamento.Nombre, departamento.Localidad);
            return RedirectToAction("Index");
        }
    }
}
