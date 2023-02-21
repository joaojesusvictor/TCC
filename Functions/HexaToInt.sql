drop function  HexaToInt
go

CREATE FUNCTION HexaToInt(@Entrada varchar(30))
RETURNS int
AS
BEGIN
	Declare @Saida Int
	SELECT @saida= CAST( CONVERT(VARBINARY,'0x'+RIGHT('00000000'+REPLACE(@Entrada,'x',''),8),1) AS INT)
	return @saida
END

--select dbo.HexToInt('FD')