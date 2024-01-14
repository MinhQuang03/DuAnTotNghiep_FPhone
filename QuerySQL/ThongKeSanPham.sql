--USE [PRO219_WebsiteBanDienThoai]
--GO

--/****** Object:  StoredProcedure [dbo].[Caclu_Statitics_PerDay]    Script Date: 23/11/2023 09:49:08 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--Alter Proc [dbo].[Caclu_Statitics_PerDay]
--AS 
--BEGIN
-----------------------------Declare và Set--------------------------------------------------------
--declare @timefrom Datetime,
--		@timeto Datetime,
--		@rate decimal(18,2),
--		@pecent decimal(18,2),
--		@topquantity nvarchar(MAX),
--		@trahang decimal(18,2),
--		@totalmoneyafter decimal(18,2)
--	Set @timefrom = Coalesce(@timefrom, DATEADD(day, -8,  DATETRUNC( day,GETDATE())))
--	Set @timeto = Coalesce(@timeto ,DATEADD(day, -4, DATETRUNC( day, GETDATE())))
--	Set @rate = CASE 
--							WHEN(Select SUM(GiaTien)
--								 From #TraHang 
--								 Where NgayTra > @timefrom and NgayTra < @timeto ) is not null 
--									THEN((Select SUM(TotalMoney) 
--										  From Bill
--					                      Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
--	                                        -   
--			                             ((Select SUM(TotalMoney) as TempTotalMoney
--									       FROM Bill		
--					                       Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom))))
--				                            -
--										  (Select SUM(GiaTien)
--									      From #TraHang 
--						                  Where NgayTra > @timefrom and NgayTra < @timeto )) --- Có sp trả
--							WHEN (Select SUM(GiaTien)
--								  From #TraHang 
--								  Where NgayTra > @timefrom and NgayTra < @timeto ) is null 
--									THEN 
--										(Select SUM(TotalMoney) 
--										 From Bill
--									   	 Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
--												 -   
--					                   ((Select 
--					                   SUM(TotalMoney) as TempTotalMoney
--					                   FROM Bill		
--					                   Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom))))  -- không có sp trả lại
--						END	
						
--	 Set @pecent = (@rate / (Select SUM(TotalMoney) 
--					From Bill
--					Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto ) ) * 100  
			
--	Set @topquantity =	(Select P.PhoneName FROM
--									(Select 
--										MAX(A.TopPhone) TopPhone , 
--										MAX(IdPhoneDetail) IdPhoneDetail
--									  From(
--										Select Count(IdPhoneDetail)  as TopPhone , IdPhoneDetail
--										From BillDetails
--										Join Bill as B on B.Id = BillDetails.IdBill
--										Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto 
--										Group By IdPhoneDetail
--										) As A 	
--									) AS C
--								Join PhoneDetailds PD on PD.Id = C.IdPhoneDetail
--								Join Phones P on P.Id = PD.IdPhone)	
--		Set @trahang = (Select SUM(GiaTien)
--						From #TraHang 
--						Where NgayTra > @timefrom and NgayTra < @timeto ) 

--		Set @totalmoneyafter = (Select SUM(TotalMoney) From Bill Where PaymentDate > @timefrom and PaymentDate < @timeto) - @trahang

--	--------------------------------------------------------------------------------------------------------------------------------------	

--	------Table TraHang---
--	--Drop table if exists #TraHang
--	--Create Table #TraHang
--	--(
--	--	Id nvarchar(max),
--	--	Imei nvarchar(max),
--	--	TenSP nvarchar(max),
--	--	GiaTien decimal(18,2),
--	--	NgayTra DateTime,
--	--	StatusDetail int
--	--)
--	--insert into #TraHang(Id,Imei,TenSP,GiaTien,NgayTra,StatusDetail)
--	--values('a30a5f27-13ae-4998-9b9c-15090da6158e','449839503869130','SamSung S23 Ultra' , 300000 ,DATEADD(day, -1,  DATETRUNC( day,GETDATE())), 1 ),
--	--('390aed22-0c58-43c0-b569-4e1e8b3799a9','106763158140210','SamSung S23 Ultra' , 200000 ,DATEADD(day, -1,  DATETRUNC( day,GETDATE())), 2 ),
--	--('98546c59-c7e7-4814-bef8-14146a1e0080','568258296514650','SamSung S23 Ultra' , 100000 ,DATEADD(day, -1,  DATETRUNC( day,GETDATE())), 3 ),
--	--('38e21499-9e5f-4dec-ac1d-fa4d366cc5a2','071062015372791','SamSung S23 Ultra' , 400000 ,DATEADD(day, -1,  DATETRUNC( day,GETDATE())), 4 )
--	--select * from #TraHang

--	------------------------INSERT--------------------------------------------------	
--	Insert Into SellDailys(Id,CreateTime,TotalMoneys,TotalQuantity,
--	SellOnl,SellOff,BestSeller, [Status])
	
--	----------------------------------------------------------------------------------------		
	
--	Select
--		NEWID() as Id ,
--		@timefrom as CreateTime ,
--		CASE WHEN @trahang is not null and SUM(Bill.TotalMoney) is not null  THEN SUM(Bill.TotalMoney) - @trahang 
--			 WHEN SUM(Bill.TotalMoney) is null THEN 0
--			 WHEN SUM(Bill.TotalMoney) is not null THEN SUM(Bill.TotalMoney) END 
--			as TotalMoneys,	
--		(SELECT COUNT(BillDetails.IdPhoneDetail) 
--				FROM BillDetails
--				join Bill as B on B.Id = BillDetails.IdBill
--				Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto ) as TotalQuantity ,
		
--		COUNT(CASE
--					WHEN Bill.StatusPayment = 1 THEN 1 END) as SellOnl,
--		COUNT(CASE
--			WHEN Bill.StatusPayment = 0 THEN 1 END) as SellOff,
--		--Xử lý top sell
--		CASE WHEN @topquantity is NULL THEN N'Không có sp bán hôm nay'
--			 WHEN @topquantity IS NOT NULL THEN @topquantity 
--			 END as BestSeller,
--		----Xử lý doanh thu 
--		 CASE  WHEN SUM(TotalMoney) is null THEN 'Không có doanh thu'
--			   WHEN (( Select 
--				SUM(TotalMoney) as TempTotalMoney
--				FROM Bill		
--		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) is NULL THEN CASE WHEN @trahang is null THEN   CONCAT(N'Doanh thu tăng :' , '0' , '%') END
--			   WHEN SUM(TotalMoney) > (( Select 
--				SUM(TotalMoney) as TempTotalMoney
--				FROM Bill		
--		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu tăng :' , @pecent , '% ') 
--			   WHEN SUM(TotalMoney) < (( Select 
--				SUM(TotalMoney) as TempTotalMoney
--				FROM Bill		
--		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu giảm :' , @pecent , '% ')
--			   WHEN SUM(TotalMoney) > (( Select 
--				SUM(TotalMoney) as TempTotalMoney
--				FROM Bill		
--		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN N'Doanh thu không đổi'
--			   END as [Status]
--			--Status : 1 da xac nhan , 2 cho ,3 dang giao , 4 da giao  , 5 huy
--			--StsPayment : 1 da thah toan , 2 chua thanh toan
--			--deliveryPaymentMethod : VNPAY , COD, TT
--	From Bill
--	--join BillDetails as bd on bd.IdBill = Bill.Id
--	Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto 
--	AND Bill.[Status] = 1
	--------------------------------------------------------------------------------------------------------
--END
--GO

--select * from SellDailys
--Delete SellDailys





--Alter Proc [dbo].[Caclu_Statitics_PerDay]
--AS 
--BEGIN

------Table Thong Ke San Pham Yeu Thich---
	--Drop table if exists #TKSP
--Create Table #TKSP
--(
	--KhuVuc nvarchar(100),
	--HangSanXuat nvarchar(100),
	--MauSac nvarchar(100),
	--SLMauSac int,
	--PhanKhucGia decimal(18,2),
	--PhanKhucNguoiDung nvarchar(100)
--)
declare @timefrom Datetime,
		@timeto Datetime
	Set @timefrom = Coalesce(@timefrom, DATEADD(month, -3,  DATETRUNC( day,GETDATE())))
	Set @timeto = Coalesce(@timeto ,DATEADD(day, 0, DATETRUNC( day, GETDATE())))
--insert into #TKSP(MauSac)
Select A.TOPPHONE , a.TOPPHONE FROM(select 
	(Select MAX(Colors.[Name]) AS TOPPHONE from PhoneDetailds 
	join Colors on Colors.Id = PhoneDetailds.IdColor 
	join BillDetails on PhoneDetailds.Id = BillDetails.IdPhoneDetail
	where PhoneDetailds.Id = BillDetails.IdPhoneDetail)AS TOPPHONE
From Bill 
join BillDetails on Bill.Id = BillDetails.IdBill
	Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto 
	AND Bill.[Status] = 1 and
	Bill.Id = BillDetails.IdBill )AS A

	
--END

 select (select COUNT(IdPhoneDetail) from BillDetails WHERE Bill.Id = BillDetails.IdBill) from Bill



