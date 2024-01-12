Create or alter View VTop5_PhoneSell
as
Select TOP(5)
	PD.Id,
	P.PhoneName,
	PD.Price,
	IMG.[Image],
	COUNT(BD.Id) Total
from Bill A
Join BillDetails BD on BD.IdBill = A.Id
join PhoneDetailds PD on PD.Id = Bd.IdPhoneDetail
Join Phones P On P.Id = PD.IdPhone
Left Join ListImage IMG on IMG.IdPhoneDetaild = PD.Id 
WHERE PaymentDate > DATEADD(day, -80,  DATETRUNC( day,GETDATE())) and PaymentDate < GETDATE()
group by PD.Id,P.PhoneName,PD.Price,IMG.[Image]
order by Total DESC
select * from BillDetails
--------------------------------------
select * from VTop5_PhoneSell

