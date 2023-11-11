-- input variables -----------------------------
	if (object_id('tempdb..#t_tmp_var') is not null) drop table #t_tmp_var;
	create table #t_tmp_var (name nvarchar(max), value nvarchar(max));
	insert into #t_tmp_var
	(
		name
	  , value
	)
	select
		 name, value
	from (values
		 ('@system_runtime_serialization_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll')
		,('@newtonsoft_json_location', N'C:\grm\13\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.web\bin\Debug\Newtonsoft.Json.dll')
		,('@build_location', N'C:\grm\13\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.web\bin\Debug')
	) t (name, value);
	go
-- assembiles -------------
-- System.Runtime.Serialization -------------
/*
declare @system_runtime_serialization_location nvarchar(4000);
select
	 @system_runtime_serialization_location = t.value
from #t_tmp_var t
where t.name = '@system_runtime_serialization_location'
if (not exists (select 1 from sys.assemblies where name = N'System.Runtime.Serialization')) create ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
-- Newtonsoft.Json ---
declare @newtonsoft_json_location nvarchar(4000);
select
	 @newtonsoft_json_location = t.value
from #t_tmp_var t
where t.name = '@newtonsoft_json_location';
if (not exists (select 1 from sys.assemblies where name = N'Newtonsoft.Json')) create ASSEMBLY [Newtonsoft.Json] from @newtonsoft_json_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [Newtonsoft.Json] from @newtonsoft_json_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
*/

--------------------------------
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max);
select
	 @build_location = t.value
from #t_tmp_var t
where t.name = '@build_location';
declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.web.dll';
-- drop existing --------
if (object_id('.dbo.f_clr_ftp_request') is not null) drop function dbo.f_clr_ftp_request;
if (object_id('.dbo.f_clr_web_get') is not null) drop function dbo.f_clr_web_get;
if (object_id('.dbo.f_clr_web_get_parallel') is not null) drop function dbo.f_clr_web_get_parallel;
if (object_id('.dbo.f_clr_web_post') is not null) drop function dbo.f_clr_web_post;
if (object_id('.dbo.f_clr_web_request_with_network_credentials') is not null) drop function dbo.f_clr_web_request_with_network_credentials;
if (object_id('.dbo.__________f_clr_web_request_old') is not null) drop function dbo.__________f_clr_web_request_old;

if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.web')) drop ASSEMBLY [mssql.extensions.web];
-- create new --------------------
create assembly [mssql.extensions.web] from @build_file_name with permission_set = unsafe;
go

CREATE FUNCTION [dbo].[f_clr_ftp_request](@url [nvarchar](max), @method [nvarchar](max), @file_name [nvarchar](max), @user_name [nvarchar](max), @password [nvarchar](max), @null_when_error [bit])
RETURNS  TABLE (
	[row_type] [nvarchar](max) NULL,
	[row_key] [nvarchar](max) NULL,
	[row_value] [nvarchar](max) NULL
)
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[ProcessFtpRequest];
go

CREATE FUNCTION [dbo].[f_clr_web_get](@url [nvarchar](max), @headersInUrlFormat [nvarchar](max), @null_when_error [bit]) returns nvarchar(max) 
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[Get];
go

CREATE FUNCTION [dbo].[f_clr_web_get_parallel](@parallel_web_request_url_input_json [nvarchar](max), @header_json [nvarchar](max), @cookie_json [nvarchar](max), @null_when_error [bit]) RETURNS [nvarchar](max)
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[GetParallel];
go

CREATE FUNCTION [dbo].[f_clr_web_post](@url [nvarchar](max), @body [nvarchar](max), @headersInUrlFormat [nvarchar](max), @null_when_error [bit]) RETURNS [nvarchar](max)
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[Post];
go

CREATE FUNCTION [dbo].[f_clr_web_request_with_network_credentials](@url [nvarchar](max), @method [nvarchar](max), @body [nvarchar](max), @content_type [nvarchar](max), @code_page [int], @headers_in_url_format [nvarchar](max), @cookies_in_url_format [nvarchar](max), @allow_auto_redirect [bit], @file_name [nvarchar](max), @network_credential_user [nvarchar](max), @network_credential_password [nvarchar](max), @null_when_error [bit])
RETURNS  TABLE (
	[row_type] [nvarchar](max) NULL,
	[row_key] [nvarchar](max) NULL,
	[row_value] [nvarchar](max) NULL
)
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[ProcessWebRequestWithNetworkCredentials];
go


/*
CREATE FUNCTION [dbo].[dbo.f_clr_web_get_hc](@url nvarchar(max), @headersInUrlFormat nvarchar(max), @filesInUrlFormat nvarchar(max), @nullWhenError bit) returns nvarchar(max) 
with execute as owner 
as external name [mssql.extensions.web].[bi_dev.sql.mssql.extensions.web.Utils].[Post_HC];
go
*/

