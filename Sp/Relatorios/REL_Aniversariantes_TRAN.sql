/** Sobre o CREATE PROCEDURE 'REL_Aniversariantes_TRAN' ***/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.REL_Aniversariantes_TRAN') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.REL_Aniversariantes_TRAN'
   PRINT ' '
   DROP PROCEDURE dbo.REL_Aniversariantes_TRAN
END
GO

CREATE PROCEDURE dbo.REL_Aniversariantes_TRAN
	@Modo						integer					,
	@Mes						int				=	NULL,
	@UsuarioTran				int				=	NULL
AS
 
/************************
 ** Nome:         REL_Aniversariantes_TRAN 5
 ** Autor:        Joao
 ** Descrição:    Gera relatorio de Aniversariantes para cada mes
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

IF @Modo = 4 -- Consulta Aniversariantes do Mes
Begin
	select * from
	(
		select	F.NomeFuncionario Nome,
				F.Telefone1 Telefone,
				ISNULL(U.Email, '') Email,
				CASE 
					WHEN 
						MONTH(GETDATE()) > MONTH(F.DataNascimento)
						OR
						(
						    MONTH(GETDATE()) = MONTH(F.DataNascimento) 
						    AND DAY(GETDATE()) >= DAY(F.DataNascimento) 
						)
					THEN DATEDIFF(YEAR, F.DataNascimento, GETDATE()) 
					ELSE DATEDIFF(YEAR, F.DataNascimento, GETDATE()) -1
				END Idade,
				'Funcionário' TipoPessoa,
				MONTH(F.DataNascimento) MesNascimento
		from	Funcionario F left join
				Usuario U on F.CodigoFuncionario = U.CodigoFuncionario
		where	F.Ativo = 1
		and		ISNULL(F.DataNascimento, '') <> ''

		union all

		select	NomeCliente Nome,
				Telefone1 Telefone,
				Email,
				CASE 
					WHEN 
						MONTH(GETDATE()) > MONTH(DataNascimento)
						OR
						(
						    MONTH(GETDATE()) = MONTH(DataNascimento) 
						    AND DAY(GETDATE()) >= DAY(DataNascimento) 
						)
					THEN DATEDIFF(YEAR, DataNascimento, GETDATE()) 
					ELSE DATEDIFF(YEAR, DataNascimento, GETDATE()) -1
				END Idade,
				'Cliente' TipoPessoa,
				MONTH(DataNascimento) MesNascimento
		from	Cliente
		where	Ativo = 1
		and		ISNULL(DataNascimento, '') <> ''
	) A
	where A.MesNascimento = @Mes
	order by TipoPessoa, Nome
End

ELSE IF @Modo = 5 -- Consulta Aniversariantes por Mes
Begin
	select COUNT(id) QtdAniversariantes, MesNascimento from
	(
		select	CodigoFuncionario Id, MONTH(DataNascimento) MesNascimento
		from	Funcionario
		where	Ativo = 1
		and		ISNULL(DataNascimento, '') <> ''

		union all

		select	CodigoCliente Id, MONTH(DataNascimento) MesNascimento
		from	Cliente
		where	Ativo = 1
		and		ISNULL(DataNascimento, '') <> ''
	) A
	group by MesNascimento
End
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.REL_Aniversariantes_TRAN') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.REL_Aniversariantes_TRAN'
END
GO