using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class FuncionariosController : BaseController
    {
        private FuncionariosRepository _funcionariosRepository;
        private readonly ISessao _sessao;

        public FuncionariosController(FuncionariosRepository funcionariosRepository, ISessao sessao)
        {
            _funcionariosRepository = funcionariosRepository;
            _sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<FuncionariosRepository.Funcionario> funcionarios = await _funcionariosRepository.GetFuncionariosAsync();
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new FuncionariosViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
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
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new FuncionariosViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(FuncionariosViewModel model)
        {
            if (!CpfValido(model.Cpf))
            {
                MostraMsgErro("Este CPF não é válido!");

                return RedirectToAction(nameof(New));
            }

            int gravado = await _funcionariosRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe um Funcionário com este CPF.");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Funcionário incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            FuncionariosRepository.Funcionario funcionario = await _funcionariosRepository.GetFuncionarioAsync(id);
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

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
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(FuncionariosViewModel model)
        {            
            if (!CpfValido(model.Cpf))
            {
                MostraMsgErro("Este CPF não é válido!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFuncionario });
            }

            int gravado = await _funcionariosRepository.UpdateAsync(model);

            if(gravado == 0)
            {
                MostraMsgErro("Já existe um Funcionário com este CPF.");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFuncionario });
            }

            MostraMsgSucesso("Funcionário alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _funcionariosRepository.DeleteAsync(id, codigoUsuario);

            MostraMsgSucesso("Funcionário excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}