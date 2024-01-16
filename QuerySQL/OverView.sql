Create View vOverView
AS Select TOP(1)
--------------------------------
	Case 
		WHEN (Select SUM(TotalMoney) From Bill 
		WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 3 ) is null THEN 0
		WHEN (Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 3 ) is not null THEN (Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATETRUNC(month ,GETDATE()) And PaymentDate < Getdate() and [Status] = 3 )
	END 
	AS DoanhThuThangNay , 
	----------
	CASE 
		WHEN (Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and [Status] = 3 ) is null THEN 0
		WHEN (Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and [Status] = 3 ) > 0 Then
	(Select SUM(TotalMoney) From Bill 
	WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and [Status] = 3 ) END 
	AS DoanhThuThangTruoc ,
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

-----------------------------------------
Create View BillGanDay
as Select TOP(6)
ROW_NUMBER() OVER(ORDER BY(SELECT NULL)) as STT,
BillCode as MaHoaDon,
CreatedTime As NgayTao , 
[Name] AS TenNguoiMua,
TotalMoney as GiaTien,
[Status] TrangThaiGiaoHang,
StatusPayment as TrangThaiThanhToan
from Bill
Order BY CreatedTime DESC

Select * from BillGanDay