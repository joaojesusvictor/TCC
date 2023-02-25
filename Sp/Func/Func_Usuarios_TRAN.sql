/** Sobre o CREATE PROCEDURE 'Func_Usuarios_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a �ltima vers�o de dbo.Func_Usuarios_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Func_Usuarios_TRAN
END
GO

CREATE PROCEDURE dbo.Func_Usuarios_TRAN
	@Modo			integer					,
	@Login			varchar(100)	= NULL	,
	@Senha			varchar(1000)	= NULL
AS
 
/************************
 ** Nome:         Func_Usuarios_TRAN 4
 ** Autor:        Joao
 ** Descri��o:    Transaciona a Tabela de Usuario
 **
 ** Valor Retorno: C�digo     Condi��o
 **                ------     ---------
 **
 * **************
 **           Hist�rico Manuten��o
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
		
-- Vari�vel para obter o nome do usu�rio que vai transacionar a tabela		
Declare	@NomeUsuarioTRAN			Varchar(30)	,
		@TextoLog					VarChar(100),
		@ProcedureName				VarChar(80)	,
		@DataLog					datetime
		
--if	ISNULL(@UsuarioTran,'') <> '' 
--begin
--	select	@NomeUsuarioTRAN		=	NomeFuncionario 
--	from	Funcionario	
--	where	CodigoFuncionario		=	@UsuarioTran
--end

-- Constante para exibi��o de mensagens
select @NomeTabela = 'Tabela de Usuario.'

IF @Modo = 4 -- Valida Login
Begin
	if exists(select * from Usuario where LoginUsuario = @Login and Senha = dbo.EncriptDecript(@Senha, 'E'))
		Begin
			select 1
		End
	else
		Begin
			select 0
		End
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Func_Usuarios_TRAN'
END
GO