using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    public class FuncionariosController : Controller
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

        public async Task<IActionResult> New(string msg = "")
        {
            var viewModel = new FuncionariosViewModel()
            {
                MsgErro = msg
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(FuncionariosViewModel model)
        {
            string msgErro = "";

            if (!CpfValido(model.Cpf))
            {
                msgErro = "Este CPF não é válido!";

                return RedirectToAction(nameof(New), new { msg = msgErro });
            }

            await _funcionariosRepository.CreateAsync(model);

            //AlertSuccess("");

            //AddNotification("");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, string msg = "")
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
                Cargo = funcionario.Cargo,
                MsgErro = msg
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(FuncionariosViewModel model)
        {
            string msgErro = "";
            
            if (!CpfValido(model.Cpf))
            {
                msgErro = "Este CPF não é válido!";

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFuncionario, msg = msgErro });
            }

            await _funcionariosRepository.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> Delete(int id)
        {
            await _funcionariosRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public static bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}