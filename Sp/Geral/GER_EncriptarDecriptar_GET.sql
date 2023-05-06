/***** Sobre o CREATE PROCEDURE 'GER_EncriptarDecriptar_GET' ******/
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.GER_EncriptarDecriptar_GET') and sysstat & 0xf = 4)
BEGIN
   PRINT 'Removida a última versão de dbo.GER_EncriptarDecriptar_GET'
   PRINT ' '
   DROP PROCEDURE dbo.GER_EncriptarDecriptar_GET
END
GO
 
CREATE PROCEDURE dbo.GER_EncriptarDecriptar_GET
	@Texto		VarChar(500),
	@Acao		VarChar(10)
AS
 
/********************************************************************
 ** Nome:         GER_EncriptarDecriptar_GET
 ** Autor:        Joao
 ** Descricao:    Encriptar/Decriptar strings
 ** *****************************************
 **           Historico Manutencao
 ** *****************************************
 ** Data:		Autor:		Descricao:
 ** ---------- --------   	-----------------
 ** 
 ** 
 *******************************************************************/

SET NOCOUNT ON

select dbo.EncriptDecript(@texto, @acao) as Resultado

GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.GER_EncriptarDecriptar_GET') and sysstat & 0xf = 4)
BEGIN
	PRINT 'Gerada a stored procedure dbo.GER_EncriptarDecriptar_GET'
END
GO
 
