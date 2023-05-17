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
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

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
                    Documento = f.Documento,
                    Telefone1 = f.Telefone1
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new ClientesViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(ClientesViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New));
            }

            string doc = DeixaSoNumeros(model.Documento);

            if (doc.Length == 11)
            {
                if (!CpfValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(New));
                }
            }
            else
            {
                if (!CnpjValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(New));
                }
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
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

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
                Documento = cliente.Documento,
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
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
            }

            string doc = DeixaSoNumeros(model.Documento);

            if (doc.Length == 11)
            {
                if (!CpfValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
                }
            }
            else
            {
                if (!CnpjValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(Edit), new { id = model.CodigoCliente });
                }
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

        public string Validar(ClientesViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.NomeCliente))
                msg = "O Nome Cliente é necessário! ";

            if (string.IsNullOrEmpty(model.Telefone1))
                msg += "O Telefone é necessário! ";

            if (string.IsNullOrEmpty(model.Documento))
                msg += "O Número Documento é necessário! ";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            ClientesRepository.Cliente cliente = await _clientesRepository.GetClienteAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = cliente.NomeCliente,
                Mensagem1 = $"Deseja realmente excluir o Cliente \"{cliente.NomeCliente}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            bool excluido = await _clientesRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            if (!excluido)
            {
                MostraMsgErro($"O Cliente \"{model.NomeEntidade}\" não pode ser excluído enquanto tiver OS Aberta/Em Execução para ele!");

                return RedirectToAction(nameof(Index));
            }

            MostraMsgSucesso($"O Cliente \"{model.NomeEntidade}\" foi excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }
    }
}