
---------------------------------------------------------
Create Proc [dbo].[PROC_RANKING_PERMONTH]
AS 
BEGIN
declare @timefrom Datetime,
		@timeto  Datetime
	Set @timefrom = Coalesce(@timefrom, DATEADD(year, -3,  DATETRUNC( day,GETDATE())))
	Set @timeto = Coalesce(@timeto ,DATEADD(day, +1, DATETRUNC( day, GETDATE())))
	
	-------------Update IF EXISTS------------------------------
	Delete Ranks
	---------------------------INSERT INTO---------------------------------------------
	INSERT INTO Ranks(Id,STT,Username,IdAccount,Point,TotalPoint,Ranking,DateRank,Policies,Benefits,[Status])
	
	---------------------------SELECT VALUES---------------------------------------------
	SELECT 
	B.Id,
	ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) as STT,
	B.Username,
	B.IdAccount,
	B.Point,
	B.TotalPoint,
	B.Ranking,
	B.DateRank,
	B.Policies,
	B.Benefits,
	B.[Status]
	FROM(SELECT 
	A.Id,
	ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) as STT,
	A.Username,
	A.IdAccount,
	A.Point,
	A.TotalPoint,
	A.Ranking,
	A.DateRank,
	A.Policies,
	A.Benefits,
	A.[Status]
	FROM(SELECT
	NEWID() as Id,
	ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) as STT,
	Accounts.[Name] as Username,
	Accounts.Id	as IdAccount,
	CAST((SUM(TotalMoney))/1000 as int) as Point ,
	Accounts.Points as TotalPoint,
	CASE 
		WHEN cast((SUM(TotalMoney))/1000 as int) is null THEN N'UnRank'   
		WHEN cast((SUM(TotalMoney))/1000 as int) >= 0 and cast((SUM(TotalMoney))/1000 as int) <= 3000 THEN N'FPL_Bzone'
		WHEN cast((SUM(TotalMoney))/1000 as int) >= 3000 and cast((SUM(TotalMoney))/1000 as int) <= 15000 THEN N'FPL_Sivel'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 15000 and cast((SUM(TotalMoney))/1000 as int) <= 35000 THEN N'FPL_Gold'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 35000 and cast((SUM(TotalMoney))/1000 as int) <= 70000 THEN N'FPL_Diamond'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 70000  THEN N'FPL_VVIP'
		END as Ranking,
		CAST(@timefrom as date) as DateRank , 
	CASE 
		WHEN cast((SUM(TotalMoney))/1000 as int) is null THEN 'NULL'
		WHEN cast((SUM(TotalMoney))/1000 as int) >= 0 and cast((SUM(TotalMoney))/1000 as int) <= 3000 THEN N'0'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 3000 and cast((SUM(TotalMoney))/1000 as int) <= 15000 THEN N'1'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 15000 and cast((SUM(TotalMoney))/1000 as int) <= 35000 THEN N'2'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 35000 and cast((SUM(TotalMoney))/1000 as int) <= 70000 THEN N'3'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 70000  THEN N'4'
	END as Policies,
	CASE 
		WHEN cast((SUM(TotalMoney))/1000 as int) is null THEN 'NULL'
		WHEN cast((SUM(TotalMoney))/1000 as int) >= 0 and cast((SUM(TotalMoney))/1000 as int) <= 3000 THEN N'0'
		WHEN cast((SUM(TotalMoney))/1000 as int) >= 3000 and cast((SUM(TotalMoney))/1000 as int) <= 70000 THEN N'1'
		WHEN cast((SUM(TotalMoney))/1000 as int) > 70000  THEN N'2'
	END as Benefits	,
	1 as [Status]
From Bill
join Accounts on Accounts.Id = Bill.IdAccount
Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto and StatusPayment =1
Group by Accounts.[Name] , Accounts.Points , Accounts.Id) as A
order by A.Point DESC offset 0 rows ) as B

END
--------------------END------------------------------------------
EXEC [dbo].[PROC_RANKING_PERMONTH]
select * from Ranks
delete Ranks



