using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    public class FuncionariosController : BaseController
    {
        private readonly ILogger<FuncionariosController> _logger;
        private FuncionariosRepository _funcionariosRepository;

        public FuncionariosController(ILogger<FuncionariosController> logger, FuncionariosRepository funcionariosRepository)
        {
            _logger = logger;
            _funcionariosRepository = funcionariosRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<FuncionariosRepository.Funcionario> funcionarios = await _funcionariosRepository.GetFuncionariosAsync();

            var viewModel = new FuncionariosViewModel()
            {
                UsuarioAdm = true
            };

            foreach (var f in funcionarios)
            {
                viewModel.Funcionarios.Add(new FuncionarioViewModel()
                {
                    CodigoFuncionario = f.CodigoFuncionario,
                    NomeFuncionario = f.NomeFuncionario,
                    Email = f.Email,
                    Cpf = f.Cpf,
                    Telefone1 = f.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var viewModel = new FuncionariosViewModel(){};

            return View(viewModel);
        }

        public async Task<IActionResult> Create(FuncionariosViewModel model)
        {
            if (!CpfValido(model.Cpf))
            {
                AddNotification("Este CPF não é válido!");

                return RedirectToAction(nameof(New));
            }

            await _funcionariosRepository.CreateAsync(model);

            AlertSuccess("Funcionário incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            FuncionariosRepository.Funcionario funcionario = await _funcionariosRepository.GetFuncionarioAsync(id);

            var viewModel = new FuncionariosViewModel()
            {
                ModoEdit = true,
                CodigoFuncionario = id,
                DataInclusao = funcionario.DataInclusao,
                DataUltimaAlteracao = funcionario.DataUltimaAlteracao,
                UsuarioIncluiu = funcionario.UsuarioIncluiu,
                UsuarioUltimaAlteracao = funcionario.UsuarioUltimaAlteracao,
                NomeFuncionario = funcionario.NomeFuncionario,
                DataNascimento = funcionario.DataNascimento,
                Telefone1= funcionario.Telefone1,
                Cpf = funcionario.Cpf,
                Cep = funcionario.Cep,
                Endereco = funcionario.Endereco,
                Numero = funcionario.Numero,
                Complemento = funcionario.Complemento,
                Bairro = funcionario.Bairro,
                Cidade = funcionario.Cidade,
                Uf = funcionario.Uf,
                Pais = funcionario.Pais,
                Sexo = funcionario.Sexo,
                Cargo = funcionario.Cargo
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(FuncionariosViewModel model)
        {            
            if (!CpfValido(model.Cpf))
            {
                AddNotification("Este CPF não é válido!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFuncionario });
            }

            await _funcionariosRepository.UpdateAsync(model);

            AlertSuccess("Funcionário alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> Delete(int id)
        {
            await _funcionariosRepository.DeleteAsync(id);

            AlertSuccess("Funcionário excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}