IF OBJECT_ID('dbo.FI_SP_PesqBeneficiario', 'P') IS NOT NULL
    DROP PROC dbo.FI_SP_PesqBeneficiario;
GO

CREATE PROC dbo.FI_SP_PesqBeneficiario
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
    WHERE IDCLIENTE = @Id
    ORDER BY NOME;
END;
GO