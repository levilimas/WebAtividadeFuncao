IF OBJECT_ID('dbo.FI_SP_AltBenef', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_AltBenef;
GO

CREATE PROC dbo.FI_SP_AltBenef
    @ID   BIGINT,
    @Nome NVARCHAR(100),
    @CPF  VARCHAR(11)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.BENEFICIARIOS
    SET
        NOME = @Nome,
        CPF  = @CPF
    WHERE ID = @ID;
END;
GO