﻿using Microsoft.AspNetCore.Mvc;
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
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New));
            }

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
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoFornecedor });
            }

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

        public string Validar(FornecedoresViewModel model)
        {
            string msg = "";

            if (string.IsNullOrEmpty(model.RazaoSocial))
                msg = "A Razão Social é necessária! ";

            if (string.IsNullOrEmpty(model.NomeFantasia))
                msg += "O Nome Fantasia é necessário! ";

            if (string.IsNullOrEmpty(model.Telefone1))
                msg += "O Telefone é necessário! ";

            if (string.IsNullOrEmpty(model.Documento))
                msg += "O Documento é necessário! ";

            if (string.IsNullOrEmpty(model.Cep))
                msg += "O Cep é necessário! ";

            if (string.IsNullOrEmpty(model.Endereco))
                msg += "O Endereço é necessário! ";

            if (string.IsNullOrEmpty(model.Bairro))
                msg += "O Bairro é necessário! ";

            if (string.IsNullOrEmpty(model.Cidade))
                msg += "A Cidade é necessária! ";

            if (string.IsNullOrEmpty(model.Uf))
                msg += "O Estado é necessário! ";

            if (string.IsNullOrEmpty(model.Pais))
                msg += "O País é necessário!";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            FornecedoresRepository.Fornecedor fornecedor = await _fornecedoresRepository.GetFornecedorAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = fornecedor.NomeFantasia,
                Mensagem1 = $"Deseja realmente excluir o Fornecedor \"{fornecedor.NomeFantasia}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            await _fornecedoresRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"O Fornecedor \"{model.NomeEntidade}\" foi excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }
    }
}