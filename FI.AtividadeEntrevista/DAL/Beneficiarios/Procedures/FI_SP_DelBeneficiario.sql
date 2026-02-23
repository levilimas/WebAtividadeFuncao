IF OBJECT_ID('dbo.FI_SP_DelBeneficiario', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_DelBeneficiario;
GO

CREATE PROC dbo.FI_SP_DelBeneficiario
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.BENEFICIARIOS
    WHERE ID = @Id;
END;
GO