/** Sobre o CREATE PROCEDURE 'Func_Usuarios_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Func_Usuarios_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Func_Usuarios_TRAN
END
GO

CREATE PROCEDURE dbo.Func_Usuarios_TRAN
	@Modo						integer					,
	@Login						varchar(100)	= NULL	,
	@Senha						varchar(1000)	= NULL	,
	@UsuarioTran				int				= NULL	,
	@CodigoUsuario				int				= NULL	,
	@NomeUsuario				varchar(100)	= NULL	,
	@Email						varchar(100)	= NULL	,
	@UsuarioAdm					bit				= NULL	,
	@CodigoFuncionario			int				= NULL	,
	@NovaSenha					varchar(1000)	= NULL
AS
 
/************************
 ** Nome:         Func_Usuarios_TRAN 6
 ** Autor:        Joao
 ** Descrição:    Transaciona a Tabela de Usuario
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
 
 select * from Usuario
 
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
select @NomeTabela = 'Tabela de Usuario.'

IF @Modo = 1 --Inclusao
Begin
	if not exists(select * from Usuario where LoginUsuario = @Login and Ativo = 1)
		begin
			if exists(select * from Usuario where LoginUsuario = @Login and Ativo = 0)
				begin
					Update	Usuario
					set		NomeUsuario = (select NomeFuncionario from Funcionario where CodigoFuncionario = @CodigoFuncionario),
							LoginUsuario = @Login,
							Senha = @Senha,
							Email = @Email,
							UsuarioAdm = @UsuarioAdm,
							Ativo = 1,
							DataUltimaAlteracao = GETDATE(),
							UsuarioUltimaAlteracao = @NomeUsuarioTRAN
					where	LoginUsuario = @Login

					select 1
				end
			else
				begin
					insert Usuario(	NomeUsuario, LoginUsuario, Senha, Email, UsuarioAdm, Ativo, DataInclusao, UsuarioIncluiu, CodigoFuncionario )
									Output inserted.CodigoUsuario
		
					Values		(	(select NomeFuncionario from Funcionario where CodigoFuncionario = @CodigoFuncionario),
									@Login, @Senha, @Email, @UsuarioAdm, 1, GETDATE(), @NomeUsuarioTRAN, @CodigoFuncionario )
				end
		end
	else
		begin
			select 0
		end	
End

ELSE IF @Modo = 2 --Alteracao
Begin
	if not exists(select * from Usuario where CodigoUsuario <> @CodigoUsuario and LoginUsuario = @Login and Ativo = 1)
		begin
			Update	Usuario
			set		LoginUsuario = @Login,
					Email = @Email,
					UsuarioAdm = @UsuarioAdm,
					DataUltimaAlteracao = GETDATE(),
					UsuarioUltimaAlteracao = @NomeUsuarioTRAN
			where	CodigoUsuario = @CodigoUsuario

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
End

ELSE IF @Modo = 3 -- Exclusao
begin
		Update	Usuario 
		set		Ativo = 0,
				DataUltimaAlteracao = GETDATE(),
				UsuarioUltimaAlteracao = @NomeUsuarioTRAN
		where	CodigoUsuario = @CodigoUsuario

	    select @errorreturned = @@error     
        IF @errorreturned <> 0
        begin
           select 'Ocorreu uma falha na delecao ' + @NomeTabela     
           return -100        
        end
End

ELSE If @Modo = 4 -- Get Dados Usuario Logado
Begin
	select	*
	from	Usuario
	where	LoginUsuario = @Login
End

ELSE IF @Modo = 5 -- Consulta Unico Usuario
Begin
		Select	*
		From	Usuario
		Where	CodigoUsuario = @CodigoUsuario
		and		Ativo = 1
End

ELSE IF @Modo = 6 -- Consulta Varios Usuarios
Begin
		Select	*
		From	Usuario
		Where	Ativo = 1
End

ELSE IF @Modo = 7 -- Consulta Usuario Para Redefinir Senha
Begin
	Select	*
	From	Usuario
	Where	LoginUsuario = @Login
	and		Email = @Email
End

ELSE IF @Modo = 8 -- Atualiza Senha Usuario
Begin
	Update	Usuario
	Set		Senha = @Senha
	Where	CodigoUsuario = @CodigoUsuario
End

ELSE IF @Modo = 9 -- Redefine Senha Usuario
Begin
	if exists(select * from Usuario where CodigoUsuario = @CodigoUsuario and Senha = @Senha)
		Begin
			Update	Usuario
			Set		Senha = @NovaSenha
			Where	CodigoUsuario = @CodigoUsuario

			select 1
		End
	else
		Begin
			select 0
		End
End

ELSE IF @Modo = 10 -- Valida se Funcionario já tem Login
Begin
	if exists(select * from Usuario where CodigoFuncionario = @CodigoFuncionario and Ativo = 1)
		begin
			select 1
		end
	else
		begin
			select 0
		end
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Func_Usuarios_TRAN'
END
GO