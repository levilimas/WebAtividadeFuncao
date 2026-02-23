IF OBJECT_ID('dbo.FI_SP_ConsBeneficiario', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_ConsBeneficiario;
GO

CREATE PROC dbo.FI_SP_ConsBeneficiario
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID,
        NOME,
        IDCLIENTE,
        CPF
    FROM dbo.BENEFICIARIOS WITH(NOLOCK)
    WHERE ID = @Id;
END;
GO