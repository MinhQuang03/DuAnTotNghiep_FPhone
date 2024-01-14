Create View vOverView
AS Select TOP(1)
--------------------------------
	(Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 3 ) AS DoanhThuThangNay , 
	----------
	(Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and [Status] = 3 ) AS DoanhThuThangTruoc ,
	----------
	(Select COUNT(Id) from Reviews
	WHERE [DateTime] > DATETRUNC(month ,GETDATE()) And [DateTime] < Getdate() ) AS DanhGiaThangNay,
	---------
	(Select COUNT(Id) from Reviews
	WHERE [DateTime] > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And [DateTime] < DATETRUNC(month ,GETDATE()) ) AS DanhGiaThangTruoc , 
	---------
	(select COUNT(Bill.Id) from Bill
	WHERE Bill.PaymentDate > DATETRUNC(month ,GETDATE()) And Bill.PaymentDate < Getdate() and Bill.[Status] = 3 and Bill.deliveryPaymentMethod = 'TT' ) as BillOffThangNay ,
	---------
	(select COUNT(Bill.Id) from Bill
	WHERE Bill.PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And Bill.PaymentDate <DATETRUNC(month ,GETDATE()) and Bill.[Status] = 3 and Bill.deliveryPaymentMethod = 'TT') as BillOffThangTruoc , 
	---------
	(select COUNT(Bill.Id) from Bill
	WHERE Bill.PaymentDate > DATETRUNC(month ,GETDATE()) And Bill.PaymentDate < Getdate() and Bill.[Status] = 3 and( Bill.deliveryPaymentMethod = 'VNPAY'or Bill.deliveryPaymentMethod = 'COD'  ) ) as BillOnlThangNay ,
	---------
	(select COUNT(Bill.Id) from Bill
	WHERE Bill.PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And Bill.PaymentDate <DATETRUNC(month ,GETDATE()) and Bill.[Status] = 3 and ( Bill.deliveryPaymentMethod = 'VNPAY'or Bill.deliveryPaymentMethod = 'COD'  )) as BillOnlThangTruoc ,
	--------
	(select COUNT(Id) from Bill 
	WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 3 ) AS BillThanhCong ,
	------
	(Select COUNT(Id) From Bill 
	WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 5 ) AS BillThatbai
	------
	
From Bill
LEft join BillDetails  on  BillDetails.IdBill = Bill.Id

select * from Bill
select * from vOverView