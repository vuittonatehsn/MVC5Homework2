CREATE VIEW [dbo].[MemberView]
	AS 
	
	
	select 
	distinct(q.客戶名稱),
	(select  count(a.id) from 客戶資料 a inner join 客戶聯絡人 b on a.id = b.客戶Id) as 聯絡人數量
	,
	(select  count(a.id) from 客戶資料 a inner join 客戶銀行資訊 c on a.id = c.客戶Id) as 銀行帳戶數量
	From [客戶資料] q INNER JOIN  客戶聯絡人 b ON q.Id = b.客戶Id
		INNER JOIN 客戶銀行資訊 c ON q.Id = c.客戶Id