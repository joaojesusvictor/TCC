/** Sobre o CREATE PROCEDURE 'Clien_Clientes_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Clien_Clientes_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Clien_Clientes_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Clien_Clientes_TRAN
END
GO

CREATE PROCEDURE dbo.Clien_Clientes_TRAN
	@Modo						integer					,
	@CodigoCliente				int				=	NULL,
	@NomeCliente				varchar(100)	=	NULL,
	@Cep						varchar(10)		=	NULL,
	@Endereco					varchar(200)	=	NULL,
	@Numero						int				=	NULL,
	@Complemento				varchar(100)	=	NULL,
	@Bairro						varchar(100)	=	NULL,
	@Cidade						varchar(100)	=	NULL,
	@Uf							varchar(2)		=	NULL,
	@Pais						varchar(50)		=	NULL,
	@DataNascimento				datetime		=	NULL,
	@Cpf						varchar(20)		=	NULL,
	@Sexo						varchar(1)		=	NULL,
	@Email						varchar(100)	=	NULL,
	@Telefone1					varchar(20)		=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Clien_Clientes_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Cliente
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
 
 select * from Funcionario
 
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
select @NomeTabela = 'Tabela de Cliente.'
    
IF @Modo = 1 --Inclusao
begin
	if not exists(select * from Cliente where Cpf = @Cpf and Ativo = 1)
		begin
			if exists(select * from Cliente where Cpf = @Cpf and Ativo = 0)
				begin
					Update	Cliente
					set		NomeCliente = @NomeCliente,
							Cep = @Cep,
							Endereco = @Endereco,
							Numero = @Numero,
							Complemento = @Complemento,
							Bairro = @Bairro,
							Cidade = @Cidade,
							Uf = @Uf,
							Pais = @Pais,
							DataNascimento = @DataNascimento,
							Cpf = @Cpf,
							Sexo = @Sexo,
							Email = @Email,
							Telefone1 = @Telefone1,
							Ativo = 1,
							DataUltimaAlteracao = GETDATE(),
							UsuarioUltimaAlteracao = @NomeUsuarioTRAN
					where	Cpf = @Cpf

					select 1
				end
			else
				begin
					insert Cliente (
									DataCadastro, NomeCliente, Cep, Endereco, Numero, Complemento, Bairro, Cidade, Uf, Pais,
									DataNascimento, Cpf, Sexo, Email, Telefone1, Ativo, DataInclusao, UsuarioIncluiu )
									Output inserted.CodigoCliente
		
					Values		(	
									GETDATE(), @NomeCliente, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais,
									@DataNascimento, @Cpf, @Sexo, @Email, @Telefone1, 1, GETDATE(), @NomeUsuarioTRAN )
				end
		end
	else
		begin
			select 0
		end	
end

else IF @Modo = 2 -- Alteracao
begin
	if not exists(select * from Cliente where CodigoCliente <> @CodigoCliente and Cpf = @Cpf and Ativo = 1)
		begin
			Update	Cliente
			set		NomeCliente = @NomeCliente,
					Cep = @Cep,
					Endereco = @Endereco,
					Numero = @Numero,
					Complemento = @Complemento,
					Bairro = @Bairro,
					Cidade = @Cidade,
					Uf = @Uf,
					Pais = @Pais,
					DataNascimento = @DataNascimento,
					Cpf = @Cpf,
					Sexo = @Sexo,
					Email = @Email,
					Telefone1 = @Telefone1,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoCliente = @CodigoCliente

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
		Update	Cliente 
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoCliente = @CodigoCliente

	    select @errorreturned = @@error     
        IF @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na delecao ' + @NomeTabela     
           return -100        
        end
End

ELSE IF @Modo = 4 -- Consulta Unico Cliente
Begin
		Select	*
		From	Cliente
		Where	CodigoCliente = @CodigoCliente
		and		Ativo = 1

End

ELSE IF @Modo = 5 -- Consulta Varios Clientes
Begin
		Select	*
		From	Cliente
		Where	Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Clien_Clientes_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Clien_Clientes_TRAN'
END
GO