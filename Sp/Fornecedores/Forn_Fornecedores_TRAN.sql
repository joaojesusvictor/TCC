/** Sobre o CREATE PROCEDURE 'Forn_Fornecedores_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Forn_Fornecedores_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Forn_Fornecedores_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Forn_Fornecedores_TRAN
END
GO

CREATE PROCEDURE dbo.Forn_Fornecedores_TRAN
	@Modo						integer					,
	@CodigoFornecedor			int				=	NULL,
	@RazaoSocial				varchar(100)	=	NULL,
	@NomeFantasia				varchar(100)	=	NULL,
	@Documento					varchar(20)		=	NULL,
	@Cep						varchar(10)		=	NULL,
	@Endereco					varchar(200)	=	NULL,
	@Numero						int				=	NULL,
	@Complemento				varchar(100)	=	NULL,
	@Bairro						varchar(100)	=	NULL,
	@Cidade						varchar(100)	=	NULL,
	@Uf							varchar(2)		=	NULL,
	@Pais						varchar(50)		=	NULL,
	@Telefone1					varchar(20)		=	NULL,
	@Email						varchar(100)	=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Forn_Fornecedores_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Fornecedor
 **
 ** Valor Retorno: Código     Condição
 **                ------     ---------
 **
 * **************
 **           Histórico Manutenção
 * **************
 **
 ** ---------- --------   	-----------------
 ** 
 
 select * from Fornecedor
 
 ** 
 ***********************/
set nocount on

declare @rowsaffected				integer,
		@errorreturned				integer,
		@NomeTabela					VarChar(80)
		
-- Variável para obter o nome do usuário que vai transacionar a tabela		
Declare	@NomeUsuarioTRAN			Varchar(30)	
		
if	ISNULL(@UsuarioTran,'') <> '' 
begin
	select	@NomeUsuarioTRAN	=	NomeUsuario 
	from	Usuario	
	where	CodigoUsuario		=	@UsuarioTran
end

-- Constante para exibição de mensagens
select @NomeTabela = 'Tabela de Fornecedor.'
    
IF @Modo = 1 --Inclusao
begin
	if not exists(select * from Fornecedor where Documento = @Documento and Ativo = 1)
		begin
			if exists(select * from Fornecedor where Documento = @Documento and Ativo = 0)
				begin
					Update	Fornecedor
					set		RazaoSocial = @RazaoSocial,
							NomeFantasia = @NomeFantasia,					
							Documento = @Documento,
							Cep = @Cep,
							Endereco = @Endereco,
							Numero = @Numero,
							Complemento = @Complemento,
							Bairro = @Bairro,
							Cidade = @Cidade,
							Uf = @Uf,
							Pais = @Pais,
							Telefone1 = @Telefone1,
							Email = @Email,
							Ativo = 1,
							DataUltimaAlteracao = GETDATE(),
							UsuarioUltimaAlteracao = @NomeUsuarioTRAN
					where	Documento = @Documento

					select 1
				end
			else
				begin
					insert Fornecedor (
									DataCadastro, RazaoSocial, NomeFantasia, Documento, Cep, Endereco, Numero, Complemento, Bairro, Cidade, Uf, Pais,
									Telefone1, Email, Ativo, DataInclusao, UsuarioIncluiu )
									Output inserted.CodigoFornecedor
		
					Values		(	
									GETDATE(), @RazaoSocial, @NomeFantasia, @Documento, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais,
									@Telefone1, @Email, 1, GETDATE(), @NomeUsuarioTRAN )
				end
		end
	else
		begin
			select 0
		end	
end

else IF @Modo = 2 -- Alteracao
begin
	if not exists(select * from Fornecedor where CodigoFornecedor <> @CodigoFornecedor and Documento = @Documento and Ativo = 1)
		begin
			Update	Fornecedor
			set		RazaoSocial = @RazaoSocial,
					NomeFantasia = @NomeFantasia,
					Documento = @Documento,
					Cep = @Cep,
					Endereco = @Endereco,
					Numero = @Numero,
					Complemento = @Complemento,
					Bairro = @Bairro,
					Cidade = @Cidade,
					Uf = @Uf,
					Pais = @Pais,
					Telefone1 = @Telefone1,
					Email = @Email,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoFornecedor = @CodigoFornecedor

			select 1
		end
	else
		begin
			select 0
		end

		select @rowsaffected = @@rowcount, @errorreturned = @@error     
        IF @rowsaffected = 0 OR @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na alteracao ' + @NomeTabela     
           return -100        
        end
end

ELSE IF @Modo = 3 -- Exclusao
begin
		Update	Fornecedor 
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoFornecedor = @CodigoFornecedor

	    select @errorreturned = @@error     
        IF @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na delecao ' + @NomeTabela     
           return -100        
        end
End

ELSE IF @Modo = 4 -- Consulta Unico Fornecedor
Begin
		Select	*
		From	Fornecedor
		Where	CodigoFornecedor = @CodigoFornecedor
		and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varios Fornecedores
Begin
		Select	*
		From	Fornecedor
		Where	Ativo = 1
End

ELSE IF @Modo = 6 -- Combo Fornecedores
Begin
	Select	CodigoFornecedor, NomeFantasia
	From	Fornecedor
	Where	Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Forn_Fornecedores_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Forn_Fornecedores_TRAN'
END
GO