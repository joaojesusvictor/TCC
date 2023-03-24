using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class ClientesController : BaseController
    {
        private ClientesRepository _clientesRepository;
        private readonly ISessao _sessao;

        public ClientesController(ClientesRepository clientesRepository, ISessao sessao)
        {
            _clientesRepository = clientesRepository;
            _sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ClientesRepository.Cliente> clientes = await _clientesRepository.GetClientesAsync();
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ClientesViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var f in clientes)
            {
                viewModel.Clientes.Add(new ClienteViewModel()
                {
                    CodigoCliente = f.CodigoCliente,
                    NomeCliente = f.NomeCliente,
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

            var viewModel = new ClientesViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ClientesViewModel model)
        {
            if (!CpfValido(model.Cpf))
            {
                MostraMsgErro("Este CPF não é válido!");

                return RedirectToAction(nameof(New));
            }

            if (!EmailValido(model.Email, true))
            {
                MostraMsgErro("O E-mail precisa ser válido");

                return RedirectToAction(nameof(New));
            }

            int gravado = await _clientesRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe um Cliente com este CPF.");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Cliente incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ClientesRepository.Cliente cliente = await _clientesRepository.GetClienteAsync(id);
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ClientesViewModel()
            {
                ModoEdit = true,
                CodigoCliente = id,
                DataInclusao = cliente.DataInclusao,
                DataUltimaAlteracao = cliente.DataUltimaAlteracao,
                UsuarioIncluiu = cliente.UsuarioIncluiu,
                UsuarioUltimaAlteracao = cliente.UsuarioUltimaAlteracao,
                NomeCliente = cliente.NomeCliente,
                DataNascimento = cliente.DataNascimento,
                Email = cliente.Email,
                Telefone1= cliente.Telefone1,
                Cpf = cliente.Cpf,
                Cep = cliente.Cep,
                Endereco = cliente.Endereco,
                Numero = cliente.Numero,
                Complemento = cliente.Complemento,
                Bairro = cliente.Bairro,
                Cidade = cliente.Cidade,
                Uf = cliente.Uf,
                Pais = cliente.Pais,
                Sexo = cliente.Sexo,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(ClientesViewModel model)
        {            
            if (!CpfValido(model.Cpf))
            {
                MostraMsgErro("Este CPF não é válido!");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
            }

            if (!EmailValido(model.Email, true))
            {
                MostraMsgErro("O E-mail precisa ser válido");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
            }

            int gravado = await _clientesRepository.UpdateAsync(model);

            if(gravado == 0)
            {
                MostraMsgErro("Já existe um Cliente com este CPF.");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
            }

            MostraMsgSucesso("Cliente alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _clientesRepository.DeleteAsync(id, codigoUsuario);

            MostraMsgSucesso("Cliente excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}