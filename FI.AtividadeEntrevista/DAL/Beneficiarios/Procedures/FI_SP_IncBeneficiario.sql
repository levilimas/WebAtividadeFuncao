IF OBJECT_ID('dbo.FI_SP_IncBeneficiario', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_IncBeneficiario;
GO

CREATE PROC dbo.FI_SP_IncBeneficiario
    @Nome      NVARCHAR(100),
    @IdCliente BIGINT,
    @CPF       VARCHAR(11)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.BENEFICIARIOS (Nome, IdCliente, CPF)
    VALUES (@Nome, @IdCliente, @CPF);

    SELECT SCOPE_IDENTITY();
END;
GO