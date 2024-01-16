--USE [PRO219_WebsiteBanDienThoai]
--GO
--select *from BillDetails
----/****** Object:  StoredProcedure [dbo].[Caclu_Statitics_PerDay]    Script Date: 23/11/2023 09:49:08 ******/
--EXEC [dbo].[Caclu_Statitics_PerDay]
--Select * from SellDailys
--delete SellDaily
--DELETE [Ranks]
----SET ANSI_NULLS ON
----GO

--SET QUOTED_IDENTIFIER ON
--GO
Create Proc [dbo].[Caclu_Statitics_PerDay]
AS 
BEGIN

-----------------------------Declare và Set--------------------------------------------------------

declare @timefrom Datetime,
		@timeto Datetime,
		@rate decimal(18,2),
		@pecent decimal(18,2),
		@topquantity nvarchar(MAX),
		@trahang decimal(18,2),
		@totalmoneyafter decimal(18,2)
	Set @timefrom = Coalesce(@timefrom, DATEADD(day, -2,  DATETRUNC( day,GETDATE())))
	Set @timeto = Coalesce(@timeto ,DATEADD(minute, -1, DATETRUNC( day, GETDATE())))
	Set @rate = CASE 
							WHEN(Select SUM(Price)
								 From Refund 
								 Where CreateDate > @timefrom and CreateDate < @timeto ) is not null 
									THEN((Select SUM(TotalMoney) 
										  From Bill
					                      Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
	                                        -   
			                             ((Select SUM(TotalMoney) as TempTotalMoney
									       FROM Bill		
					                       Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom))))
				                            -
										  (Select SUM(Price)
									      From Refund 
						                  Where CreateDate > @timefrom and CreateDate < @timeto )) --- Có sp trả
							WHEN (Select SUM(Price)
								  From Refund 
								  Where CreateDate > @timefrom and CreateDate < @timeto ) is null 
									THEN 
										(Select SUM(TotalMoney) 
										 From Bill
									   	 Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
												 -   
					                   ((Select 
					                   SUM(TotalMoney) as TempTotalMoney
					                   FROM Bill		
					                   Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom))))  -- không có sp trả lại
									   END 
	 Set @pecent = (@rate / (Select SUM(TotalMoney) 
					From Bill
					Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto ) ) * 100  
			
	 Set @topquantity =	(Select P.PhoneName FROM
									(Select 
										MAX(A.TopPhone) TopPhone , 
										MAX(IdPhoneDetail) IdPhoneDetail
									  From(
										Select Count(IdPhoneDetail)  as TopPhone , IdPhoneDetail
										From BillDetails
										Join Bill as B on B.Id = BillDetails.IdBill
										Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto 
										Group By IdPhoneDetail
										) As A 	
									) AS C
								Join PhoneDetailds PD on PD.Id = C.IdPhoneDetail
								Join Phones P on P.Id = PD.IdPhone)	
								
	---------------------------------Xử lý trả hàng--------------------------------------------------------------------------------------
		Set @trahang = (Select SUM(Price)
						From Refund 
						Where CreateDate > @timefrom and CreateDate < @timeto ) 

		Set @totalmoneyafter = (Select SUM(TotalMoney) From Bill Where PaymentDate > @timefrom and PaymentDate < @timeto) - @trahang

	--------------------------------------------------------------------------------------------------------------------------------------	

	------------------------INSERT--------------------------------------------------	
Insert Into SellDailys(Id,CreateTime,TotalMoneys,Refund,TotalQuantity,
	SellOnl,SellOff,BestSeller, [Status])
	
	----------------------------------------------------------------------------------------		
	
	Select
	    ----Id---
		NEWID() as Id ,
		------Createtime---
		@timefrom as CreateTime ,
        -----TotalMoneys
		CASE WHEN @trahang is not null and SUM(Bill.TotalMoney) is not null  THEN SUM(Bill.TotalMoney) - @trahang 
			 WHEN SUM(Bill.TotalMoney) is null THEN 0
			 WHEN SUM(Bill.TotalMoney) is not null THEN SUM(Bill.TotalMoney) END 
			as TotalMoneys,	
		---------
		CASE WHEN @trahang is null THEN 0 
			 WHEN @trahang > 0 THEN @trahang
		END as Refund ,
		---------
		(SELECT COUNT(BillDetails.IdPhoneDetail) 
				FROM BillDetails
				join Bill as B on B.Id = BillDetails.IdBill
				Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto ) as TotalQuantity ,
		
		COUNT(CASE
					WHEN Bill.deliveryPaymentMethod = 'VNPay' THEN 1 
					WHEN Bill.deliveryPaymentMethod = 'COD' THEN 1
			END) as SellOnl,
		COUNT(CASE
					WHEN Bill.deliveryPaymentMethod = 'TT' THEN 1 END) as SellOff,
		--Xử lý top sell
		CASE WHEN @topquantity is NULL THEN N'Không có sp bán hôm nay'
			 WHEN @topquantity IS NOT NULL THEN @topquantity 
			 END as BestSeller,
		--Xử lý doanh thu 
		 CASE  WHEN SUM(TotalMoney) is null OR SUM(TotalMoney) = 0 THEN 'Không có doanh thu'
			   WHEN (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) is NULL THEN   CONCAT(N'Doanh thu tăng :' , '100' , '%')
			   WHEN SUM(TotalMoney) > (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu tăng :' , @pecent , '% ') 
			   WHEN SUM(TotalMoney) < (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu giảm :' , @pecent , '% ')
			   WHEN SUM(TotalMoney) > (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(day, -1, DATETRUNC(day,@timefrom)))) THEN N'Doanh thu không đổi'
			   END as [Status]
			--Status : 1 da xac nhan , 2 cho ,3 dang giao , 4 da giao  , 5 huy
			--StsPayment : 1 da thah toan , 2 chua thanh toan
			--deliveryPaymentMethod : VNPAY , COD, TT
	From Bill
	--join BillDetails as bd on bd.IdBill = Bill.Id
	Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto 
	AND ( Bill.[Status] = 1  or  Bill.[Status] = 2 or  Bill.[Status] = 3) and StatusPayment = 1
END
GO
select *from BillDetails
 ----------------------------------------PER MONTH----------------------------------------------------------
 Create Proc [dbo].[Caclu_Statitics_PerMonth]
AS 
BEGIN
-----------------------------Declare và Set--------------------------------------------------------
declare @timefrom Datetime,
		@timeto Datetime,
		@rate decimal(18,2),
		@pecent decimal(18,2),
		@topquantity nvarchar(MAX),
		@trahang decimal(18,2),
		@totalmoneyafter decimal(18,2)
	Set @timefrom = Coalesce(@timefrom, DATEADD(MONTH, -1,  DATETRUNC( day,GETDATE())))
	Set @timeto = Coalesce(@timeto ,DATEADD(MINUTE, +15, DATETRUNC( day, GETDATE())))
	Set @rate = CASE 
							WHEN(Select SUM(Price)
								 From Refund 
								 Where CreateDate > @timefrom and CreateDate < @timeto ) is not null 
									THEN((Select SUM(TotalMoney) 
										  From Bill
					                      Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
	                                        -   
			                             ((Select SUM(TotalMoney) as TempTotalMoney
									       FROM Bill		
					                       Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom))))
				                            -
										  (Select SUM(Price)
									      From Refund 
						                  Where CreateDate > @timefrom and CreateDate < @timeto )) --- Có sp trả
							WHEN (Select SUM(Price)
								  From Refund 
								  Where CreateDate > @timefrom and CreateDate < @timeto ) is null 
									THEN 
										(Select SUM(TotalMoney) 
										 From Bill
									   	 Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto )
												 -   
					                   ((Select 
					                   SUM(TotalMoney) as TempTotalMoney
					                   FROM Bill		
					                   Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom))))  -- không có sp trả lại	
									   END
	 Set @pecent = (@rate / (Select SUM(TotalMoney) 
					From Bill
					Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto ) ) * 100  
			
	 Set @topquantity =	(Select P.PhoneName FROM
									(Select 
										MAX(A.TopPhone) TopPhone , 
										MAX(IdPhoneDetail) IdPhoneDetail
									  From(
										Select Count(IdPhoneDetail)  as TopPhone , IdPhoneDetail
										From BillDetails
										Join Bill as B on B.Id = BillDetails.IdBill
										Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto 
										Group By IdPhoneDetail
										) As A 	
									) AS C
								Join PhoneDetailds PD on PD.Id = C.IdPhoneDetail
								Join Phones P on P.Id = PD.IdPhone)	
								
	---------------------------------Xử lý trả hàng--------------------------------------------------------------------------------------
		Set @trahang = (Select SUM(Price)
						From Refund 
						Where CreateDate > @timefrom and CreateDate < @timeto ) 

		Set @totalmoneyafter = (Select SUM(TotalMoney) From Bill Where PaymentDate > @timefrom and PaymentDate < @timeto) - @trahang

	--------------------------------------------------------------------------------------------------------------------------------------	

	------------------------INSERT--------------------------------------------------	
	Insert Into SellMonthlys(Id,CreateTime,TotalMoneys,Refund,TotalQuantity,
	SellOnl,SellOff,BestSeller, [Status])
	
	----------------------------------------------------------------------------------------		
	
	Select
	    ----Id---
		NEWID() as Id ,
		------Createtime---
		@timefrom as CreateTime ,
        -----TotalMoneys
		CASE WHEN @trahang is not null and SUM(Bill.TotalMoney) is not null  THEN SUM(Bill.TotalMoney) - @trahang 
			 WHEN SUM(Bill.TotalMoney) is null THEN 0
			 WHEN SUM(Bill.TotalMoney) is not null THEN SUM(Bill.TotalMoney) END 
			as TotalMoneys,	
		---------
		CASE WHEN @trahang is null THEN 0 
			 WHEN @trahang > 0 THEN @trahang
		END as Refund ,
		---------
		(SELECT COUNT(BillDetails.IdPhoneDetail) 
				FROM BillDetails
				join Bill as B on B.Id = BillDetails.IdBill
				Where B.PaymentDate > @timefrom and B.PaymentDate < @timeto ) as TotalQuantity ,
		
		COUNT(CASE
					WHEN Bill.deliveryPaymentMethod = 'VNPay' THEN 1 
					WHEN Bill.deliveryPaymentMethod = 'COD' THEN 1
			END) as SellOnl,
		COUNT(CASE
					WHEN Bill.deliveryPaymentMethod = 'TT' THEN 1 END) as SellOff,
		--Xử lý top sell
		CASE WHEN @topquantity is NULL THEN N'Không có sp bán tháng này'
			 WHEN @topquantity IS NOT NULL THEN @topquantity 
			 END as BestSeller,
		--Xử lý doanh thu 
		 CASE  WHEN SUM(TotalMoney) is null OR SUM(TotalMoney) = 0 THEN 'Không có doanh thu'
			   WHEN (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom)))) is NULL THEN   CONCAT(N'Doanh thu tăng :' , '100' , '%')
			   WHEN SUM(TotalMoney) > (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu tăng :' , @pecent , '% ') 
			   WHEN SUM(TotalMoney) < (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom)))) THEN CONCAT(N'Doanh thu giảm :' , @pecent , '% ')
			   WHEN SUM(TotalMoney) > (( Select 
				SUM(TotalMoney) as TempTotalMoney
				FROM Bill		
		        Where PaymentDate =  DATEADD(MONTH, -1, DATETRUNC(day,@timefrom)))) THEN N'Doanh thu không đổi'
			   END as [Status]
			--Status : 1 da xac nhan , 2 cho ,3 dang giao , 4 da giao  , 5 huy
			--StsPayment : 1 da thah toan , 2 chua thanh toan
			--deliveryPaymentMethod : VNPAY , COD, TT
	From Bill
	--join BillDetails as bd on bd.IdBill = Bill.Id
	Where Bill.PaymentDate > @timefrom and Bill.PaymentDate < @timeto 
	AND Bill.[Status] = 4 and StatusPayment = 1
END
GO
select * from SellMonthly