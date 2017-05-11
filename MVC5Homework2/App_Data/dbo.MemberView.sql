CREATE VIEW dbo.MemberView
AS

select a.客戶名稱, (SELECT        COUNT(b.Id)
					FROM           dbo.客戶聯絡人 b 
					WHERE a.Id = b.客戶Id AND (b.IsDeleted <> 1) 
					) AS 聯絡人數量
				, (SELECT        COUNT(c.Id)
					FROM            dbo.客戶銀行資訊 AS c 
					WHERE a.Id = c.客戶Id AND (c.IsDeleted <> 1)
					) AS 銀行帳戶數量
FROM 客戶資料 a 