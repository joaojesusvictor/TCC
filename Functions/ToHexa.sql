CREATE FUNCTION ToHexa(@value int)
RETURNS varchar(50)
AS
BEGIN
    DECLARE @seq char(16)
    DECLARE @result varchar(50)
    DECLARE @digit char(1)
    SET @seq = '0123456789ABCDEF'

    SET @result = SUBSTRING(@seq, (@value%16)+1, 1)

    WHILE @value > 0
    BEGIN
        SET @digit = SUBSTRING(@seq, ((@value/16)%16)+1, 1)

        SET @value = @value/16
        IF @value <> 0 SET @result = @digit + @result
    END 

    RETURN @result
END