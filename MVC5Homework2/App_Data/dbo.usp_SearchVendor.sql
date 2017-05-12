CREATE PROCEDURE [dbo].[usp_SearchVendor]
	@param1 nvarchar(100)
AS
	SELECT a.Id, a.Email, a.地址, a.客戶名稱, a.統一編號, a.傳真, a.電話
		,b.分行代碼, b.帳戶名稱, b.帳戶號碼, b.銀行名稱
		,c.姓名, c.職稱, c.電話 AS 聯絡人電話, c.手機
	FROM 客戶資料 a INNER JOIN 客戶銀行資訊 b ON a.id = b.客戶Id 
			INNER JOIN 客戶聯絡人 c ON a.id = c.客戶Id 
	WHERE a.客戶名稱 like N'%'+@param1+'%' 
	OR b.銀行名稱 like N'%'+@param1+'%' 
	OR c.姓名 like N'%'+@param1+'%'
	AND a.IsDeleted != 1 AND b.IsDeleted != 1 AND c.IsDeleted != 1