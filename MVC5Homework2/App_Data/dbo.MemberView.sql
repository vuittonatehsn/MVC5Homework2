CREATE VIEW dbo.MemberView
AS
SELECT DISTINCT q.客戶名稱,
                             (SELECT        COUNT(a.Id) AS Expr1
                               FROM            dbo.客戶資料 AS a INNER JOIN
                                                         dbo.客戶聯絡人 AS b ON a.Id = b.客戶Id
                               WHERE        (b.IsDeleted <> 1)) AS 聯絡人數量,
                             (SELECT        COUNT(a.Id) AS Expr1
                               FROM            dbo.客戶資料 AS a INNER JOIN
                                                         dbo.客戶銀行資訊 AS c ON a.Id = c.客戶Id
                               WHERE        (c.IsDeleted <> 1)) AS 銀行帳戶數量
FROM            dbo.客戶資料 AS q INNER JOIN
                         dbo.客戶聯絡人 AS b ON q.Id = b.客戶Id INNER JOIN
                         dbo.客戶銀行資訊 AS c ON q.Id = c.客戶Id
WHERE        (q.IsDeleted <> 1)
GO
