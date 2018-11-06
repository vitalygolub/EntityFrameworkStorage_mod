use Loyalty
declare @clientsid int
declare @password varchar(100)='1234567'
declare @mail varchar(100)='vitalygolub@hotmail.com'

select @clientsid=clientsid from clientspublic where Email=@mail 

UPDATE ClientsPublic SET 
	PwdHash=HASHBYTES('SHA1',cast(@password as varbinary(100))),
	PwdExpirationDate=CONVERT(DATE,DateAdd(y,100,GETDATE()))
  WHERE ClientsId=@ClientsId