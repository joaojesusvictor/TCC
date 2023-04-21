/** Sobre o CREATE PROCEDURE 'Func_Funcionarios_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Funcionarios_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Func_Funcionarios_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Func_Funcionarios_TRAN
END
GO

CREATE PROCEDURE dbo.Func_Funcionarios_TRAN
	@Modo						integer					,
	@CodigoFuncionario			int				=	NULL,
	@NomeFuncionario			varchar(100)	=	NULL,
	@DataContratacao			datetime		=	NULL,
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
	@Telefone1					varchar(20)		=	NULL,
	@Cargo						varchar(100)	=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         Func_Funcionarios_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Funcionario
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
select @NomeTabela = 'Tabela de Funcionario.'
    
IF @Modo = 1 --Inclusao
begin
	if not exists(select * from Funcionario where Cpf = @Cpf and Ativo = 1)
		begin
			if exists(select * from Funcionario where Cpf = @Cpf and Ativo = 0)
				begin
					Update	Funcionario
					set		NomeFuncionario = @NomeFuncionario,
							DataContratacao = @DataContratacao,
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
							Telefone1 = @Telefone1,
							Cargo = @Cargo,
							Ativo = 1,
							DataUltimaAlteracao = GETDATE(),
							UsuarioUltimaAlteracao = @NomeUsuarioTRAN
					where	Cpf = @Cpf

					select 1
				end
			else
				begin
					insert Funcionario (
									DataContratacao, NomeFuncionario, Cep, Endereco, Numero, Complemento, Bairro, Cidade, Uf, Pais,
									DataNascimento, Cpf, Sexo, Telefone1, Cargo, Ativo, DataInclusao, UsuarioIncluiu )
									Output inserted.CodigoFuncionario
		
					Values		(	
									@DataContratacao, @NomeFuncionario, @Cep, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @Uf, @Pais,
									@DataNascimento, @Cpf, @Sexo, @Telefone1, @Cargo, 1, GETDATE(), @NomeUsuarioTRAN )
				end
		end
	else
		begin
			select 0
		end	
end

else IF @Modo = 2 -- Alteracao
begin
	if not exists(select * from Funcionario where CodigoFuncionario <> @CodigoFuncionario and Cpf = @Cpf and Ativo = 1)
		begin
			Update	Funcionario
			set		NomeFuncionario = @NomeFuncionario,
					DataContratacao = @DataContratacao,
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
					Telefone1 = @Telefone1,
					Cargo = @Cargo,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoFuncionario = @CodigoFuncionario

			update	Usuario
			set		NomeUsuario = @NomeFuncionario
			where	CodigoFuncionario = @CodigoFuncionario

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
		update	Usuario
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoFuncionario = @CodigoFuncionario

		Update	Funcionario 
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoFuncionario = @CodigoFuncionario

	    select @errorreturned = @@error     
        IF @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na delecao ' + @NomeTabela     
           return -100        
        end
End

ELSE IF @Modo = 4 -- Consulta Unico Funcionario
Begin
		Select	*
		From	Funcionario
		Where	CodigoFuncionario = @CodigoFuncionario
		and		Ativo = 1
End

ELSE IF @Modo = 5 -- Consulta Varios Funcionarios
Begin
		Select	*
		From	Funcionario F left join
				Usuario U on f.CodigoFuncionario = u.CodigoFuncionario
		Where	F.Ativo = 1
End

ELSE IF @Modo = 6 -- Combo Funcionarios
Begin
	Select	CodigoFuncionario, NomeFuncionario
	From	Funcionario
	Where	Ativo = 1
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Funcionarios_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Func_Funcionarios_TRAN'
END
GO