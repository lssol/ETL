USE ERP_Document
GO
IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'ETL')
	BEGIN
		EXEC('CREATE SCHEMA [ETL]')
	END
GO
IF object_id('ETL.GetDocuments', 'p') IS NOT NULL
		DROP PROCEDURE ETL.GetDocuments
GO
CREATE PROC ETL.GetDocuments
		@Offset int,
		@BatchSize int 
AS 
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		  D.ID            AS DocumentId
		, D.CandidateID   AS CandidateId
	    , D.Title         AS Title
	    , D.Document      AS Document
	    , D.[Description] AS [Description]
	    , D.LanguageID    AS LanguageId
	    , D.Extension     AS Extension
	    , D.Created_Date  AS CreatedDate
	FROM Document D
	ORDER BY Id
	OFFSET @Offset ROWS
	FETCH NEXT @BatchSize ROWS ONLY
END

GO

-- set statistics time on
-- EXEC ETL.GetDocuments 100, 1000
-- set statistics time off