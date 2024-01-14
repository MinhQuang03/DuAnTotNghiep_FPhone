Create or alter View vsell_online 
as Select
	Convert(date , GETDATE()) as [Datetime],
	Bill.TotalMoney as TotalMoney ,
	
 from Bill

 select * from  vsell_online

