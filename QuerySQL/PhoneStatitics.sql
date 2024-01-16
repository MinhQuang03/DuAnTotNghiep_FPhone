Select top 1
----------PhoneName
(Select 
A.PhoneName from (Select TOp 1
	P.PhoneName  PhoneName,
	COUNT(P.PhoneName) CountPhoneName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE B.PaymentDate > DATETRUNC(month ,GETDATE()) And B.PaymentDate < Getdate()
Group By P.PhoneName
OrDer BY CountPhoneName DESC ) A ) NamePhoneThangNay, --null
-----------CountPHoneName
(Select 
A.CountPhoneName from (Select TOp 1
	P.PhoneName  PhoneName,
	COUNT(P.PhoneName) CountPhoneName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE B.PaymentDate > DATETRUNC(month ,GETDATE()) And B.PaymentDate < Getdate()
Group By P.PhoneName
OrDer BY CountPhoneName DESC ) A ) SlBanThangNay,
-------------------------------------
(Select 
A.PhoneName from (Select TOp 1
	P.PhoneName  PhoneName,
	COUNT(P.PhoneName) CountPhoneName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and B.[Status] = 3 
Group By P.PhoneName
OrDer BY CountPhoneName DESC ) A ) NamePhoneThangTruoc , --null
-------------------------------------------------
(Select 
A.CountPhoneName from (Select TOp 1
	P.PhoneName  PhoneName,
	COUNT(P.PhoneName) CountPhoneName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and B.[Status] = 3 
Group By P.PhoneName
OrDer BY CountPhoneName DESC ) A ) SlBanThangTruoc  ,
-------------------------------------------------------------------
(Select 
A.ColorName from (Select TOp 1
	C.Name  ColorName,
	COUNT(C.Name) CountColorName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE B.PaymentDate > DATETRUNC(month ,GETDATE()) And B.PaymentDate < Getdate()
Group by C.Name
OrDer BY CountColorName DESC ) A ) NameColorThangNay , --null
-----------------------------------------------------------------------
(Select 
A.CountColorName from (Select TOp 1
	C.Name  ColorName,
	COUNT(C.Name) CountColorName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE B.PaymentDate > DATETRUNC(month ,GETDATE()) And B.PaymentDate < Getdate()
Group by C.Name
OrDer BY CountColorName DESC ) A ) SlColorThangNay ,
----------------------------------------------------------------------------
(Select 
A.ColorName from (Select TOp 1
	C.Name  ColorName,
	COUNT(C.Name) CountColorName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and B.[Status] = 3 
Group by C.Name
OrDer BY CountColorName DESC ) A ) NameColorThangTruoc , --null
---------------------------------------------------------------------------
-----------------------------------------------------------------------
(Select 
A.CountColorName from (Select TOp 1
	C.Name  ColorName,
	COUNT(C.Name) CountColorName
From Bill B
Left Join BillDetails DB on DB.IdBill = B.Id
Left Join PhoneDetailds PD on PD.Id = DB.IdPhoneDetail
Left Join Colors C on C.Id = PD.IdColor
Left Join Phones P on P.Id = PD.IdPhone
WHERE PaymentDate > DATEADD(MONTH, -1, DATETRUNC(month ,GETDATE())) And PaymentDate < DATETRUNC(month ,GETDATE()) and B.[Status] = 3 
Group by C.Name
OrDer BY CountColorName DESC ) A ) SlColorThangTruoc ,
----------------------------------------------------------------------
Case When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 0 and (DATEPART(hour, PaymentDate)) < 11) is not null 
	THEN (Select TOP 1
		COUNT(B.PaymentDate )
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 0 and (DATEPART(hour, PaymentDate)) < 12)
		When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 0 and (DATEPART(hour, PaymentDate)) < 12) is null 
	THEN 0
	END 'BuoiSang' ,
--------------------------------------------
Case When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 13 and (DATEPART(hour, PaymentDate)) < 18) is not null 
	THEN (Select TOP 1
		COUNT(B.PaymentDate )
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 13 and (DATEPART(hour, PaymentDate)) < 18)
		When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 13 and (DATEPART(hour, PaymentDate)) < 18) is null
		THEN 0
	
	END 'BuoiChieu',
	-----------------------------------------
	Case When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 19 and (DATEPART(hour, PaymentDate)) < 23) is not null 
	THEN (Select TOP 1
		COUNT(B.PaymentDate )
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 19 and (DATEPART(hour, PaymentDate)) < 23)
		When(Select TOP 1
		B.PaymentDate 
		From Bill B 
		Where (DATEPART(hour, PaymentDate)) >= 19 and (DATEPART(hour, PaymentDate)) < 23) is null
		THEN 0
	
	END 'BuoiToi' , 

	--------------------------------------------
	-----------------------KhoangGia
	Case WHEN (Select TOP 1 Price From BillDetails Where Price > 0 and Price < 10000000) is null then 0
			WHEN (Select TOP 1 Price From BillDetails Where Price > 0 and Price < 10000000) is not null 
			THEN (Select TOP 1 COUNT(Price) From BillDetails Where Price > 0 and Price < 10000000)	
	END 'PhanKhucThap' ,
	--------------------------------------------
	Case WHEN (Select TOP 1 Price From BillDetails Where Price >= 10000000 and Price < 30000000) is null then 0
			WHEN (Select TOP 1 Price From BillDetails Where Price >= 10000000 and Price < 30000000) is not null 
			THEN (Select TOP 1 COUNT(Price) From BillDetails Where Price >= 10000000 and Price < 30000000)	
	END 'PhanKhucTrungCap',
		--------------------------------------------
	Case WHEN (Select TOP 1 Price From BillDetails Where Price >= 30000000 ) is null then 0
			WHEN (Select TOP 1 Price From BillDetails Where Price >= 30000000 ) is not null 
			THEN (Select TOP 1 COUNT(Price) From BillDetails Where Price >= 30000000 )	
	END 'PhanKhucCaoCap'
From Bill
