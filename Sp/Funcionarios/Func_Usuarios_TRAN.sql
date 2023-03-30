/** Sobre o CREATE PROCEDURE 'Func_Usuarios_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.Func_Usuarios_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.Func_Usuarios_TRAN
END
GO

CREATE PROCEDURE dbo.Func_Usuarios_TRAN
	@Modo			integer					,
	@Login			varchar(100)	= NULL	,
	@Senha			varchar(1000)	= NULL	,
	@UsuarioTran	int				= NULL
AS
 
/************************
 ** Nome:         Func_Usuarios_TRAN 4
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

If @Modo = 4 -- Get Dados Usuario
Begin
	select	*, dbo.EncriptDecript(Senha, 'D') SenhaDecript
	from	Usuario
	where	LoginUsuario = @Login
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.Func_Usuarios_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.Func_Usuarios_TRAN'
END
GO