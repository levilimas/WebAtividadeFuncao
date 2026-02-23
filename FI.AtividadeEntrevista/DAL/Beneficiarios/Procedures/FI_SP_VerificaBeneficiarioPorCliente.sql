IF OBJECT_ID('dbo.FI_SP_VerificaBeneficiarioPorCliente', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_VerificaBeneficiarioPorCliente;
GO

CREATE PROC dbo.FI_SP_VerificaBeneficiarioPorCliente
    @IdCliente BIGINT,
    @CPF VARCHAR(11)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 1
    FROM dbo.BENEFICIARIOS WITH(NOLOCK)
    WHERE IDCLIENTE = @IdCliente AND CPF = @CPF;
END;
GO