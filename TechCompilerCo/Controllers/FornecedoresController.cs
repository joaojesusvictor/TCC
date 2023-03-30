using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [SomenteAdm]

    public class FornecedoresController : BaseController
    {
        private FornecedoresRepository _fornecedoresRepository;
        private readonly ISessao _sessao;

        public FornecedoresController(FornecedoresRepository fornecedoresRepository, ISessao sessao)
        {
            _fornecedoresRepository = fornecedoresRepository;
            _sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<FornecedoresRepository.Fornecedor> fornecedores = await _fornecedoresRepository.GetFornecedoresAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new FornecedoresViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var f in fornecedores)
            {
                viewModel.Fornecedores.Add(new FornecedorViewModel()
                {
                    CodigoFornecedor = f.CodigoFornecedor,
                    RazaoSocial = f.RazaoSocial,
                    NomeFantasia = f.NomeFantasia,
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

            var viewModel = new FornecedoresViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(FornecedoresViewModel model)
        {
            string doc = DeixaSoNumeros(model.Documento);

            if(doc.Length == 11)
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

            if (!EmailValido(model.Email))
            {
                MostraMsgErro("O Email precisa ser válido");

                return RedirectToAction(nameof(New));
            }

            int gravado = await _fornecedoresRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe um Fornecedor com este Documento.");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Fornecedor incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            FornecedoresRepository.Fornecedor fornecedor = await _fornecedoresRepository.GetFornecedorAsync(id);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new FornecedoresViewModel()
            {
                ModoEdit = true,
                CodigoFornecedor = id,
                DataInclusao = fornecedor.DataInclusao,
                DataUltimaAlteracao = fornecedor.DataUltimaAlteracao,
                UsuarioIncluiu = fornecedor.UsuarioIncluiu,
                UsuarioUltimaAlteracao = fornecedor.UsuarioUltimaAlteracao,
                RazaoSocial = fornecedor.RazaoSocial,
                NomeFantasia = fornecedor.NomeFantasia,
                Telefone1= fornecedor.Telefone1,
                Documento = fornecedor.Documento,
                Cep = fornecedor.Cep,
                Endereco = fornecedor.Endereco,
                Numero = fornecedor.Numero,
                Complemento = fornecedor.Complemento,
                Bairro = fornecedor.Bairro,
                Cidade = fornecedor.Cidade,
                Uf = fornecedor.Uf,
                Pais = fornecedor.Pais,
                Email = fornecedor.Email,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(FornecedoresViewModel model)
        {
            string doc = DeixaSoNumeros(model.Documento);

            if (doc.Length == 11)
            {
                if (!CpfValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(Edit), new { id = model.CodigoFornecedor });
                }
            }
            else
            {
                if (!CnpjValido(model.Documento))
                {
                    MostraMsgErro("Este Documento não é válido!");

                    return RedirectToAction(nameof(Edit), new { id = model.CodigoFornecedor });
                }
            }

            if (!EmailValido(model.Email))
            {
                MostraMsgErro("O Email precisa ser válido");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFornecedor });
            }

            int gravado = await _fornecedoresRepository.UpdateAsync(model);

            if(gravado == 0)
            {
                MostraMsgErro("Já existe um Fornecedor com este Documento.");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFornecedor });
            }

            MostraMsgSucesso("Fornecedor alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _fornecedoresRepository.DeleteAsync(id, codigoUsuario);

            MostraMsgSucesso("Fornecedor excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}