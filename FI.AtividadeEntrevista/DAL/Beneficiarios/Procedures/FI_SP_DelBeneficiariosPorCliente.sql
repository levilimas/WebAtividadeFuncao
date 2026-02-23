IF OBJECT_ID('dbo.FI_SP_DelBeneficiariosPorCliente', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_DelBeneficiariosPorCliente;
GO

CREATE PROC dbo.FI_SP_DelBeneficiariosPorCliente
    @IdCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.BENEFICIARIOS
    WHERE IDCLIENTE = @IdCliente;
END;
GO