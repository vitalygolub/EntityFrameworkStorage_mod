use testIdentityServer4;
declare @root nvarchar(100);
set @root='https://lv-nv-pp03.new-vision.lv/IdentityServer4/mvcclient';
set @root='http://localhost:5002';


UPDATE [testIdentityServer4].[dbo].[ClientPostLogoutRedirectUris]
   SET [PostLogoutRedirectUri] = @root +'/signout-callback-oidc'

UPDATE [testIdentityServer4].[dbo].[ClientRedirectUris]
   SET [RedirectUri] = @root+'/signin-oidc'

